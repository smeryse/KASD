using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class MaxFlowFordFulkerson
{
    private static List<(int to, int cap, int flow, int rev)>[] adj = null!;
    private static bool[] visited = null!;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        adj = new List<(int, int, int, int)>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<(int, int, int, int)>();

        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            int cap = int.Parse(parts[2]);

            adj[u].Add((v, cap, 0, adj[v].Count));
            adj[v].Add((u, 0, 0, adj[u].Count - 1));
        }

        line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;
        parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int s = int.Parse(parts[0]);
        int t = int.Parse(parts[1]);

        Console.WriteLine(MaxFlow(s, t));
    }

    private static int MaxFlow(int s, int t)
    {
        int flow = 0;

        while (true)
        {
            visited = new bool[adj.Length];
            var parent = new (int from, int edgeIndex)[adj.Length];
            var queue = new Queue<int>();
            queue.Enqueue(s);
            visited[s] = true;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                if (u == t)
                    break;

                for (int i = 0; i < adj[u].Count; i++)
                {
                    var (v, cap, f, rev) = adj[u][i];
                    if (!visited[v] && cap - f > 0)
                    {
                        visited[v] = true;
                        parent[v] = (u, i);
                        queue.Enqueue(v);
                    }
                }
            }

            if (!visited[t])
                break;

            int push = int.MaxValue;
            int cur = t;
            while (cur != s)
            {
                var (from, idx) = parent[cur];
                push = Math.Min(push, adj[from][idx].cap - adj[from][idx].flow);
                cur = from;
            }

            cur = t;
            while (cur != s)
            {
                var (from, idx) = parent[cur];
                var edge = adj[from][idx];
                adj[from][idx] = (edge.to, edge.cap, edge.flow + push, edge.rev);
                var backEdge = adj[cur][edge.rev];
                adj[cur][edge.rev] = (backEdge.to, backEdge.cap, backEdge.flow - push, backEdge.rev);
                cur = from;
            }

            flow += push;
        }

        return flow;
    }
}
