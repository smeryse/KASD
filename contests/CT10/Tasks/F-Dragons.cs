using System;
using System.Collections.Generic;
using System.IO;

namespace CT10.Tasks;

internal static class TaskF
{
    static int[] matchB;
    static bool[] vis;
    static List<int>[] adj;

    static bool Dfs(int u)
    {
        if (vis[u]) return false;
        vis[u] = true;
        foreach (int v in adj[u])
        {
            if (matchB[v] == -1 || Dfs(matchB[v]))
            {
                matchB[v] = u;
                return true;
            }
        }
        return false;
    }

    public static void Solve()
    {
        var fs = new FastScannerF(Console.OpenStandardInput());
        int m = fs.NextInt();
        int k = fs.NextInt();
        int n = fs.NextInt();

        int t = fs.NextInt();
        var forbidden = new HashSet<(int, int)>();
        for (int i = 0; i < t; i++)
        {
            int g = fs.NextInt();
            int y = fs.NextInt();
            forbidden.Add((g, y - m));
        }

        int q = fs.NextInt();
        var canBeAlone = new bool[m + k + 1];
        for (int i = 1; i <= m + k; i++) canBeAlone[i] = true;
        for (int i = 0; i < q; i++)
        {
            int dragon = fs.NextInt();
            canBeAlone[dragon] = false;
        }

        int mustGreen = 0, mustYellow = 0;
        for (int i = 1; i <= m; i++)
            if (!canBeAlone[i]) mustGreen++;
        for (int i = m + 1; i <= m + k; i++)
            if (!canBeAlone[i]) mustYellow++;

        adj = new List<int>[m];
        for (int i = 1; i <= m; i++)
        {
            adj[i - 1] = new List<int>();
            for (int j = 1; j <= k; j++)
            {
                if (!forbidden.Contains((i, j)))
                    adj[i - 1].Add(j - 1);
            }
        }

        matchB = new int[k];
        Array.Fill(matchB, -1);
        var matchA = new int[m];
        Array.Fill(matchA, -1);

        for (int i = 1; i <= m; i++)
        {
            if (!canBeAlone[i])
            {
                vis = new bool[m];
                DfsF(i - 1, matchA, matchB, adj);
            }
        }

        for (int i = 1; i <= m; i++)
        {
            vis = new bool[m];
            DfsF(i - 1, matchA, matchB, adj);
        }

        int matching = 0;
        for (int j = 0; j < k; j++)
            if (matchB[j] != -1) matching++;

        if (matching < n)
        {
            Console.WriteLine("NO");
            return;
        }

        var matchedGreen = new bool[m + 1];
        var matchedYellow = new bool[m + k + 1];
        for (int j = 0; j < k; j++)
        {
            if (matchB[j] != -1)
            {
                matchedGreen[matchB[j] + 1] = true;
                matchedYellow[m + j + 1] = true;
            }
        }

        for (int i = 1; i <= m; i++)
            if (!canBeAlone[i] && !matchedGreen[i])
            { Console.WriteLine("NO"); return; }
        for (int i = m + 1; i <= m + k; i++)
            if (!canBeAlone[i] && !matchedYellow[i])
            { Console.WriteLine("NO"); return; }

        Console.WriteLine("YES");
        int printed = 0;
        for (int j = 0; j < k && printed < n; j++)
        {
            if (matchB[j] != -1)
            {
                Console.WriteLine($"{matchB[j] + 1} {m + j + 1}");
                printed++;
            }
        }
    }

    static bool DfsF(int u, int[] matchA, int[] matchB, List<int>[] adj)
    {
        int m = matchA.Length;
        if (vis[u]) return false;
        vis[u] = true;
        foreach (int v in adj[u])
        {
            if (matchB[v] == -1 || DfsF(matchB[v], matchA, matchB, adj))
            {
                matchA[u] = v;
                matchB[v] = u;
                return true;
            }
        }
        return false;
    }
}

internal sealed class FastScannerF
{
    private readonly Stream stream;
    private readonly byte[] buffer = new byte[1 << 16];
    private int pos, len;
    public FastScannerF(Stream s) { stream = s; }
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
}
