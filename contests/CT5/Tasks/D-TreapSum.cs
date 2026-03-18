using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class TreapSum
{
    private const int MOD = 1000000; 
    private static long lastSum = 0;

    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();

        var values = new System.Collections.Generic.SortedSet<long>();
        var sb = new StringBuilder();

        for (int q = 0; q < n; q++)
        {
            char type = fs.NextChar();
            switch (type)
            {
                case '+':
                    long x = fs.NextLong();
                    if (lastSum != -1) x = (x + lastSum) % MOD;
                    lastSum = -1;
                    values.Add(x);
                    break;

                case '?':
                    long l = fs.NextLong();
                    long r = fs.NextLong();
                    long sum = 0;
                    foreach (var v in values)
                    {
                        if (v < l) continue;
                        if (v > r) break;
                        sum += v;
                    }
                    lastSum = sum;
                    sb.AppendLine(sum.ToString());
                    break;
            }
        }

        Console.Write(sb.ToString());
    }

    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int len, ptr;

        public FastScanner(Stream stream, int bufferSize = 1 << 16)
        {
            this.stream = stream;
            buffer = new byte[bufferSize];
        }

        private byte Read()
        {
            if (ptr >= len)
            {
                len = stream.Read(buffer, 0, buffer.Length);
                ptr = 0;
                if (len <= 0) return 0;
            }
            return buffer[ptr++];
        }

        public int NextInt()
        {
            int c;
            do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }
            int val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val * sign;
        }

        public long NextLong()
        {
            long c;
            do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }
            long val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val * sign;
        }

        public char NextChar()
        {
            int c;
            do c = Read(); while (c <= ' ');
            return (char)c;
        }
    }
}

