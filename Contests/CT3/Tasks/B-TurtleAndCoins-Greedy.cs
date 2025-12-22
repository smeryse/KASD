using System;
using System.Collections.Generic;

namespace CT3.Tasks;

static class TurtleAndCoinsGreedy
{
    public static void Solve()
    {
        var nm = Console.ReadLine().Split();
        int n = int.Parse(nm[0]);
        int m = int.Parse(nm[1]);

        int[,] a = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            var row = Console.ReadLine().Split();
            for (int j = 0; j < m; j++)
                a[i, j] = int.Parse(row[j]);
        }

        int x = 0, y = 0;
        int sum = a[0, 0];
        var path = new List<char>();

        while (x < n - 1 || y < m - 1)
        {
            if (x == n - 1)
            {
                y++;
                path.Add('R');
            }
            else if (y == m - 1)
            {
                x++;
                path.Add('D');
            }
            else
            {
                // ЖАДНЫЙ ВЫБОР
                if (a[x, y + 1] >= a[x + 1, y])
                {
                    y++;
                    path.Add('R');
                }
                else
                {
                    x++;
                    path.Add('D');
                }
            }

            sum += a[x, y];
        }

        Console.WriteLine(sum);
        Console.WriteLine(new string(path.ToArray()));
    }
}
