using System;
using System.IO;
using System.Text;

namespace CT9.Tasks;

internal static class SubstringCompare
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        string s = fs.NextString();
        int m = fs.NextInt();

        int n = s.Length;
        long MOD = 1_000_000_007L;
        long BASE = 31L;

        // Префиксные хэши: h[i] = хэш s[0..i-1]
        long[] h = new long[n + 1];
        // powers[i] = BASE^i mod MOD
        long[] powers = new long[n + 1];
        powers[0] = 1;
        h[0] = 0;

        for (int i = 0; i < n; i++)
        {
            h[i + 1] = (h[i] * BASE + s[i]) % MOD;
            powers[i + 1] = (powers[i] * BASE) % MOD;
        }

        var sb = new StringBuilder();

        for (int i = 0; i < m; i++)
        {
            int a = fs.NextInt();
            int b = fs.NextInt();
            int c = fs.NextInt();
            int d = fs.NextInt();

            // Переводим в 0-индексацию: s[a..b] -> [a-1, b-1]
            // Длина подстроки: b - a + 1
            // Хэш подстроки s[l..r] (0-индексы) = (h[r+1] - h[l] * powers[r-l+1]) mod MOD
            int len1 = b - a + 1;
            int len2 = d - c + 1;

            if (len1 != len2)
            {
                sb.Append("No\n");
                continue;
            }

            long hash1 = GetHash(h, powers, a - 1, b - 1, MOD);
            long hash2 = GetHash(h, powers, c - 1, d - 1, MOD);

            if (hash1 == hash2)
                sb.Append("Yes\n");
            else
                sb.Append("No\n");
        }

        Console.Write(sb.ToString());
    }

    private static long GetHash(long[] h, long[] powers, int l, int r, long MOD)
    {
        long result = (h[r + 1] - h[l] * powers[r - l + 1]) % MOD;
        if (result < 0) result += MOD;
        return result;
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
