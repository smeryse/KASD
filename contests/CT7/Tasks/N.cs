using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class TreeDP
{
    private static List<int>[] adj = null!;
    private static bool[] visited = null!;

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

        var dp0 = new long[n + 1];
        var dp1 = new long[n + 1];
        visited = new bool[n + 1];

        DFS(1, dp0, dp1);

        Console.WriteLine(Math.Max(dp0[1], dp1[1]));
    }

    private static void DFS(int u, long[] dp0, long[] dp1)
    {
        visited[u] = true;
        dp1[u] = 1;
        dp0[u] = 0;

        foreach (int v in adj[u])
        {
            if (!visited[v])
            {
                DFS(v, dp0, dp1);
                dp1[u] += dp0[v];
                dp0[u] += Math.Max(dp0[v], dp1[v]);
            }
        }
    }
}
