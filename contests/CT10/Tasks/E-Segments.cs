using System;
using System.Collections.Generic;
using System.IO;

namespace CT10.Tasks;

internal static class TaskE
{
    struct Segment
    {
        public long x1, y1, x2, y2;
        public bool IsHorizontal => y1 == y2;
    }

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

    static bool Intersects(Segment h, Segment v)
    {
        long hx1 = Math.Min(h.x1, h.x2), hx2 = Math.Max(h.x1, h.x2);
        long vy1 = Math.Min(v.y1, v.y2), vy2 = Math.Max(v.y1, v.y2);

        return v.x1 >= hx1 && v.x1 <= hx2 && h.y1 >= vy1 && h.y1 <= vy2;
    }

    public static void Solve()
    {
        var fs = new FastScannerE(Console.OpenStandardInput());
        int n = fs.NextInt();

        var hor = new List<Segment>();
        var ver = new List<Segment>();

        for (int i = 0; i < n; i++)
        {
            long x1 = fs.NextLong(), y1 = fs.NextLong();
            long x2 = fs.NextLong(), y2 = fs.NextLong();
            if (x1 > x2) { var t = x1; x1 = x2; x2 = t; }
            if (y1 > y2) { var t = y1; y1 = y2; y2 = t; }
            var seg = new Segment { x1 = x1, y1 = y1, x2 = x2, y2 = y2 };
            if (seg.IsHorizontal) hor.Add(seg);
            else ver.Add(seg);
        }

        int nh = hor.Count, nv = ver.Count;
        adj = new List<int>[nh];
        for (int i = 0; i < nh; i++)
        {
            adj[i] = new List<int>();
            for (int j = 0; j < nv; j++)
            {
                if (Intersects(hor[i], ver[j]))
                    adj[i].Add(j);
            }
        }

        matchB = new int[nv];
        Array.Fill(matchB, -1);
        int matching = 0;
        for (int i = 0; i < nh; i++)
        {
            vis = new bool[nh];
            if (Dfs(i)) matching++;
        }

        Console.WriteLine(n - matching);
    }
}

internal sealed class FastScannerE
{
    private readonly Stream stream;
    private readonly byte[] buffer = new byte[1 << 16];
    private int pos, len;
    public FastScannerE(Stream s) { stream = s; }
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
    public long NextLong()
    {
        int c = ReadByte();
        while (c <= ' ') { if (c == 0) return 0; c = ReadByte(); }
        long res = 0;
        do { res = res * 10 + c - '0'; c = ReadByte(); } while (c >= '0' && c <= '9');
        return res;
    }
}
