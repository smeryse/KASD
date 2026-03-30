using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CT9.Tasks;

internal static class CommonSubstring
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int k = fs.NextInt();
        string[] strings = new string[k];
        for (int i = 0; i < k; i++)
            strings[i] = fs.NextString();

        int left = 0, right = strings[0].Length;
        string best = "";

        long MOD1 = 1_000_000_007L;
        long BASE1 = 31L;
        long MOD2 = 1_000_000_009L;
        long BASE2 = 37L;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            string candidate = FindCommonSubstring(strings, mid, MOD1, BASE1, MOD2, BASE2);
            if (candidate != null)
            {
                best = candidate;
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        Console.WriteLine(best ?? "");
    }

    private static string FindCommonSubstring(string[] strings, int len, long MOD1, long BASE1, long MOD2, long BASE2)
    {
        if (len == 0) return "";

        var commonHashes = GetSubstringHashes(strings[0], len, MOD1, BASE1, MOD2, BASE2);

        for (int i = 1; i < strings.Length && commonHashes.Count > 0; i++)
        {
            var hashes = GetSubstringHashes(strings[i], len, MOD1, BASE1, MOD2, BASE2);
            commonHashes.IntersectWith(hashes);
        }

        if (commonHashes.Count == 0) return null;

        var hash = commonHashes.First();
        return GetSubstringByHash(strings[0], len, hash, MOD1, BASE1, MOD2, BASE2);
    }

    private static System.Collections.Generic.HashSet<(long, long)> GetSubstringHashes(string s, int len, long MOD1, long BASE1, long MOD2, long BASE2)
    {
        var hashes = new System.Collections.Generic.HashSet<(long, long)>();
        if (len > s.Length) return hashes;

        long power1 = 1;
        long power2 = 1;
        for (int i = 0; i < len; i++)
        {
            power1 = (power1 * BASE1) % MOD1;
            power2 = (power2 * BASE2) % MOD2;
        }

        long hash1 = 0;
        long hash2 = 0;
        for (int i = 0; i < len; i++)
        {
            hash1 = (hash1 * BASE1 + s[i]) % MOD1;
            hash2 = (hash2 * BASE2 + s[i]) % MOD2;
        }

        hashes.Add((hash1, hash2));

        for (int i = len; i < s.Length; i++)
        {
            hash1 = (hash1 * BASE1 + s[i]) % MOD1;
            hash1 = (hash1 - s[i - len] * power1) % MOD1;
            if (hash1 < 0) hash1 += MOD1;

            hash2 = (hash2 * BASE2 + s[i]) % MOD2;
            hash2 = (hash2 - s[i - len] * power2) % MOD2;
            if (hash2 < 0) hash2 += MOD2;

            hashes.Add((hash1, hash2));
        }

        return hashes;
    }

    private static string GetSubstringByHash(string s, int len, (long, long) targetHash, long MOD1, long BASE1, long MOD2, long BASE2)
    {
        long power1 = 1;
        long power2 = 1;
        for (int i = 0; i < len; i++)
        {
            power1 = (power1 * BASE1) % MOD1;
            power2 = (power2 * BASE2) % MOD2;
        }

        long hash1 = 0;
        long hash2 = 0;
        for (int i = 0; i < len; i++)
        {
            hash1 = (hash1 * BASE1 + s[i]) % MOD1;
            hash2 = (hash2 * BASE2 + s[i]) % MOD2;
        }

        if (hash1 == targetHash.Item1 && hash2 == targetHash.Item2) return s.Substring(0, len);

        for (int i = len; i < s.Length; i++)
        {
            hash1 = (hash1 * BASE1 + s[i]) % MOD1;
            hash1 = (hash1 - s[i - len] * power1) % MOD1;
            if (hash1 < 0) hash1 += MOD1;

            hash2 = (hash2 * BASE2 + s[i]) % MOD2;
            hash2 = (hash2 - s[i - len] * power2) % MOD2;
            if (hash2 < 0) hash2 += MOD2;

            if (hash1 == targetHash.Item1 && hash2 == targetHash.Item2)
                return s.Substring(i - len + 1, len);
        }

        return null;
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

            int val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val;
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
