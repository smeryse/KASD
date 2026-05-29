using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT8.Tasks;





internal static class H
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        
        int n = fs.NextInt();
        int m = fs.NextInt();
        int s = fs.NextInt();
        if (n == 0) return;
        
        var adj = new List<int>[n + 1];
        int[] outDegree = new int[n + 1];
        
        for (int i = 1; i <= n; i++)
            adj[i] = new List<int>();
        
        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            adj[u].Add(v);
            outDegree[u]++;
        }
        
        var queue = new Queue<int>();
        int[] inDegree = new int[n + 1];
        for (int u = 1; u <= n; u++)
        {
            foreach (var v in adj[u])
                inDegree[v]++;
        }

        for (int i = 1; i <= n; i++)
            if (inDegree[i] == 0)
                queue.Enqueue(i);

        var order = new List<int>(n);
        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            order.Add(u);
            foreach (var v in adj[u])
            {
                inDegree[v]--;
                if (inDegree[v] == 0)
                    queue.Enqueue(v);
            }
        }

        var win = new bool[n + 1];
        for (int i = order.Count - 1; i >= 0; i--)
        {
            int u = order[i];
            foreach (var v in adj[u])
            {
                if (!win[v])
                {
                    win[u] = true;
                    break;
                }
            }
        }

        if (win[s])
            Console.WriteLine("First player wins");
        else
            Console.WriteLine("Second player wins");
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
    }
}
