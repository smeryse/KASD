using System;
using System.Collections.Generic;
using System.IO;

namespace CT7.Tasks;

internal static class TreeDP
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        fs.NextInt();
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

        var parent = new int[n + 1];
        var order = new List<int>(n);
        var stack = new Stack<int>();
        parent[1] = 1;
        stack.Push(1);

        while (stack.Count > 0)
        {
            int u = stack.Pop();
            order.Add(u);
            foreach (int v in adj[u])
            {
                if (v == parent[u]) continue;
                parent[v] = u;
                stack.Push(v);
            }
        }

        var dp0 = new int[n + 1];
        var dp1 = new int[n + 1];
        for (int i = order.Count - 1; i >= 0; i--)
        {
            int u = order[i];
            dp1[u] = 1;
            foreach (int v in adj[u])
            {
                if (v == parent[u]) continue;
                dp1[u] += dp0[v];
                dp0[u] += Math.Max(dp0[v], dp1[v]);
            }
        }

        Console.WriteLine(Math.Max(dp0[1], dp1[1]));
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
