using System;
using System.Collections.Generic;
using System.IO;

namespace CT7.Tasks;

internal static class LCABinaryLifting
{
    private readonly record struct Edge(int From, int To, long Weight);

    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        int m = fs.NextInt();
        if (n == 0) return;

        var edges = new List<Edge>(m);
        var reach = new List<int>[n];
        for (int i = 0; i < n; i++) reach[i] = new List<int>();

        for (int i = 0; i < m; i++)
        {
            int from = fs.NextInt() - 1;
            int to = fs.NextInt() - 1;
            long weight = fs.NextLong();
            edges.Add(new Edge(from, to, weight));
            reach[from].Add(to);
        }

        if (!AllReachable(reach))
        {
            Console.WriteLine("NO");
            return;
        }

        long answer = DirectedMst(0, n, edges);
        Console.WriteLine("YES");
        Console.WriteLine(answer);
    }

    private static bool AllReachable(List<int>[] graph)
    {
        int n = graph.Length;
        var used = new bool[n];
        var stack = new Stack<int>();
        used[0] = true;
        stack.Push(0);
        while (stack.Count > 0)
        {
            int u = stack.Pop();
            foreach (int v in graph[u])
            {
                if (used[v]) continue;
                used[v] = true;
                stack.Push(v);
            }
        }

        for (int i = 0; i < n; i++)
            if (!used[i]) return false;
        return true;
    }

    private static long DirectedMst(int root, int n, List<Edge> edges)
    {
        long result = 0;
        while (true)
        {
            var minIn = new long[n];
            var parent = new int[n];
            Array.Fill(minIn, long.MaxValue);
            Array.Fill(parent, -1);

            foreach (var edge in edges)
            {
                if (edge.From != edge.To && edge.Weight < minIn[edge.To])
                {
                    minIn[edge.To] = edge.Weight;
                    parent[edge.To] = edge.From;
                }
            }

            minIn[root] = 0;
            for (int i = 0; i < n; i++) result += minIn[i];

            var id = new int[n];
            var mark = new int[n];
            Array.Fill(id, -1);
            Array.Fill(mark, -1);
            int cycles = 0;

            for (int i = 0; i < n; i++)
            {
                int v = i;
                while (mark[v] != i && id[v] == -1 && v != root)
                {
                    mark[v] = i;
                    v = parent[v];
                }

                if (v != root && id[v] == -1)
                {
                    for (int u = parent[v]; u != v; u = parent[u]) id[u] = cycles;
                    id[v] = cycles++;
                }
            }

            if (cycles == 0) break;

            for (int i = 0; i < n; i++)
                if (id[i] == -1) id[i] = cycles++;

            var nextEdges = new List<Edge>();
            foreach (var edge in edges)
            {
                int from = id[edge.From];
                int to = id[edge.To];
                if (from != to) nextEdges.Add(new Edge(from, to, edge.Weight - minIn[edge.To]));
            }

            root = id[root];
            n = cycles;
            edges = nextEdges;
        }

        return result;
    }

    private sealed class FastScanner
    {
        private readonly TextReader reader;
        private readonly char[] buffer = new char[1 << 16];
        private int len;
        private int ptr;

        public FastScanner(TextReader reader)
        {
            this.reader = reader;
        }

        private int Read()
        {
            if (ptr >= len)
            {
                len = reader.Read(buffer, 0, buffer.Length);
                ptr = 0;
                if (len == 0) return 0;
            }

            return buffer[ptr++];
        }

        public int NextInt()
        {
            int c = Read();
            while (c <= ' ')
            {
                if (c == 0) return 0;
                c = Read();
            }

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            int result = 0;
            while (c > ' ')
            {
                result = result * 10 + c - '0';
                c = Read();
            }

            return result * sign;
        }

        public long NextLong()
        {
            int c = Read();
            while (c <= ' ')
            {
                if (c == 0) return 0;
                c = Read();
            }

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            long result = 0;
            while (c > ' ')
            {
                result = result * 10 + c - '0';
                c = Read();
            }

            return sign == 1 ? result : -result;
        }
    }
}
