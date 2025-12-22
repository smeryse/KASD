using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var input = Console.ReadLine().Split();
        int n = int.Parse(input[0]);
        long s = long.Parse(input[1]);

        long[] w = new long[n];
        for (int i = 0; i < n; i++)
            w[i] = long.Parse(Console.ReadLine());

        int N = 1 << n;

        // sum[mask] — суммарный вес подмножества
        long[] sum = new long[N];
        for (int mask = 1; mask < N; mask++)
        {
            int b = mask & -mask;
            int i = BitIndex(b);
            sum[mask] = sum[mask ^ b] + w[i];
        }

        // good[mask] — помещается ли в один рюкзак
        bool[] good = new bool[N];
        for (int mask = 0; mask < N; mask++)
            good[mask] = sum[mask] <= s;

        const int INF = 1_000_000;
        int[] dp = new int[N];
        int[] parent = new int[N];

        for (int i = 0; i < N; i++)
            dp[i] = INF;

        dp[0] = 0;

        for (int mask = 1; mask < N; mask++)
        {
            for (int sub = mask; sub > 0; sub = (sub - 1) & mask)
            {
                if (!good[sub]) continue;
                if (dp[mask ^ sub] + 1 < dp[mask])
                {
                    dp[mask] = dp[mask ^ sub] + 1;
                    parent[mask] = sub;
                }
            }
        }

        Console.WriteLine(dp[N - 1]);

        // Восстановление ответа
        int cur = N - 1;
        while (cur != 0)
        {
            int sub = parent[cur];
            List<int> items = new();

            for (int i = 0; i < n; i++)
                if (((sub >> i) & 1) == 1)
                    items.Add(i + 1);

            Console.Write(items.Count);
            foreach (var x in items)
                Console.Write(" " + x);
            Console.WriteLine();

            cur ^= sub;
        }
    }

    static int BitIndex(int x)
    {
        int i = 0;
        while ((x >> i) != 1) i++;
        return i;
    }
}
