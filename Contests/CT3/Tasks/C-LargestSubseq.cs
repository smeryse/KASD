using System;
using System.Collections.Generic;

namespace CT3.Tasks;

internal class LIS
{
    private readonly int n;
    private readonly int[] a;
    private readonly int[] dp;
    private readonly int[] prev;

    public LIS(int[] sequence)
    {
        n = sequence.Length;
        a = sequence;
        dp = new int[n];
        prev = new int[n];

        for (int i = 0; i < n; i++)
        {
            dp[i] = 1;
            prev[i] = -1;
        }
    }

    public void Solve()
    {
        int maxLen = 0;
        int lastIndex = -1;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (a[j] < a[i] && dp[j] + 1 > dp[i])
                {
                    dp[i] = dp[j] + 1;
                    prev[i] = j;
                }
            }

            if (dp[i] > maxLen)
            {
                maxLen = dp[i];
                lastIndex = i;
            }
        }

        Console.WriteLine(maxLen);

        var path = new List<int>();
        int pos = lastIndex;
        while (pos != -1)
        {
            path.Add(a[pos]);
            pos = prev[pos];
        }
        path.Reverse();

        Console.WriteLine(string.Join(" ", path));
    }
}

internal static class LargestSubseq
{
    public static void Solve()
    {
        _ = int.Parse(Console.ReadLine());
        string[] line = Console.ReadLine().Split();
        int[] sequence = Array.ConvertAll(line, int.Parse);

        var lis = new LIS(sequence);
        lis.Solve();
    }
}
