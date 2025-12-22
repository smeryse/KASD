using System;
using System.Collections.Generic;

class Cafe
{
    private int n;
    private int[] cost;
    private int[,] dp;
    private int[,] useCoupon; // 1 если купон использован, 0 иначе
    private int maxCoupons;

    public Cafe(int n, int[] cost)
    {
        this.n = n;
        this.cost = cost;
        this.maxCoupons = n; // максимум купонов не больше дней
        dp = new int[n + 1, maxCoupons + 2]; // +2 на всякий случай
        useCoupon = new int[n + 1, maxCoupons + 2];

        for (int i = 0; i <= n; i++)
            for (int j = 0; j <= maxCoupons + 1; j++)
                dp[i, j] = int.MaxValue;
        
        dp[0, 0] = 0; // базовое состояние: день 0, купонов 0, сумма 0
    }

    public void Solve()
    {
        for (int day = 0; day < n; day++)
        {
            for (int coupons = 0; coupons <= n; coupons++)
            {
                if (dp[day, coupons] == int.MaxValue) continue;

                // 1. Не использовать купон
                int newCoupons = coupons;
                if (cost[day] > 100) newCoupons++; // получаем новый купон
                if (dp[day, coupons] + cost[day] < dp[day + 1, newCoupons])
                {
                    dp[day + 1, newCoupons] = dp[day, coupons] + cost[day];
                    useCoupon[day + 1, newCoupons] = 0; // купон не использован
                }

                // 2. Использовать купон, если есть
                if (coupons > 0)
                {
                    int remainingCoupons = coupons - 1;
                    if (dp[day + 1, remainingCoupons] > dp[day, coupons])
                    {
                        dp[day + 1, remainingCoupons] = dp[day, coupons];
                        useCoupon[day + 1, remainingCoupons] = 1; // купон использован
                    }
                }
            }
        }

        // ищем минимальную сумму и количество оставшихся купонов
        int minCost = int.MaxValue;
        int remaining = 0;
        for (int c = 0; c <= n; c++)
        {
            if (dp[n, c] < minCost || (dp[n, c] == minCost && c > remaining))
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
                couponsLeft++; // в прошлом дне мы использовали купон, поэтому добавляем его назад
            }
            else
            {
                if (cost[dayIndex - 1] > 100) couponsLeft--; // в прошлом дне получили купон
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
