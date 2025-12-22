using System;

class Levenshtein
{
    private string s1, s2;
    private int n, m;
    private int[,] dp;

    public Levenshtein(string s1, string s2)
    {
        this.s1 = s1;
        this.s2 = s2;
        n = s1.Length;
        m = s2.Length;
        dp = new int[n + 1, m + 1];

        // Инициализация базовых случаев
        for (int i = 0; i <= n; i++)
            dp[i, 0] = i; // i удалений
        for (int j = 0; j <= m; j++)
            dp[0, j] = j; // j вставок
    }

    public int Solve()
    {
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;

                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1,    // удаление
                             dp[i, j - 1] + 1),   // вставка
                    dp[i - 1, j - 1] + cost       // замена (или ничего, если символы равны)
                );
            }
        }

        return dp[n, m];
    }
}

class Program
{
    static void Main()
    {
        string s1 = Console.ReadLine();
        string s2 = Console.ReadLine();

        Levenshtein lev = new Levenshtein(s1, s2);
        Console.WriteLine(lev.Solve());
    }
}
