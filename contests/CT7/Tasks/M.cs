using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT7.Tasks;

internal static class CentroidDecomposition
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        int q = fs.NextInt();
        if (n == 0) return;

        var adj = new List<int>[n + 1];
        for (int i = 1; i <= n; i++) adj[i] = new List<int>();
        for (int i = 0; i < n - 1; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            adj[u].Add(v);
            adj[v].Add(u);
        }

        int log = 1;
        while ((1 << log) <= n) log++;
        var up = new int[n + 1, log];
        var depth = new int[n + 1];
        BuildParents(adj, up, depth, log);

        var sb = new StringBuilder();
        for (int i = 0; i < q; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            int lca = Lca(u, v, up, depth, log);
            sb.AppendLine((depth[u] + depth[v] - 2 * depth[lca]).ToString());
        }
        Console.Write(sb);
    }

    private static void BuildParents(List<int>[] adj, int[,] up, int[] depth, int log)
    {
        var stack = new Stack<(int node, int parent)>();
        stack.Push((1, 1));
        while (stack.Count > 0)
        {
            var (u, parent) = stack.Pop();
            up[u, 0] = parent;
            for (int j = 1; j < log; j++) up[u, j] = up[up[u, j - 1], j - 1];

            foreach (int v in adj[u])
            {
                if (v == parent) continue;
                depth[v] = depth[u] + 1;
                stack.Push((v, u));
            }
        }
    }

    private static int Lca(int a, int b, int[,] up, int[] depth, int log)
    {
        if (depth[a] < depth[b]) (a, b) = (b, a);
        int diff = depth[a] - depth[b];
        for (int j = 0; j < log; j++)
            if (((diff >> j) & 1) == 1)
                a = up[a, j];

        if (a == b) return a;
        for (int j = log - 1; j >= 0; j--)
        {
            if (up[a, j] == up[b, j]) continue;
            a = up[a, j];
            b = up[b, j];
        }
        return up[a, 0];
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
    }
}
