using System;
using System.Collections.Generic;

namespace CT8.Tasks;

internal class ShortestPathK
{
    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);
        int k = int.Parse(parts[2]);
        int s = int.Parse(parts[3]);

        var edges = new List<(int from, int to, int weight)>();
        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            int w = int.Parse(parts[2]);
            edges.Add((u, v, w));
        }

        const long INF = long.MaxValue / 4;
        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++)
            dist[i] = INF;
        dist[s] = 0;

        // K итераций Bellman-Ford style
        for (int step = 0; step < k; step++)
        {
            long[] newDist = new long[n + 1];
            for (int i = 1; i <= n; i++)
                newDist[i] = INF;

            foreach (var (u, v, w) in edges)
            {
                if (dist[u] != INF)
                {
                    newDist[v] = Math.Min(newDist[v], dist[u] + w);
                }
            }

            dist = newDist;
        }

        for (int i = 1; i <= n; i++)
        {
            if (dist[i] == INF)
                Console.WriteLine(-1);
            else
                Console.WriteLine(dist[i]);
        }
    }
}