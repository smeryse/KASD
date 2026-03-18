using System;

namespace CT3.Tasks;

internal class BracketRemover
{
    private readonly string s;
    private readonly int n;
    private readonly int[,] dp;
    private readonly int[,] split;

    public BracketRemover(string input)
    {
        s = input;
        n = s.Length;
        dp = new int[n, n];
        split = new int[n, n];
    }

    public void Solve()
    {
        for (int len = 1; len <= n; len++)
        {
            for (int l = 0; l + len - 1 < n; l++)
            {
                int r = l + len - 1;
                dp[l, r] = 0;
                split[l, r] = -2;

                if (IsPair(s[l], s[r]) && len > 1)
                {
                    int inner = (l + 1 <= r - 1) ? dp[l + 1, r - 1] : 0;
                    if (inner + 2 > dp[l, r])
                    {
                        dp[l, r] = inner + 2;
                        split[l, r] = -1;
                    }
                }

                for (int k = l; k < r; k++)
                {
                    if (dp[l, k] + dp[k + 1, r] > dp[l, r])
                    {
                        dp[l, r] = dp[l, k] + dp[k + 1, r];
                        split[l, r] = k;
                    }
                }
            }
        }

        Console.WriteLine(Build(0, n - 1));
    }

    private string Build(int l, int r)
    {
        if (l > r || l == r || dp[l, r] == 0) return "";

        if (split[l, r] == -1)
            return s[l] + Build(l + 1, r - 1) + s[r];
        return Build(l, split[l, r]) + Build(split[l, r] + 1, r);
    }

    private static bool IsPair(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '[' && close == ']') ||
               (open == '{' && close == '}');
    }
}

internal static class DeleteStaples
{
    public static void Solve()
    {
        string input = Console.ReadLine();
        var br = new BracketRemover(input);
        br.Solve();
    }
}
