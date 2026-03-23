using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class CentroidDecomposition
{
    private static List<int>[] adj = null!;
    private static bool[] removed = null!;
    private static int[] subtreeSize = null!, centroidParent = null!;
    private static int n;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        adj = new List<int>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<int>();

        for (int i = 0; i < n - 1; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            adj[u].Add(v);
            adj[v].Add(u);
        }

        removed = new bool[n + 1];
        subtreeSize = new int[n + 1];
        centroidParent = new int[n + 1];

        BuildCentroidDecomposition(1, 0);

        var result = new System.Text.StringBuilder();
        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            result.Append(Distance(u, v)).Append('\n');
        }

        Console.Write(result.ToString());
    }

    private static void BuildCentroidDecomposition(int u, int parent)
    {
        CalcSubtreeSize(u, parent);
        int total = subtreeSize[u];
        int centroid = FindCentroid(u, parent, total);

        removed[centroid] = true;
        centroidParent[centroid] = parent;

        foreach (int v in adj[centroid])
        {
            if (!removed[v])
                BuildCentroidDecomposition(v, centroid);
        }
    }

    private static void CalcSubtreeSize(int u, int parent)
    {
        subtreeSize[u] = 1;
        foreach (int v in adj[u])
        {
            if (v != parent && !removed[v])
            {
                CalcSubtreeSize(v, u);
                subtreeSize[u] += subtreeSize[v];
            }
        }
    }

    private static int FindCentroid(int u, int parent, int total)
    {
        foreach (int v in adj[u])
        {
            if (v != parent && !removed[v] && subtreeSize[v] > total / 2)
                return FindCentroid(v, u, total);
        }
        return u;
    }

    private static int Distance(int u, int v)
    {
        var depth = new int[n + 1];
        var parent = new int[n + 1];
        DFS(u, -1, 0, parent, depth);

        int lca = LCA(u, v, parent, depth);
        return depth[u] + depth[v] - 2 * depth[lca];
    }

    private static void DFS(int u, int p, int d, int[] parent, int[] depth)
    {
        depth[u] = d;
        parent[u] = p;
        foreach (int v in adj[u])
        {
            if (v != p)
                DFS(v, u, d + 1, parent, depth);
        }
    }

    private static int LCA(int u, int v, int[] parent, int[] depth)
    {
        while (depth[u] > depth[v])
            u = parent[u];
        while (depth[v] > depth[u])
            v = parent[v];
        while (u != v)
        {
            u = parent[u];
            v = parent[v];
        }
        return u;
    }
}
