using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class TwoSAT
{
    private static List<int>[] adj = null!, adjRev = null!;
    private static bool[] visited = null!;
    private static List<int> order = null!;
    private static int[] component = null!, assignment = null!;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        adj = new List<int>[2 * n + 1];
        adjRev = new List<int>[2 * n + 1];
        for (int i = 1; i <= 2 * n; i++)
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
            int a = int.Parse(parts[0]);
            int b = int.Parse(parts[1]);

            int aVar = Math.Abs(a);
            int bVar = Math.Abs(b);
            int aNeg = a > 0 ? aVar + n : aVar;
            int aPos = a > 0 ? aVar : aVar + n;
            int bNeg = b > 0 ? bVar + n : bVar;
            int bPos = b > 0 ? bVar : bVar + n;

            AddEdge(aNeg, bPos);
            AddEdge(bNeg, aPos);
        }

        visited = new bool[2 * n + 1];
        order = new List<int>();

        for (int i = 1; i <= 2 * n; i++)
        {
            if (!visited[i])
                DFS1(i);
        }

        component = new int[2 * n + 1];
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

        assignment = new int[n + 1];
        for (int i = 1; i <= n; i++)
        {
            if (component[i] == component[i + n])
            {
                Console.WriteLine("No solution");
                return;
            }
            assignment[i] = component[i] > component[i + n] ? 0 : 1;
        }

        var result = new System.Text.StringBuilder();
        for (int i = 1; i <= n; i++)
            result.Append(assignment[i]).Append(' ');
        Console.WriteLine(result.ToString().Trim());
    }

    private static void AddEdge(int u, int v)
    {
        adj[u].Add(v);
        adjRev[v].Add(u);
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
