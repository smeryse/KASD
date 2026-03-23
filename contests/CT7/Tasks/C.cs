using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class ArticulationPoints
{
    private static List<int>[] adj = null!;
    private static int[] tin = null!, low = null!;
    private static int timer;
    private static HashSet<int> articulationPoints = null!;

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

        for (int i = 0; i < m; i++)
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

        tin = new int[n + 1];
        low = new int[n + 1];
        articulationPoints = new HashSet<int>();
        timer = 0;

        for (int i = 1; i <= n; i++)
        {
            if (tin[i] == 0)
                DFS(i, -1);
        }

        var result = new List<int>(articulationPoints);
        result.Sort();
        Console.WriteLine(result.Count);
        Console.WriteLine(string.Join(" ", result));
    }

    private static void DFS(int u, int parent)
    {
        tin[u] = low[u] = ++timer;
        int children = 0;

        foreach (int v in adj[u])
        {
            if (v == parent)
                continue;

            if (tin[v] != 0)
            {
                low[u] = Math.Min(low[u], tin[v]);
            }
            else
            {
                children++;
                DFS(v, u);
                low[u] = Math.Min(low[u], low[v]);
                if (parent != -1 && low[v] >= tin[u])
                    articulationPoints.Add(u);
            }
        }

        if (parent == -1 && children > 1)
            articulationPoints.Add(u);
    }
}
