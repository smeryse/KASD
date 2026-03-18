using System;
using System.Collections.Generic;

namespace CT3.Tasks;

internal class Cafe
{
    private readonly int n;
    private readonly int[] cost;
    private readonly int[,] dp;
    private readonly int[,] useCoupon;
    private readonly int maxCoupons;

    public Cafe(int n, int[] cost)
    {
        this.n = n;
        this.cost = cost;
        maxCoupons = n;
        dp = new int[n + 1, maxCoupons + 2];
        useCoupon = new int[n + 1, maxCoupons + 2];

        for (int i = 0; i <= n; i++)
            for (int j = 0; j <= maxCoupons + 1; j++)
                dp[i, j] = int.MaxValue;

        dp[0, 0] = 0;
    }

    public void Solve()
    {
        for (int day = 1; day <= n; day++)
        {
            for (int coupons = 0; coupons <= day - 1; coupons++)
            {
                if (dp[day - 1, coupons] == int.MaxValue) continue;

                int newCoupons = coupons;
                if (cost[day - 1] > 100) newCoupons++;
                if (dp[day - 1, coupons] + cost[day - 1] < dp[day, newCoupons])
                {
                    dp[day, newCoupons] = dp[day - 1, coupons] + cost[day - 1];
                    useCoupon[day, newCoupons] = 0;
                }

                if (coupons > 0)
                {
                    int remainingCoupons = coupons - 1;
                    if (dp[day, remainingCoupons] > dp[day - 1, coupons])
                    {
                        dp[day, remainingCoupons] = dp[day - 1, coupons];
                        useCoupon[day, remainingCoupons] = 1;
                    }
                }
            }
        }

        int minCost = int.MaxValue;
        int remaining = 0;
        for (int c = n; c >= 0; c--)
        {
            if (dp[n, c] < minCost)
            {
                minCost = dp[n, c];
                remaining = c;
            }
        }

        var daysUsedCoupon = new List<int>();
        int dayIndex = n;
        int couponsLeft = remaining;
        while (dayIndex > 0)
        {
            if (useCoupon[dayIndex, couponsLeft] == 1)
            {
                daysUsedCoupon.Add(dayIndex);
                couponsLeft++;
            }
            else
            {
                if (cost[dayIndex - 1] > 100) couponsLeft--;
            }
            dayIndex--;
        }
        daysUsedCoupon.Reverse();

        Console.WriteLine(minCost);
        Console.WriteLine($"{remaining} {daysUsedCoupon.Count}");
        foreach (var d in daysUsedCoupon)
            Console.WriteLine(d);
    }
}

internal static class CafeTask
{
    public static void Solve()
    {
        int n = int.Parse(Console.ReadLine());
        int[] cost = new int[n];
        for (int i = 0; i < n; i++)
            cost[i] = int.Parse(Console.ReadLine());

        var cafe = new Cafe(n, cost);
        cafe.Solve();
    }
}
