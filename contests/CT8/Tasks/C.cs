using System;

namespace CT8.Tasks;

internal class NegativeCycle
{
    public static void Solve()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
            return;

        int n = int.Parse(line.Trim());
        const int INF = 100000;
        int[,] dist = new int[n + 1, n + 1];
        int[,] next = new int[n + 1, n + 1];

        for (int i = 1; i <= n; i++)
        {
            line = Console.ReadLine();
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 1; j <= n; j++)
            {
                dist[i, j] = int.Parse(parts[j - 1]);
                if (dist[i, j] != INF || i == j)
                    next[i, j] = j;
                else
                    next[i, j] = -1;
            }
        }

        // Флойд-Уоршелл
        for (int k = 1; k <= n; k++)
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (dist[i, k] != INF && dist[k, j] != INF && dist[i, k] + dist[k, j] < dist[i, j])
                    {
                        dist[i, j] = dist[i, k] + dist[k, j];
                        next[i, j] = next[i, k];
                    }
                }
            }
        }

        // Проверка на цикл отрицательного веса
        for (int i = 1; i <= n; i++)
        {
            if (dist[i, i] < 0)
            {
                // Нашли цикл отрицательного веса
                var cycle = new System.Collections.Generic.List<int>();
                int curr = i;
                do
                {
                    cycle.Add(curr);
                    curr = next[curr, i];
                } while (curr != -1 && curr != i && cycle.Count <= n);

                // Если цикл не замкнулся, пробуем восстановить иначе
                if (curr != i || cycle.Count > n)
                {
                    cycle.Clear();
                    // Ищем вершину на цикле
                    int v = i;
                    for (int step = 0; step < n; step++)
                        v = next[v, i];
                    
                    // Восстанавливаем цикл начиная с v
                    cycle.Add(v);
                    int start = v;
                    while (true)
                    {
                        v = next[v, i];
                        if (v == start || v == -1 || cycle.Count > n)
                            break;
                        cycle.Add(v);
                    }
                }

                Console.WriteLine("YES");
                Console.WriteLine(cycle.Count);
                Console.WriteLine(string.Join(" ", cycle));
                return;
            }
        }

        Console.WriteLine("NO");
    }
}

