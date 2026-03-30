using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CT9.Tasks;

internal static class SubstringSearch
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        string p = fs.NextString();
        string t = fs.NextString();

        int n = p.Length;
        int m = t.Length;

        string combined = p + "#" + t;
        int[] z = ComputeZFunction(combined);

        var positions = new List<int>();

        for (int i = n + 1; i < combined.Length; i++)
        {
            if (z[i] == n)
            {
                positions.Add(i - n);
            }
        }

        Console.WriteLine(positions.Count);
        Console.WriteLine(string.Join(" ", positions));
    }

    private static int[] ComputeZFunction(string s)
    {
        int n = s.Length;
        int[] z = new int[n];

        int l = 0, r = 0;
        for (int i = 1; i < n; i++)
        {
            if (i < r)
                z[i] = Math.Min(r - i, z[i - l]);

            while (i + z[i] < n && s[z[i]] == s[i + z[i]])
                z[i]++;

            if (i + z[i] > r)
            {
                l = i;
                r = i + z[i];
            }
        }

        return z;
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
