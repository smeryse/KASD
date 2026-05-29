using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT11.Tasks
{
    internal static class TaskE
    {
        private const int INF = 1_000_000_000;

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
            int m = fs.NextInt();
            int n = fs.NextInt();

            char[][] grid = new char[m][];
            for (int i = 0; i < m; i++)
                grid[i] = fs.NextString().ToCharArray();

            int aRow = -1, aCol = -1, bRow = -1, bCol = -1;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (grid[i][j] == 'A') { aRow = i; aCol = j; }
                    else if (grid[i][j] == 'B') { bRow = i; bCol = j; }
                }
            }
            if (AreAdjacent(aRow, aCol, bRow, bCol, m, n, grid))
            {
                if (CanReach(aRow, aCol, bRow, bCol, m, n, grid, new bool[m, n]))
                {
                    Console.WriteLine("-1");
                    return;
                }
                else
                {
                    Console.WriteLine("0");
                    for (int i = 0; i < m; i++)
                        Console.WriteLine(new string(grid[i]));
                    return;
                }
            }

            int totalCells = m * n;
            int totalNodes = totalCells * 2 + 2;
            int source = totalCells * 2;
            int sink = totalCells * 2 + 1;

            var graph = new List<Edge>[totalNodes];
            for (int i = 0; i < totalNodes; i++) graph[i] = new List<Edge>();

            void AddEdge(int u, int v, int cap)
            {
                graph[u].Add(new Edge { To = v, Capacity = cap, Flow = 0, ReverseIndex = graph[v].Count });
                graph[v].Add(new Edge { To = u, Capacity = 0, Flow = 0, ReverseIndex = graph[u].Count - 1 });
            }

            int CellIn(int r, int c) => (r * n + c) * 2;
            int CellOut(int r, int c) => (r * n + c) * 2 + 1;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int cap = grid[i][j] switch
                    {
                        '#' => 0,
                        '.' => 1,
                        '-' => INF,
                        'A' or 'B' => INF,
                        _ => INF
                    };
                    AddEdge(CellIn(i, j), CellOut(i, j), cap);
                }
            }

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (grid[i][j] == '#') continue;
                    for (int d = 0; d < 4; d++)
                    {
                        int ni = i + dr[d], nj = j + dc[d];
                        if (ni >= 0 && ni < m && nj >= 0 && nj < n && grid[ni][nj] != '#')
                        {
                            AddEdge(CellOut(i, j), CellIn(ni, nj), INF);
                        }
                    }
                }
            }

            AddEdge(source, CellOut(aRow, aCol), INF);
            AddEdge(CellIn(bRow, bCol), sink, INF);

            int maxFlow = EdmondsKarp(graph, totalNodes, source, sink);

            if (maxFlow >= INF)
            {
                Console.WriteLine("-1");
                return;
            }

            var reachable = new bool[totalNodes];
            var q = new Queue<int>();
            q.Enqueue(source);
            reachable[source] = true;

            while (q.Count > 0)
            {
                int v = q.Dequeue();
                foreach (var edge in graph[v])
                {
                    if (!reachable[edge.To] && edge.Capacity - edge.Flow > 0)
                    {
                        reachable[edge.To] = true;
                        q.Enqueue(edge.To);
                    }
                }
            }

            int wallsBuilt = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (grid[i][j] == '.' && reachable[CellIn(i, j)] && !reachable[CellOut(i, j)])
                    {
                        grid[i][j] = '+';
                        wallsBuilt++;
                    }
                }
            }

            Console.WriteLine(wallsBuilt);
            for (int i = 0; i < m; i++)
                Console.WriteLine(new string(grid[i]));
        }

        private static bool AreAdjacent(int r1, int c1, int r2, int c2, int m, int n, char[][] grid)
        {
            int dr = Math.Abs(r1 - r2), dc = Math.Abs(c1 - c2);
            return (dr + dc == 1);
        }

        private static bool CanReach(int sr, int sc, int tr, int tc, int m, int n, char[][] grid, bool[,] visited)
        {
            var q = new Queue<(int, int)>();
            q.Enqueue((sr, sc));
            visited[sr, sc] = true;

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            while (q.Count > 0)
            {
                var (r, c) = q.Dequeue();
                if (r == tr && c == tc) return true;

                for (int d = 0; d < 4; d++)
                {
                    int nr = r + dr[d], nc = c + dc[d];
                    if (nr >= 0 && nr < m && nc >= 0 && nc < n && !visited[nr, nc] && grid[nr][nc] != '#')
                    {
                        visited[nr, nc] = true;
                        q.Enqueue((nr, nc));
                    }
                }
            }
            return false;
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

                int pushed = INF;
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
