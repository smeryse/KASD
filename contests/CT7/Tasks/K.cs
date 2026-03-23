using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class LCABinaryLifting
{
    private static List<int>[] adj = null!;
    private static int[][] up = null!;
    private static int[] depth = null!;
    private static int LOG;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        adj = new List<int>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<int>();

        int root = 0;
        for (int i = 1; i <= n; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int parent = int.Parse(parts[0]);
            if (parent == 0)
                root = i;
            else
            {
                adj[parent].Add(i);
                adj[i].Add(parent);
            }
        }

        LOG = 0;
        while ((1 << LOG) <= n)
            LOG++;

        up = new int[n + 1][];
        depth = new int[n + 1];
        for (int i = 1; i <= n; i++)
            up[i] = new int[LOG];

        DFS(root, root, 0);

        var result = new System.Text.StringBuilder();
        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            result.Append(LCA(u, v)).Append('\n');
        }

        Console.Write(result.ToString());
    }

    private static void DFS(int u, int p, int d)
    {
        depth[u] = d;
        up[u][0] = p;

        for (int i = 1; i < LOG; i++)
            up[u][i] = up[up[u][i - 1]][i - 1];

        foreach (int v in adj[u])
        {
            if (v != p)
                DFS(v, u, d + 1);
        }
    }

    private static int LCA(int u, int v)
    {
        if (depth[u] < depth[v])
            (u, v) = (v, u);

        int diff = depth[u] - depth[v];
        for (int i = 0; i < LOG; i++)
        {
            if ((diff & (1 << i)) != 0)
                u = up[u][i];
        }

        if (u == v)
            return u;

        for (int i = LOG - 1; i >= 0; i--)
        {
            if (up[u][i] != up[v][i])
            {
                u = up[u][i];
                v = up[v][i];
            }
        }

        return up[u][0];
    }
}
