using System;
using System.Collections.Generic;

namespace CT8.Tasks;

internal class ShortestPath2
{
    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        var adj = new List<(int to, int weight)>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<(int, int)>();

        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            int w = int.Parse(parts[2]);
            adj[u].Add((v, w));
            adj[v].Add((u, w));
        }

        const long INF = long.MaxValue / 2;
        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++)
            dist[i] = INF;
        dist[1] = 0;

        // Дейкстра с приоритетной очередью
        var pq = new PriorityQueue<int, long>();
        pq.Enqueue(1, 0);

        while (pq.Count > 0)
        {
            int u = pq.Dequeue();

            foreach (var (v, w) in adj[u])
            {
                if (dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    pq.Enqueue(v, dist[v]);
                }
            }
        }

        var result = new long[n];
        for (int i = 1; i <= n; i++)
            result[i - 1] = dist[i];
        Console.WriteLine(string.Join(" ", result));
    }
}
