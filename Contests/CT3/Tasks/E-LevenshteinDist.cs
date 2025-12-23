using System;

namespace CT3.Tasks;

internal class Levenshtein
{
    private readonly string s1;
    private readonly string s2;
    private readonly int n;
    private readonly int m;
    private readonly int[,] dp;

    public Levenshtein(string s1, string s2)
    {
        this.s1 = s1;
        this.s2 = s2;
        n = s1.Length;
        m = s2.Length;
        dp = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++)
            dp[i, 0] = i;
        for (int j = 0; j <= m; j++)
            dp[0, j] = j;
    }

    public int Solve()
    {
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;

                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1,
                             dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost
                );
            }
        }

        return dp[n, m];
    }
}

internal static class LevenshteinDist
{
    public static void Solve()
    {
        string s1 = Console.ReadLine();
        string s2 = Console.ReadLine();

        var lev = new Levenshtein(s1, s2);
        Console.WriteLine(lev.Solve());
    }
}
