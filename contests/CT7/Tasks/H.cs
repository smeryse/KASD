using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class ShortestPathDijkstra
{
    private const long INF = long.MaxValue / 2;

    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        var adj = new List<(int to, long weight)>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<(int, long)>();

        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            long w = long.Parse(parts[2]);
            adj[u].Add((v, w));
            adj[v].Add((u, w));
        }

        line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;
        parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int start = int.Parse(parts[0]);
        int end = int.Parse(parts[1]);

        var dist = new long[n + 1];
        for (int i = 1; i <= n; i++)
            dist[i] = INF;

        dist[start] = 0;
        var pq = new PriorityQueue<int, long>();
        pq.Enqueue(start, 0);

        while (pq.Count > 0)
        {
            int u = pq.Dequeue();
            if (u == end)
                break;

            foreach (var (v, w) in adj[u])
            {
                if (dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    pq.Enqueue(v, dist[v]);
                }
            }
        }

        Console.WriteLine(dist[end] == INF ? -1 : dist[end].ToString());
    }
}
