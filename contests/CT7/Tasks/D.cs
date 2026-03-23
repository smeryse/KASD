using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class EdgeBiconnectedComponents
{
    private static List<(int to, int edgeIndex)>[] adj = null!;
    private static int[] tin = null!, low = null!, component = null!;
    private static int timer, componentCount;
    private static HashSet<int> bridgeEdges = null!;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        adj = new List<(int, int)>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<(int, int)>();

        for (int i = 1; i <= m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            adj[u].Add((v, i));
            adj[v].Add((u, i));
        }

        tin = new int[n + 1];
        low = new int[n + 1];
        component = new int[n + 1];
        bridgeEdges = new HashSet<int>();
        timer = 0;

        for (int i = 1; i <= n; i++)
        {
            if (tin[i] == 0)
                FindBridges(i, -1);
        }

        componentCount = 0;
        for (int i = 1; i <= n; i++)
        {
            if (component[i] == 0)
            {
                componentCount++;
                AssignComponent(i);
            }
        }

        Console.WriteLine(componentCount);
        var result = new int[n];
        for (int i = 1; i <= n; i++)
            result[i - 1] = component[i];
        Console.WriteLine(string.Join(" ", result));
    }

    private static void FindBridges(int u, int parentEdge)
    {
        tin[u] = low[u] = ++timer;

        foreach (var (v, edgeIndex) in adj[u])
        {
            if (edgeIndex == parentEdge)
                continue;

            if (tin[v] != 0)
            {
                low[u] = Math.Min(low[u], tin[v]);
            }
            else
            {
                FindBridges(v, edgeIndex);
                low[u] = Math.Min(low[u], low[v]);
                if (low[v] > tin[u])
                    bridgeEdges.Add(edgeIndex);
            }
        }
    }

    private static void AssignComponent(int u)
    {
        component[u] = componentCount;
        foreach (var (v, edgeIndex) in adj[u])
        {
            if (component[v] == 0 && !bridgeEdges.Contains(edgeIndex))
                AssignComponent(v);
        }
    }
}
