using System;
using System.Collections.Generic;

class Cafe
{
    private int n;
    private int[] cost;
    private int[,] dp;
    private int[,] useCoupon;
    private int maxCoupons;

    public Cafe(int n, int[] cost)
    {
        this.n = n;
        this.cost = cost;
        this.maxCoupons = n; 
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

            // 1. Не использовать купон
            int newCoupons = coupons;
            if (cost[day - 1] > 100) newCoupons++;
            if (dp[day - 1, coupons] + cost[day - 1] < dp[day, newCoupons])
            {
                dp[day, newCoupons] = dp[day - 1, coupons] + cost[day - 1];
                useCoupon[day, newCoupons] = 0;
            }

            // 2. Использовать купон, если есть
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

    // ищем минимальную сумму и количество оставшихся купонов
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

    // восстановление пути
    List<int> daysUsedCoupon = new List<int>();
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

    // вывод
    Console.WriteLine(minCost);
    Console.WriteLine($"{remaining} {daysUsedCoupon.Count}");
    foreach (var d in daysUsedCoupon)
        Console.WriteLine(d);
}


class Program
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        int[] cost = new int[n];
        for (int i = 0; i < n; i++)
            cost[i] = int.Parse(Console.ReadLine());

        Cafe cafe = new Cafe(n, cost);
        cafe.Solve();
    }
}
