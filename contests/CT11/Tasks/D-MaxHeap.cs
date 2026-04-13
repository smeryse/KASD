using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT11.Tasks
{
    /// <summary>
    /// D. Групповой турнир — распределить результаты матчей через максимальный поток.
    /// Модель: источник → матчи (3 единицы потока), матчи → команды, команды → сток (очки).
    /// Каждый матч распределяет 3 очка между двумя командами: (3,0), (0,3), (2,1), (1,2).
    /// </summary>
    internal static class TaskD
    {
        private class Edge
        {
            public int To;
            public int Capacity;
            public int Flow;
            public int ReverseIndex;
        }

        public static void Solve()
        {
            var fs = new FastScanner(Console.OpenStandardInput());
            int N = fs.NextInt();

            char[][] table = new char[N][];
            for (int i = 0; i < N; i++)
                table[i] = fs.NextString().ToCharArray();

            int[] needed = new int[N];
            for (int i = 0; i < N; i++)
                needed[i] = fs.NextInt();

            // Подсчитываем текущие очки и список нерешённых матчей
            int[] currentPoints = new int[N];
            var matches = new List<(int i, int j)>();

            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    char c = table[i][j];
                    if (c == '.')
                    {
                        matches.Add((i, j));
                    }
                    else if (c == 'W') { currentPoints[i] += 3; }
                    else if (c == 'L') { currentPoints[j] += 3; }
                    else if (c == 'w') { currentPoints[i] += 2; currentPoints[j] += 1; }
                    else if (c == 'l') { currentPoints[i] += 1; currentPoints[j] += 2; }
                }
            }

            // Сколько очков ещё нужно каждой команде
            int[] remaining = new int[N];
            for (int i = 0; i < N; i++)
            {
                remaining[i] = needed[i] - currentPoints[i];
                if (remaining[i] < 0)
                {
                    // Уже набрали больше нужного — решение невозможно
                    // Но по гарантии существует, так что просто продолжаем
                }
            }

            // Строим сеть:
            // Источник S = 0
            // Узлы матчей: 1..M
            // Узлы команд: M+1..M+N
            // Сток T = M+N+1
            int M = matches.Count;
            int S = 0;
            int T = M + N + 1;
            int totalNodes = T + 1;

            var graph = new List<Edge>[totalNodes];
            for (int i = 0; i < totalNodes; i++) graph[i] = new List<Edge>();

            void AddEdge(int u, int v, int cap)
            {
                graph[u].Add(new Edge { To = v, Capacity = cap, Flow = 0, ReverseIndex = graph[v].Count });
                graph[v].Add(new Edge { To = u, Capacity = 0, Flow = 0, ReverseIndex = graph[u].Count - 1 });
            }

            int totalFlowNeeded = 0;

            // S → каждый матч с capacity 3
            for (int k = 0; k < M; k++)
            {
                AddEdge(S, k + 1, 3);
                totalFlowNeeded += 3;

                // Матч → команда i и команда j с capacity 3 each
                var (i, j) = matches[k];
                AddEdge(k + 1, M + 1 + i, 3);
                AddEdge(k + 1, M + 1 + j, 3);
            }

            // Каждая команда → T с capacity = remaining[i]
            for (int i = 0; i < N; i++)
            {
                if (remaining[i] > 0)
                    AddEdge(M + 1 + i, T, remaining[i]);
            }

            // Находим максимальный поток
            int maxFlow = EdmondsKarp(graph, totalNodes, S, T);

            if (maxFlow != totalFlowNeeded)
            {
                // Отладка
                Console.Error.WriteLine($"maxFlow={maxFlow}, totalFlowNeeded={totalFlowNeeded}");
                for (int i = 0; i < N; i++)
                    Console.Error.WriteLine($"Team {i+1}: current={currentPoints[i]}, remaining={remaining[i]}, needed={needed[i]}");
                return;
            }

            // Восстанавливаем результаты матчей из потока
            for (int k = 0; k < M; k++)
            {
                var (i, j) = matches[k];
                // Поток из узла матча в команду i
                int flowToI = 0;
                foreach (var edge in graph[k + 1])
                {
                    if (edge.To == M + 1 + i)
                    {
                        flowToI = edge.Flow;
                        break;
                    }
                }

                int flowToJ = 3 - flowToI; // всего 3 очка

                // Определяем результат
                if (flowToI == 3 && flowToJ == 0) { table[i][j] = 'W'; table[j][i] = 'L'; }
                else if (flowToI == 0 && flowToJ == 3) { table[i][j] = 'L'; table[j][i] = 'W'; }
                else if (flowToI == 2 && flowToJ == 1) { table[i][j] = 'w'; table[j][i] = 'l'; }
                else if (flowToI == 1 && flowToJ == 2) { table[i][j] = 'l'; table[j][i] = 'w'; }
                else
                {
                    //fallback — не должно произойти
                    table[i][j] = 'W'; table[j][i] = 'L';
                }
            }

            var sb = new StringBuilder();
            for (int i = 0; i < N; i++)
                sb.AppendLine(new string(table[i]));
            Console.Write(sb.ToString());
        }

        private static int EdmondsKarp(List<Edge>[] graph, int n, int s, int t)
        {
            int maxFlow = 0;
            var parent = new int[n];
            var parentEdge = new int[n];

            while (true)
            {
                Array.Fill(parent, -1);
                parent[s] = s;
                var queue = new Queue<int>();
                queue.Enqueue(s);

                while (queue.Count > 0 && parent[t] == -1)
                {
                    int v = queue.Dequeue();
                    for (int i = 0; i < graph[v].Count; i++)
                    {
                        var edge = graph[v][i];
                        if (parent[edge.To] == -1 && edge.Capacity - edge.Flow > 0)
                        {
                            parent[edge.To] = v;
                            parentEdge[edge.To] = i;
                            queue.Enqueue(edge.To);
                        }
                    }
                }

                if (parent[t] == -1) break;

                int pushed = int.MaxValue;
                int cur = t;
                while (cur != s)
                {
                    int p = parent[cur];
                    int idx = parentEdge[cur];
                    pushed = Math.Min(pushed, graph[p][idx].Capacity - graph[p][idx].Flow);
                    cur = p;
                }

                cur = t;
                while (cur != s)
                {
                    int p = parent[cur];
                    int idx = parentEdge[cur];
                    graph[p][idx].Flow += pushed;
                    int revIdx = graph[p][idx].ReverseIndex;
                    graph[cur][revIdx].Flow -= pushed;
                    cur = p;
                }

                maxFlow += pushed;
            }

            return maxFlow;
        }

        internal sealed class FastScanner
        {
            private readonly Stream stream;
            private readonly byte[] buffer;
            private int pos;
            private int len;

            public FastScanner(Stream stream)
            {
                this.stream = stream;
                buffer = new byte[1 << 16];
                pos = 0;
                len = 0;
            }

            private byte ReadByte()
            {
                if (pos >= len)
                {
                    pos = 0;
                    len = stream.Read(buffer, 0, buffer.Length);
                    if (len == 0) return 0;
                }
                return buffer[pos++];
            }

            public int NextInt()
            {
                int c = ReadByte();
                while (c <= 32) c = ReadByte();
                int sign = 1;
                if (c == '-') { sign = -1; c = ReadByte(); }
                int res = 0;
                while (c > 32)
                {
                    res = res * 10 + (c - '0');
                    c = ReadByte();
                }
                return res * sign;
            }

            public string NextString()
            {
                var sb = new StringBuilder();
                int c = ReadByte();
                while (c <= 32) c = ReadByte();
                while (c > 32)
                {
                    sb.Append((char)c);
                    c = ReadByte();
                }
                return sb.ToString();
            }
        }
    }
}
