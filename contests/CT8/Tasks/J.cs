using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT8.Tasks;





internal static class J
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        
        int n = fs.NextInt();
        int m = fs.NextInt();
        
        var adj = new List<int>[n + 1];
        int[] inDegree = new int[n + 1];
        
        for (int i = 1; i <= n; i++)
            adj[i] = new List<int>();
        
        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            adj[u].Add(v);
            inDegree[v]++;
        }
        
        int[] grundy = new int[n + 1];
        
        
        var queue = new Queue<int>();
        for (int i = 1; i <= n; i++)
        {
            if (inDegree[i] == 0)
                queue.Enqueue(i);
        }
        
        var topoOrder = new List<int>();
        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            topoOrder.Add(u);
            
            foreach (int v in adj[u])
            {
                inDegree[v]--;
                if (inDegree[v] == 0)
                    queue.Enqueue(v);
            }
        }
        
        
        for (int i = topoOrder.Count - 1; i >= 0; i--)
        {
            int u = topoOrder[i];
            
            
            var seen = new HashSet<int>();
            foreach (int v in adj[u])
                seen.Add(grundy[v]);
            
            int mex = 0;
            while (seen.Contains(mex))
                mex++;
            
            grundy[u] = mex;
        }
        
        for (int i = 1; i <= n; i++)
            Console.WriteLine(grundy[i]);
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
