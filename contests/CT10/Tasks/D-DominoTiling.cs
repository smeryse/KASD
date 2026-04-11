using System;
using System.Collections.Generic;
using System.IO;

namespace CT10.Tasks;

internal static class TaskD
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
        var fs = new FastScannerD(Console.OpenStandardInput());
        int n = fs.NextInt();
        int m = fs.NextInt();
        int a = fs.NextInt();
        int b = fs.NextInt();

        char[][] grid = new char[n][];
        for (int i = 0; i < n; i++)
            grid[i] = fs.NextString().ToCharArray();

        int freeCells = 0;
        var blackCells = new List<(int, int)>();
        var whiteCells = new List<(int, int)>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (grid[i][j] == '*')
                {
                    freeCells++;
                    if ((i + j) % 2 == 0)
                        blackCells.Add((i, j));
                    else
                        whiteCells.Add((i, j));
                }
            }
        }

        int nb = blackCells.Count;
        int nw = whiteCells.Count;
        adj = new List<int>[nb];
        for (int i = 0; i < nb; i++) adj[i] = new List<int>();

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        var whiteIndex = new Dictionary<(int, int), int>();
        for (int j = 0; j < nw; j++)
            whiteIndex[whiteCells[j]] = j;

        for (int i = 0; i < nb; i++)
        {
            var (r, c) = blackCells[i];
            for (int d = 0; d < 4; d++)
            {
                int nr = r + dx[d], nc = c + dy[d];
                if (nr >= 0 && nr < n && nc >= 0 && nc < m && grid[nr][nc] == '*')
                {
                    if (whiteIndex.TryGetValue((nr, nc), out int idx))
                        adj[i].Add(idx);
                }
            }
        }

        matchB = new int[nw];
        Array.Fill(matchB, -1);
        int matching = 0;
        for (int i = 0; i < nb; i++)
        {
            vis = new bool[nb];
            if (Dfs(i)) matching++;
        }

        if (2 * b <= a)
        {
            Console.WriteLine(freeCells * b);
        }
        else
        {
            int dominoes = matching;
            int singles = freeCells - 2 * dominoes;
            Console.WriteLine(dominoes * a + singles * b);
        }
    }
}

internal sealed class FastScannerD
{
    private readonly Stream stream;
    private readonly byte[] buffer = new byte[1 << 16];
    private int pos, len;
    public FastScannerD(Stream s) { stream = s; }
    private byte ReadByte()
    {
        if (pos >= len) { pos = 0; len = stream.Read(buffer); if (len == 0) return 0; }
        return buffer[pos++];
    }
    public int NextInt()
    {
        int c = ReadByte();
        bool neg = c == '-';
        if (neg) c = ReadByte();
        while (c <= ' ') { if (c == 0) return 0; c = ReadByte(); }
        int res = 0;
        do { res = res * 10 + c - '0'; c = ReadByte(); } while (c >= '0' && c <= '9');
        return neg ? -res : res;
    }
    public string NextString()
    {
        int c = ReadByte();
        while (c <= ' ') c = ReadByte();
        var s = "";
        while (c > ' ') { s += (char)c; c = ReadByte(); }
        return s;
    }
}


