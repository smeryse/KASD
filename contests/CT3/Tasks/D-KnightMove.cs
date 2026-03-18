using System;

namespace CT3.Tasks;

internal class KnightPhone
{
    private readonly int n;
    private readonly long mod = 1000000000;
    private readonly long[,] dp;

    private readonly int[][] moves =
    [
        new[] { 4, 6 },    // 0
        new[] { 6, 8 },    // 1
        new[] { 7, 9 },    // 2
        new[] { 4, 8 },    // 3
        new[] { 0, 3, 9 }, // 4
        [],               // 5 (конь не может ходить на 5)
        new[] { 0, 1, 7 }, // 6
        new[] { 2, 6 },    // 7
        new[] { 1, 3 },    // 8
        new[] { 2, 4 }     // 9
    ];

    public KnightPhone(int n)
    {
        this.n = n;
        dp = new long[n + 1, 10];
    }

    public void Solve()
    {
        for (int d = 0; d <= 9; d++)
        {
            if (d != 0 && d != 8) dp[1, d] = 1;
        }

        for (int len = 2; len <= n; len++)
        {
            for (int d = 0; d <= 9; d++)
            {
                foreach (int prev in moves[d])
                {
                    dp[len, d] = (dp[len, d] + dp[len - 1, prev]) % mod;
                }
            }
        }

        long result = 0;
        for (int d = 0; d <= 9; d++) result = (result + dp[n, d]) % mod;

        Console.WriteLine(result);
    }
}

internal static class KnightMove
{
    public static void Solve()
    {
        int n = int.Parse(Console.ReadLine());
        var kp = new KnightPhone(n);
        kp.Solve();
    }
}
