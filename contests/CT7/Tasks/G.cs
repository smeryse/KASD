using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal class ShortestPathBFS
{
    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);

        var adj = new List<int>[n + 1];
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
            adj[v].Add(u);
        }

        line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;
        parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int start = int.Parse(parts[0]);
        int end = int.Parse(parts[1]);

        var dist = new int[n + 1];
        for (int i = 1; i <= n; i++)
            dist[i] = -1;

        var queue = new Queue<int>();
        queue.Enqueue(start);
        dist[start] = 0;

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            if (u == end)
                break;

            foreach (int v in adj[u])
            {
                if (dist[v] == -1)
                {
                    dist[v] = dist[u] + 1;
                    queue.Enqueue(v);
                }
            }
        }

        Console.WriteLine(dist[end]);
    }
}
