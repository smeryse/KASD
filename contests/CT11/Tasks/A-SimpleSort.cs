using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT11.Tasks
{
    internal static class TaskA
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
            int M = fs.NextInt();

            var graph = new List<Edge>[N + 1];
            for (int i = 1; i <= N; i++)
                graph[i] = new List<Edge>();

            var edgeIndex = new (int u, int edgeIdx)[M];

            for (int i = 0; i < M; i++)
            {
                int u = fs.NextInt();
                int v = fs.NextInt();
                int c = fs.NextInt();

                graph[u].Add(new Edge { To = v, Capacity = c, Flow = 0, ReverseIndex = graph[v].Count });
                graph[v].Add(new Edge { To = u, Capacity = c, Flow = 0, ReverseIndex = graph[u].Count - 1 });

                edgeIndex[i] = (u, graph[u].Count - 1);
            }

            int source = 1;
            int sink = N;

            int maxFlow = 0;
            while (true)
            {
                var parent = new int[N + 1];
                var parentEdge = new int[N + 1];
                Array.Fill(parent, -1);
                parent[source] = source;

                var queue = new Queue<int>();
                queue.Enqueue(source);

                while (queue.Count > 0 && parent[sink] == -1)
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

                if (parent[sink] == -1) break;

                int pushed = int.MaxValue;
                int cur = sink;
                while (cur != source)
                {
                    int p = parent[cur];
                    int idx = parentEdge[cur];
                    pushed = Math.Min(pushed, graph[p][idx].Capacity - graph[p][idx].Flow);
                    cur = p;
                }

                cur = sink;
                while (cur != source)
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

            var sb = new StringBuilder();
            sb.AppendLine(maxFlow.ToString());
            for (int i = 0; i < M; i++)
            {
                var (u, idx) = edgeIndex[i];
                int flow = graph[u][idx].Flow;
                sb.AppendLine(flow.ToString());
            }
            Console.Write(sb.ToString());
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
        }
    }
}
