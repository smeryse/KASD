using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CT10.Tasks;

internal static class TaskB
{
    static int[] matchR;
    static bool[] vis;
    static List<int>[] compAdj;
    static bool[,] knows;

    static bool Dfs(int u, int m, bool[] used)
    {
        if (used[u]) return false;
        used[u] = true;
        foreach (int v in compAdj[u])
        {
            if (matchR[v] == -1 || Dfs(matchR[v], m, used))
            {
                matchR[v] = u;
                return true;
            }
        }
        return false;
    }

    public static void Solve()
    {
        var fs = new FastScannerB(Console.OpenStandardInput());
        int k = fs.NextInt();

        for (int caseNum = 0; caseNum < k; caseNum++)
        {
            if (caseNum > 0) Console.WriteLine();

            int mb = fs.NextInt();
            int nb = fs.NextInt();

            knows = new bool[mb, nb];
            for (int i = 0; i < mb; i++)
            {
                while (true)
                {
                    int g = fs.NextInt();
                    if (g == 0) break;
                    knows[i, g - 1] = true;
                }
            }

            compAdj = new List<int>[mb];
            for (int i = 0; i < mb; i++)
            {
                compAdj[i] = new List<int>();
                for (int j = 0; j < nb; j++)
                {
                    if (!knows[i, j])
                        compAdj[i].Add(j);
                }
            }

            matchR = new int[nb];
            Array.Fill(matchR, -1);
            int matching = 0;
            for (int i = 0; i < mb; i++)
            {
                vis = new bool[mb];
                if (Dfs(i, nb, vis)) matching++;
            }

            bool[] used = new bool[mb];
            bool[] inVertexCoverB = new bool[mb];
            bool[] inVertexCoverG = new bool[nb];

            bool[] matchedBoy = new bool[mb];
            for (int j = 0; j < nb; j++)
                if (matchR[j] != -1) matchedBoy[matchR[j]] = true;

            List<int>[] revAdj = new List<int>[nb];
            for (int j = 0; j < nb; j++) revAdj[j] = new List<int>();
            for (int i = 0; i < mb; i++)
                foreach (int v in compAdj[i])
                    revAdj[v].Add(i);

            Array.Fill(used, false);
            var stack = new Stack<int>();
            for (int i = 0; i < mb; i++)
                if (!matchedBoy[i]) { used[i] = true; stack.Push(i); }

            while (stack.Count > 0)
            {
                int u = stack.Pop();
                foreach (int v in compAdj[u])
                {
                    if (matchR[v] != -1 && !used[matchR[v]])
                    {
                        used[matchR[v]] = true;
                        stack.Push(matchR[v]);
                    }
                }
            }

            for (int i = 0; i < mb; i++) inVertexCoverB[i] = !used[i];
            for (int j = 0; j < nb; j++)
            {
                if (matchR[j] != -1 && used[matchR[j]])
                    inVertexCoverG[j] = true;
            }

            var boys = new List<int>();
            var girls = new List<int>();
            for (int i = 0; i < mb; i++)
                if (!inVertexCoverB[i]) boys.Add(i + 1);
            for (int j = 0; j < nb; j++)
                if (!inVertexCoverG[j]) girls.Add(j + 1);

            int total = boys.Count + girls.Count;
            Console.WriteLine(total);
            Console.WriteLine($"{boys.Count} {girls.Count}");
            Console.WriteLine(string.Join(" ", boys));
            Console.WriteLine(string.Join(" ", girls));
        }
    }
}

internal sealed class FastScannerB
{
    private readonly Stream stream;
    private readonly byte[] buffer = new byte[1 << 16];
    private int pos, len;
    public FastScannerB(Stream s) { stream = s; }
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