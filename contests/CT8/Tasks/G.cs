using System;
using System.Collections.Generic;
using System.IO;


internal static class G
{
    public static void Main()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        
        int n = fs.NextInt();
        int m = fs.NextInt();
        if (n == 0) return;
        
        var adj = new List<(int to, int weight)>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<(int, int)>();
        
        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            int w = fs.NextInt();
            adj[u].Add((v, w));
            adj[v].Add((u, w));
        }
        
        long t = fs.NextLong();

        var potential = new long[n + 1];
        var visited = new bool[n + 1];
        var stack = new Stack<int>();
        visited[1] = true;
        stack.Push(1);

        while (stack.Count > 0)
        {
            int u = stack.Pop();
            foreach (var (v, w) in adj[u])
            {
                if (!visited[v])
                {
                    visited[v] = true;
                    potential[v] = potential[u] + w;
                    stack.Push(v);
                }
            }
        }

        if (!visited[n])
        {
            Console.WriteLine("Impossible");
            return;
        }

        long period = 0;
        for (int u = 1; u <= n; u++)
        {
            if (!visited[u]) continue;
            foreach (var (v, w) in adj[u])
            {
                if (visited[v])
                    period = Gcd(period, Math.Abs(potential[u] + w - potential[v]));
            }
        }

        if (period == 0)
        {
            Console.WriteLine(t == 0 && n == 1 ? "Possible" : "Impossible");
            return;
        }

        int mod = (int)period;
        long[,] dist = new long[n + 1, mod];
        for (int i = 1; i <= n; i++)
            for (int r = 0; r < mod; r++)
                dist[i, r] = long.MaxValue;

        var pq = new PriorityQueue<(int node, int residue), long>();
        dist[1, 0] = 0;
        pq.Enqueue((1, 0), 0);

        while (pq.Count > 0)
        {
            var state = pq.Dequeue();
            int u = state.node;
            int residue = state.residue;
            long cur = dist[u, residue];

            foreach (var (v, w) in adj[u])
            {
                int nextResidue = (int)((residue + (long)w) % mod);
                long nextDist = cur + w;
                if (nextDist < dist[v, nextResidue])
                {
                    dist[v, nextResidue] = nextDist;
                    pq.Enqueue((v, nextResidue), nextDist);
                }
            }
        }

        int targetResidue = (int)(t % mod);
        Console.WriteLine(dist[n, targetResidue] <= t ? "Possible" : "Impossible");
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long t = a % b;
            a = b;
            b = t;
        }
        return Math.Abs(a);
    }

    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer = new byte[1 << 16];
        private int bufferPtr;
        private int bytesRead;

        public FastScanner(Stream stream)
        {
            this.stream = stream;
        }

        private void ReadBuffer()
        {
            bufferPtr = 0;
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0)
                buffer[0] = (byte)'\0';
        }

        private byte ReadByte()
        {
            if (bufferPtr >= bytesRead)
                ReadBuffer();
            return buffer[bufferPtr++];
        }

        public int NextInt()
        {
            int result = 0;
            byte c = ReadByte();

            while (c <= ' ')
            {
                if (c == '\0') return 0;
                c = ReadByte();
            }

            bool negative = false;
            if (c == '-')
            {
                negative = true;
                c = ReadByte();
            }

            while (c > ' ')
            {
                result = result * 10 + (c - '0');
                c = ReadByte();
            }

            return negative ? -result : result;
        }

        public long NextLong()
        {
            long result = 0;
            byte c = ReadByte();

            while (c <= ' ')
            {
                if (c == '\0') return 0;
                c = ReadByte();
            }

            bool negative = false;
            if (c == '-')
            {
                negative = true;
                c = ReadByte();
            }

            while (c > ' ')
            {
                result = result * 10 + (c - '0');
                c = ReadByte();
            }

            return negative ? -result : result;
        }
    }
}
