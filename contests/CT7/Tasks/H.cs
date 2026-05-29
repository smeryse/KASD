using System;
using System.Collections.Generic;
using System.IO;

namespace CT7.Tasks;

internal static class ShortestPathDijkstra
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        if (n == 0) return;

        var cost = new long[n, n];
        long high = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                cost[i, j] = fs.NextLong();
                if (cost[i, j] > high) high = cost[i, j];
            }
        }

        long low = 0;
        while (low < high)
        {
            long mid = (low + high) / 2;
            if (IsStrong(cost, mid)) high = mid;
            else low = mid + 1;
        }

        Console.WriteLine(low);
    }

    private static bool IsStrong(long[,] cost, long limit)
    {
        int n = cost.GetLength(0);
        return Reach(cost, limit, false) && Reach(cost, limit, true);
    }

    private static bool Reach(long[,] cost, long limit, bool reverse)
    {
        int n = cost.GetLength(0);
        var used = new bool[n];
        var stack = new Stack<int>();
        used[0] = true;
        stack.Push(0);

        while (stack.Count > 0)
        {
            int u = stack.Pop();
            for (int v = 0; v < n; v++)
            {
                long w = reverse ? cost[v, u] : cost[u, v];
                if (used[v] || w > limit) continue;
                used[v] = true;
                stack.Push(v);
            }
        }

        for (int i = 0; i < n; i++)
            if (!used[i]) return false;
        return true;
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
