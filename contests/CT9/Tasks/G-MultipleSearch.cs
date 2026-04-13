using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT9.Tasks;

internal static class TaskG
{
    public static void Main()
    {
        Solve();
    }

    public static void Solve()
    {
        var fs = new FastScannerG(Console.OpenStandardInput());
        int n = fs.NextInt();

        var patterns = new string[n];
        int totalLen = 0;
        for (int i = 0; i < n; i++)
        {
            patterns[i] = fs.NextString();
            totalLen += patterns[i].Length;
        }
        string t = fs.NextString();

        var seen = new Dictionary<string, int>();
        var canonicalIdx = new int[n];

        int nodeCount = totalLen + 2;
        var go = new int[nodeCount][];
        var fail = new int[nodeCount];
        var output = new int[nodeCount];
        var dict = new int[nodeCount];

        for (int i = 0; i < nodeCount; i++)
        {
            go[i] = new int[26];
            for (int c = 0; c < 26; c++) go[i][c] = -1;
            output[i] = -1;
            dict[i] = -1;
        }

        int size = 1;

        for (int i = 0; i < n; i++)
        {
            if (!seen.TryGetValue(patterns[i], out int canonical))
            {
                canonical = i;
                seen[patterns[i]] = i;

                int cur = 0;
                foreach (char ch in patterns[i])
                {
                    int c = ch - 'a';
                    if (go[cur][c] == -1)
                        go[cur][c] = size++;
                    cur = go[cur][c];
                }
                if (output[cur] == -1)
                    output[cur] = i;
            }
            canonicalIdx[i] = canonical;
        }

        var queue = new Queue<int>();
        for (int c = 0; c < 26; c++)
        {
            if (go[0][c] == -1)
                go[0][c] = 0;
            else
            {
                fail[go[0][c]] = 0;
                queue.Enqueue(go[0][c]);
            }
        }

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            dict[u] = (output[fail[u]] != -1) ? fail[u] : dict[fail[u]];

            for (int c = 0; c < 26; c++)
            {
                if (go[u][c] == -1)
                {
                    go[u][c] = go[fail[u]][c];
                }
                else
                {
                    fail[go[u][c]] = go[fail[u]][c];
                    queue.Enqueue(go[u][c]);
                }
            }
        }

        var found = new bool[n];
        int cur2 = 0;
        foreach (char ch in t)
        {
            cur2 = go[cur2][ch - 'a'];
            int tmp = (output[cur2] != -1) ? cur2 : dict[cur2];
            while (tmp != -1)
            {
                found[output[tmp]] = true;
                tmp = dict[tmp];
            }
        }

        var sb = new StringBuilder(n * 4);
        for (int i = 0; i < n; i++)
            sb.AppendLine(found[canonicalIdx[i]] ? "YES" : "NO");

        Console.Write(sb);
    }
}

internal sealed class FastScannerG
{
    private readonly Stream stream;
    private readonly byte[] buffer = new byte[1 << 16];
    private int pos, len;
    public FastScannerG(Stream s) { stream = s; }
    private byte ReadByte()
    {
        if (pos >= len) { pos = 0; len = stream.Read(buffer); if (len == 0) return 0; }
        return buffer[pos++];
    }
    public int NextInt()
    {
        int c = ReadByte();
        while (c <= ' ') { if (c == 0) return 0; c = ReadByte(); }
        int res = 0;
        do { res = res * 10 + c - '0'; c = ReadByte(); } while (c >= '0' && c <= '9');
        return res;
    }
    public string NextString()
    {
        int c = ReadByte();
        while (c <= ' ') { if (c == 0) return ""; c = ReadByte(); }
        var sb = new StringBuilder();
        do { sb.Append((char)c); c = ReadByte(); } while (c > ' ');
        return sb.ToString();
    }
}