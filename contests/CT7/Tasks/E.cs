using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class VertexBiconnectedComponents
{
    private static List<(int to, int edgeIndex)>[] adj = null!;
    private static int[] tin = null!, low = null!, edgeComponent = null!;
    private static int timer, componentCount;
    private static Stack<int> edgeStack = null!;

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
        edgeComponent = new int[m + 1];
        edgeStack = new Stack<int>();
        timer = 0;
        componentCount = 0;

        for (int i = 1; i <= n; i++)
        {
            if (tin[i] == 0)
                DFS(i, -1);
        }

        Console.WriteLine(componentCount);
        var result = new int[m];
        for (int i = 1; i <= m; i++)
            result[i - 1] = edgeComponent[i];
        Console.WriteLine(string.Join(" ", result));
    }

    private static void DFS(int u, int parentEdgeIndex)
    {
        tin[u] = low[u] = ++timer;

        foreach (var (v, edgeIndex) in adj[u])
        {
            if (edgeIndex == parentEdgeIndex)
                continue;

            if (tin[v] != 0)
            {
                low[u] = Math.Min(low[u], tin[v]);
                if (tin[v] < tin[u])
                    edgeStack.Push(edgeIndex);
            }
            else
            {
                edgeStack.Push(edgeIndex);
                DFS(v, edgeIndex);
                low[u] = Math.Min(low[u], low[v]);

                if (low[v] >= tin[u])
                {
                    componentCount++;
                    while (true)
                    {
                        int edge = edgeStack.Pop();
                        edgeComponent[edge] = componentCount;
                        if (edge == edgeIndex)
                            break;
                    }
                }
            }
        }
    }
}


