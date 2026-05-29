using System;
using System.IO;

namespace CT7.Tasks;

internal static class MSTKruskal
{
    private readonly record struct Edge(int U, int V, long W);

    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        int m = fs.NextInt();
        if (n == 0) return;

        var edges = new Edge[m];
        for (int i = 0; i < m; i++)
            edges[i] = new Edge(fs.NextInt(), fs.NextInt(), fs.NextLong());

        Array.Sort(edges, (a, b) => a.W.CompareTo(b.W));
        var dsu = new Dsu(n);
        long answer = 0;
        int used = 0;

        foreach (var edge in edges)
        {
            if (!dsu.Union(edge.U, edge.V)) continue;
            answer += edge.W;
            used++;
            if (used == n - 1) break;
        }

        Console.WriteLine(answer);
    }

    private sealed class Dsu
    {
        private readonly int[] parent;
        private readonly int[] rank;

        public Dsu(int n)
        {
            parent = new int[n + 1];
            rank = new int[n + 1];
            for (int i = 1; i <= n; i++) parent[i] = i;
        }

        private int Find(int v)
        {
            if (parent[v] != v) parent[v] = Find(parent[v]);
            return parent[v];
        }

        public bool Union(int a, int b)
        {
            a = Find(a);
            b = Find(b);
            if (a == b) return false;
            if (rank[a] < rank[b]) (a, b) = (b, a);
            parent[b] = a;
            if (rank[a] == rank[b]) rank[a]++;
            return true;
        }
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
