using System;
using System.Text;

namespace CT3.Tasks;

static class TurtleAndCoins
{
    public static void Solve()
    {
        var first = Console.ReadLine().Split();
        int n = int.Parse(first[0]);
        int m = int.Parse(first[1]);

        int[,] a = new int[n, m];
        for (int i = 0; i < n; i++)
        {
            var row = Console.ReadLine().Split();
            for (int j = 0; j < m; j++)
                a[i, j] = int.Parse(row[j]);
        }

        int[,] dp = new int[n, m];
        char[,] prev = new char[n, m]; 

        dp[0, 0] = a[0, 0];

        for (int j = 1; j < m; j++)
        {
            dp[0, j] = dp[0, j - 1] + a[0, j];
            prev[0, j] = 'R';
        }

        for (int i = 1; i < n; i++)
        {
            dp[i, 0] = dp[i - 1, 0] + a[i, 0];
            prev[i, 0] = 'D';
        }

        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
            {
                if (dp[i - 1, j] > dp[i, j - 1])
                {
                    dp[i, j] = dp[i - 1, j] + a[i, j];
                    prev[i, j] = 'D';
                }
                else
                {
                    dp[i, j] = dp[i, j - 1] + a[i, j];
                    prev[i, j] = 'R';
                }
            }
        }

        StringBuilder path = new StringBuilder();
        int x = n - 1, y = m - 1;

        while (x > 0 || y > 0)
        {
            char p = prev[x, y];
            path.Append(p);

            if (p == 'D') x--;
            else y--;
        }

        // ответ
        Console.WriteLine(dp[n - 1, m - 1]);
        char[] resultPath = path.ToString().ToCharArray();
        Array.Reverse(resultPath);
        Console.WriteLine(new string(resultPath));
    }
}
