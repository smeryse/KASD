using System;
using System.Collections.Generic;

namespace CT8.Tasks;

internal class ShortestPaths
{
    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);
        int s = int.Parse(parts[2]);

        var edges = new List<(int from, int to, long weight)>();
        for (int i = 0; i < m; i++)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            long w = long.Parse(parts[2]);
            edges.Add((u, v, w));
        }

        const long INF = long.MaxValue / 4;
        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++)
            dist[i] = INF;
        dist[s] = 0;

        // Bellman-Ford: n-1 итерация
        for (int i = 0; i < n - 1; i++)
        {
            bool changed = false;
            foreach (var (u, v, w) in edges)
            {
                if (dist[u] != INF && dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    changed = true;
                }
            }
            if (!changed)
                break;
        }

        // Проверка на достижимость из цикла отрицательного веса
        bool[] reachableFromNegCycle = new bool[n + 1];
        
        // Запускаем ещё n итераций для распространения -∞
        for (int i = 0; i < n; i++)
        {
            foreach (var (u, v, w) in edges)
            {
                if (dist[u] != INF && dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    reachableFromNegCycle[v] = true;
                }
                if (reachableFromNegCycle[u])
                {
                    reachableFromNegCycle[v] = true;
                }
            }
        }

        // Вывод результатов
        for (int i = 1; i <= n; i++)
        {
            if (dist[i] == INF)
                Console.WriteLine("*");
            else if (reachableFromNegCycle[i])
                Console.WriteLine("-");
            else
                Console.WriteLine(dist[i]);
        }
    }
}