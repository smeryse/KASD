using System;
using System.IO;
using System.Text;

namespace CT9.Tasks;

internal static class PrefixFunction
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        string s = fs.NextString();

        int n = s.Length;
        int[] pi = new int[n];

        for (int i = 1; i < n; i++)
        {
            int j = pi[i - 1];
            while (j > 0 && s[i] != s[j])
                j = pi[j - 1];
            if (s[i] == s[j])
                j++;
            pi[i] = j;
        }

        var sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            sb.Append(pi[i]);
            if (i < n - 1) sb.Append(' ');
        }
        Console.WriteLine(sb.ToString());
    }

    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int len;
        private int ptr;

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

        public string NextString()
        {
            int c;
            do c = Read(); while (c <= ' ');

            var sb = new StringBuilder();
            while (c > ' ')
            {
                sb.Append((char)c);
                c = Read();
            }
            return sb.ToString();
        }
    }
}
