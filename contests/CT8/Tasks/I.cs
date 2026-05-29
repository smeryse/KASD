using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT8.Tasks;





internal static class I
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int testCount = fs.NextInt();
        
        for (int test = 0; test < testCount; test++)
        {
            if (test > 0)
                Console.WriteLine();
            
            SolveTest(fs);
        }
    }
    
    private static void SolveTest(FastScanner fs)
    {
        int n = fs.NextInt();
        int m = fs.NextInt();
        
        var adj = new List<int>[n + 1];
        var revAdj = new List<int>[n + 1];
        int[] outDegree = new int[n + 1];
        
        for (int i = 1; i <= n; i++)
        {
            adj[i] = new List<int>();
            revAdj[i] = new List<int>();
        }
        
        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            adj[u].Add(v);
            revAdj[v].Add(u);
            outDegree[u]++;
        }
        
        
        int[] result = new int[n + 1];
        
        
        
        
        
        
        var queue = new Queue<int>();
        
        
        for (int i = 1; i <= n; i++)
        {
            if (outDegree[i] == 0)
            {
                result[i] = 2; 
                queue.Enqueue(i);
            }
        }
        
        while (queue.Count > 0)
        {
            int v = queue.Dequeue();
            
            foreach (int u in revAdj[v])
            {
                if (result[u] != 0)
                    continue;
                
                if (result[v] == 2)
                {
                    
                    result[u] = 1; 
                    queue.Enqueue(u);
                }
                else
                {
                    
                    outDegree[u]--;
                    if (outDegree[u] == 0)
                    {
                        
                        result[u] = 2; 
                        queue.Enqueue(u);
                    }
                }
            }
        }
        
        
        for (int i = 1; i <= n; i++)
        {
            if (result[i] == 0)
                result[i] = 0; 
        }
        
        for (int i = 1; i <= n; i++)
        {
            if (result[i] == 0)
                Console.WriteLine("DRAW");
            else if (result[i] == 1)
                Console.WriteLine("FIRST");
            else
                Console.WriteLine("SECOND");
        }
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
