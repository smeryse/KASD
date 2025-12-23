using System;
using System.Collections.Generic;

namespace CT3.Tasks;

internal class Grasshopper
{
    private readonly int n;
    private readonly int k;
    private readonly int[] coins;
    private readonly int[] dp;
    private readonly int[] prev;

    public Grasshopper(int n, int k, int[] coinsInput)
    {
        this.n = n;
        this.k = k;
        coins = new int[n + 1];
        dp = new int[n + 1];
        prev = new int[n + 1];

        coins[1] = 0;
        for (int i = 2; i <= n - 1; i++)
        {
            coins[i] = coinsInput[i - 2];
        }
        coins[n] = 0;

        for (int i = 0; i <= n; i++)
        {
            dp[i] = int.MinValue;
            prev[i] = -1;
        }
        dp[1] = 0;
    }

    public void Solve()
    {
        for (int i = 2; i <= n; i++)
        {
            for (int j = Math.Max(1, i - k); j < i; j++)
            {
                if (dp[j] + coins[i] > dp[i])
                {
                    dp[i] = dp[j] + coins[i];
                    prev[i] = j;
                }
            }
        }

        Console.WriteLine(dp[n]);

        var path = new List<int>();
        int pos = n;
        while (pos != -1)
        {
            path.Add(pos);
            pos = prev[pos];
        }
        path.Reverse();

        Console.WriteLine(path.Count - 1);
        Console.WriteLine(string.Join(" ", path));
    }
}

internal static class GrasshopperCollect
{
    public static void Solve()
    {
        string[] firstLine = Console.ReadLine().Split();
        int n = int.Parse(firstLine[0]);
        int k = int.Parse(firstLine[1]);

        string[] secondLine = Console.ReadLine().Split();
        int[] coinsInput = Array.ConvertAll(secondLine, int.Parse);

        var g = new Grasshopper(n, k, coinsInput);
        g.Solve();
    }
}
