using System;
using System.Collections.Generic;

class NicePatterns
{
    private int n, m;
    private Dictionary<(int, int), long> dp;

    public NicePatterns(int n, int m)
    {
        this.n = n;
        this.m = m;
        dp = new Dictionary<(int, int), long>();
    }

    public long Solve()
    {
        return Dfs(0, -1);
    }

    private long Dfs(int row, int prevMask)
    {
        // Все строки обработаны
        if (row == n)
            return 1;

        var key = (row, prevMask);
        if (dp.ContainsKey(key))
            return dp[key];

        long res = 0;
        int limit = 1 << m;

        for (int curMask = 0; curMask < limit; curMask++)
        {
            if (IsValid(prevMask, curMask))
            {
                res += Dfs(row + 1, curMask);
            }
        }

        dp[key] = res;
        return res;
    }

    private bool IsValid(int prev, int cur)
    {
        // Для первой строки ограничений нет
        if (prev == -1)
            return true;

        for (int i = 0; i + 1 < m; i++)
        {
            int a = (prev >> i) & 1;
            int b = (prev >> (i + 1)) & 1;
            int c = (cur >> i) & 1;
            int d = (cur >> (i + 1)) & 1;

            // Запрещённый 2×2
            if (a == b && b == c && c == d)
                return false;
        }

        return true;
    }
}

class Program
{
    static void Main()
    {
        string[] input = Console.ReadLine().Split();
        int n = int.Parse(input[0]);
        int m = int.Parse(input[1]);

        // Всегда выгоднее делать DP по меньшей стороне
        if (n < m)
        {
            int tmp = n;
            n = m;
            m = tmp;
        }

        NicePatterns solver = new NicePatterns(n, m);
        Console.WriteLine(solver.Solve());
    }
}
