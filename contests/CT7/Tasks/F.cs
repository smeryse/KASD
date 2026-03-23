using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class CondensationGraph
{
    private static List<int>[] adj = null!, adjRev = null!;
    private static bool[] visited = null!;
    private static List<int> order = null!;
    private static int[] component = null!;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        adj = new List<int>[n + 1];
        adjRev = new List<int>[n + 1];
        for (int i = 1; i <= n; i++)
        {
            adj[i] = new List<int>();
            adjRev[i] = new List<int>();
        }

        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            if (u == v) continue;
            adj[u].Add(v);
            adjRev[v].Add(u);
        }

        visited = new bool[n + 1];
        order = new List<int>();

        for (int i = 1; i <= n; i++)
        {
            if (!visited[i])
                DFS1(i);
        }

        component = new int[n + 1];
        int componentCount = 0;

        order.Reverse();
        foreach (int v in order)
        {
            if (component[v] == 0)
            {
                componentCount++;
                DFS2(v, componentCount);
            }
        }

        var condensationEdges = new HashSet<(int, int)>();
        for (int u = 1; u <= n; u++)
        {
            foreach (int v in adj[u])
            {
                if (component[u] != component[v])
                    condensationEdges.Add((component[u], component[v]));
            }
        }

        Console.WriteLine(condensationEdges.Count);
    }

    private static void DFS1(int u)
    {
        visited[u] = true;
        foreach (int v in adj[u])
        {
            if (!visited[v])
                DFS1(v);
        }
        order.Add(u);
    }

    private static void DFS2(int u, int comp)
    {
        component[u] = comp;
        foreach (int v in adjRev[u])
        {
            if (component[v] == 0)
                DFS2(v, comp);
        }
    }
}
