using System;

namespace CT3.Tasks;

class KnightPhone
{
    private int n;
    private long mod = 1000000000;
    private long[,] dp;

    private int[][] moves = new int[][]
    {
        new int[] {4,6},    // 0
        new int[] {6,8},    // 1
        new int[] {7,9},    // 2
        new int[] {4,8},    // 3
        new int[] {0,3,9},  // 4
        new int[] {},       // 5 (конь не может ходить на 5)
        new int[] {0,1,7},  // 6
        new int[] {2,6},    // 7
        new int[] {1,3},    // 8
        new int[] {2,4}     // 9
    };

    public KnightPhone(int n)
    {
        this.n = n;
        dp = new long[n + 1, 10]; // dp[length, digit]
    }

    public void Solve()
    {
        // База: длина 1
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

static class KnightMove
{
    public static void Solve()
    {
        int n = int.Parse(Console.ReadLine());
        KnightPhone kp = new KnightPhone(n);
        kp.Solve();
    }
}
