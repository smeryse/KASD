using System;
using System.Collections.Generic;
using System.IO;

namespace CT10.Tasks;

internal static class TaskA
{
    static int n, m;
    static List<int>[] adj;
    static int[] matchB;
    static bool[] visited;

    static bool TryKuhn(int u)
    {
        if (visited[u]) return false;
        visited[u] = true;
        foreach (int v in adj[u])
        {
            if (matchB[v] == -1 || TryKuhn(matchB[v]))
            {
                matchB[v] = u;
                return true;
            }
        }
        return false;
    }

    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        n = fs.NextInt();
        m = fs.NextInt();

        adj = new List<int>[n];

        for (int i = 0; i < n; i++)
        {
            adj[i] = new List<int>();
            while (true)
            {
                int v = fs.NextInt();
                if (v == 0) break;
                adj[i].Add(v - 1);
                
            }
        }

        matchB = new int[m];
        Array.Fill(matchB, -1);

        for (int i = 0; i < n; i++)
        {
            visited = new bool[n];
            TryKuhn(i);
        }

        var result = new List<(int a, int b)>();
        for (int j = 0; j < m; j++)
        {
            if (matchB[j] != -1)
                result.Add((matchB[j] + 1, j + 1));
        }

        Console.WriteLine(result.Count);
        foreach (var (a, b) in result)
            Console.WriteLine($"{a} {b}");
    }
}

internal sealed class FastScanner
{
    private readonly Stream stream;
    private readonly byte[] buffer = new byte[1 << 16];
    private int pos, len;

    public FastScanner(Stream s) { stream = s; }

    private byte ReadByte()
    {
        if (pos >= len) { pos = 0; len = stream.Read(buffer); if (len == 0) return 0; }
        return buffer[pos++];
    }

    public int NextInt()
    {
        int c = ReadByte();
        while (c <= ' ') { if (c == 0) return -1; c = ReadByte(); }
        int res = 0;
        do { res = res * 10 + c - '0'; c = ReadByte(); } while (c >= '0' && c <= '9');
        return res;
    }
}

