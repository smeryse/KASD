using System;

class BracketRemover
{
    private string s;
    private int n;
    private int[,] dp;
    private int[,] split;

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
                split[l, r] = -2; // нет решения

                if (IsPair(s[l], s[r]) && len > 1)
                {
                    int inner = (l + 1 <= r - 1) ? dp[l + 1, r - 1] : 0;
                    if (inner + 2 > dp[l, r])
                    {
                        dp[l, r] = inner + 2;
                        split[l, r] = -1; // использовать пару (l, r)
                    }
                }

                for (int k = l; k < r; k++)
                {
                    if (dp[l, k] + dp[k + 1, r] > dp[l, r])
                    {
                        dp[l, r] = dp[l, k] + dp[k + 1, r];
                        split[l, r] = k; // разбить в точке k
                    }
                }
            }
        }

        Console.WriteLine(Build(0, n - 1));
    }

    private string Build(int l, int r)
    {
        if (l > r) return "";
        if (l == r) return "";
        if (dp[l, r] == 0) return ""; // нет решения

        if (split[l, r] == -1)
            return s[l] + Build(l + 1, r - 1) + s[r];
        else
            return Build(l, split[l, r]) + Build(split[l, r] + 1, r);
    }

    private bool IsPair(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '[' && close == ']') ||
               (open == '{' && close == '}');
    }
}

class Program
{
    static void Main()
    {
        string input = Console.ReadLine();
        BracketRemover br = new BracketRemover(input);
        br.Solve();
    }
}