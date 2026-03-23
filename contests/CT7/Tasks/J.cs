using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class MSTKruskal
{
    private static int[] parent = null!, rank = null!;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        var edges = new List<(int u, int v, long w)>();

        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            long w = long.Parse(parts[2]);
            edges.Add((u, v, w));
        }

        edges.Sort((a, b) => a.w.CompareTo(b.w));

        parent = new int[n + 1];
        rank = new int[n + 1];
        for (int i = 1; i <= n; i++)
        {
            parent[i] = i;
            rank[i] = 0;
        }

        long mstWeight = 0;
        int edgesCount = 0;

        foreach (var (u, v, w) in edges)
        {
            int rootU = Find(u);
            int rootV = Find(v);

            if (rootU != rootV)
            {
                mstWeight += w;
                edgesCount++;
                Union(rootU, rootV);
            }
        }

        if (edgesCount < n - 1 && n > 1)
            Console.WriteLine("No solution");
        else
            Console.WriteLine(mstWeight);
    }

    private static int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    private static void Union(int x, int y)
    {
        if (rank[x] < rank[y])
            parent[x] = y;
        else if (rank[x] > rank[y])
            parent[y] = x;
        else
        {
            parent[y] = x;
            rank[x]++;
        }
    }
}
