using System;
using System.Collections.Generic;
using System.IO;

namespace CT7.Tasks;

internal class TopologicalSort
{
    private static List<int>[] adj = null!;
    private static int[] state = null!;
    private static List<int> result = null!;
    private static bool hasCycle;

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
        }

        state = new int[n + 1];
        result = new List<int>();
        hasCycle = false;

        for (int i = 1; i <= n; i++)
        {
            if (state[i] == 0)
            {
                DFS(i);
                if (hasCycle)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }
        }

        result.Reverse();
        Console.WriteLine(string.Join(" ", result));
    }

    private static void DFS(int u)
    {
        state[u] = 1;

        foreach (int v in adj[u])
        {
            if (state[v] == 1)
            {
                hasCycle = true;
                return;
            }
            if (state[v] == 0)
            {
                DFS(v);
                if (hasCycle)
                    return;
            }
        }

        state[u] = 2;
        result.Add(u);
    }
}
