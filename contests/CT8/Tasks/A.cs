using System;

namespace CT8.Tasks;

internal class Floyd
{
    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        int n = int.Parse(line.Trim());
        int[,] dist = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            line = Console.ReadLine();
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < n; j++)
            {
                dist[i, j] = int.Parse(parts[j]);
            }
        }

        // Алгоритм Флойда-Уоршелла
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dist[i, k] != 100000 && dist[k, j] != 100000)
                    {
                        dist[i, j] = Math.Min(dist[i, j], dist[i, k] + dist[k, j]);
                    }
                }
            }
        }

        for (int i = 0; i < n; i++)
        {
            var row = new int[n];
            for (int j = 0; j < n; j++)
            {
                row[j] = dist[i, j];
            }
            Console.WriteLine(string.Join(" ", row));
        }
    }
}
