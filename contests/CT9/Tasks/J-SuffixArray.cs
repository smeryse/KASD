using System;
using System.IO;
using System.Text;

namespace CT9.Tasks;

internal static class SuffixArrayTask
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        string s = fs.NextString();

        int[] suffixArray = BuildSuffixArray(s);
        int[] lcp = BuildLcp(s, suffixArray);

        var sb = new StringBuilder();
        for (int i = 0; i < suffixArray.Length; i++)
        {
            sb.Append(suffixArray[i] + 1);
            sb.Append(' ');
        }
        sb.AppendLine();

        for (int i = 0; i < lcp.Length; i++)
        {
            sb.Append(lcp[i]);
            sb.Append(' ');
        }
        sb.AppendLine();

        Console.Write(sb);
    }

    internal static int[] BuildSuffixArray(string s)
    {
        string text = s + '\0';
        int[] order = BuildCyclicShiftArray(text);
        var suffixArray = new int[s.Length];
        Array.Copy(order, 1, suffixArray, 0, s.Length);
        return suffixArray;
    }

    private static int[] BuildCyclicShiftArray(string s)
    {
        int n = s.Length;
        var p = new int[n];
        var c = new int[n];

        var count = new int[Math.Max(256, n)];
        for (int i = 0; i < n; i++) count[s[i]]++;
        for (int i = 1; i < 256; i++) count[i] += count[i - 1];
        for (int i = 0; i < n; i++) p[--count[s[i]]] = i;

        int classes = 1;
        for (int i = 1; i < n; i++)
        {
            if (s[p[i]] != s[p[i - 1]]) classes++;
            c[p[i]] = classes - 1;
        }

        var pn = new int[n];
        var cn = new int[n];
        for (int h = 0; (1 << h) < n; h++)
        {
            int len = 1 << h;
            for (int i = 0; i < n; i++) pn[i] = p[i] - len < 0 ? p[i] - len + n : p[i] - len;

            Array.Clear(count, 0, classes);
            for (int i = 0; i < n; i++) count[c[pn[i]]]++;
            for (int i = 1; i < classes; i++) count[i] += count[i - 1];
            for (int i = n - 1; i >= 0; i--) p[--count[c[pn[i]]]] = pn[i];

            cn[p[0]] = 0;
            int newClasses = 1;
            for (int i = 1; i < n; i++)
            {
                var cur = (c[p[i]], c[(p[i] + len) % n]);
                var prev = (c[p[i - 1]], c[(p[i - 1] + len) % n]);
                if (cur != prev) newClasses++;
                cn[p[i]] = newClasses - 1;
            }

            var temp = c;
            c = cn;
            cn = temp;
            classes = newClasses;
        }

        return p;
    }

    internal static int[] BuildLcp(string s, int[] suffixArray)
    {
        int n = s.Length;
        var rank = new int[n];
        for (int i = 0; i < n; i++) rank[suffixArray[i]] = i;

        var lcp = new int[Math.Max(0, n - 1)];
        int k = 0;
        for (int i = 0; i < n; i++)
        {
            int r = rank[i];
            if (r == n - 1)
            {
                k = 0;
                continue;
            }

            int j = suffixArray[r + 1];
            while (i + k < n && j + k < n && s[i + k] == s[j + k]) k++;
            lcp[r] = k;
            if (k > 0) k--;
        }

        return lcp;
    }

    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer = new byte[1 << 16];
        private int len;
        private int ptr;

        public FastScanner(Stream stream)
        {
            this.stream = stream;
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
            do c = Read(); while (c <= ' ' && c != 0);

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

