using System;
using System.Collections.Generic;

class Item
{
    public int Weight { get; }
    public int Value { get; }

    public Item(int weight, int value)
    {
        Weight = weight;
        Value = value;
    }
}

class Knapsack
{
    private int W;
    private int N;
    private List<Item> Items;
    private int[,] dp;

    public Knapsack(int w, List<Item> items)
    {
        W = w;
        Items = items;
        N = items.Count;
        dp = new int[N + 1, W + 1];

        // Инициализация -1 для мемоизации
        for (int i = 0; i <= N; i++)
        {
            for (int j = 0; j <= W; j++)
            {
                dp[i, j] = -1;
            }
        }
    }

    public int Solve()
    {
        return Recursive(N, W);
    }

    private int Recursive(int i, int w)
    {
        if (i == 0 || w == 0)
            return 0;

        if (dp[i, w] != -1)
            return dp[i, w];

        // Не берём текущий предмет
        int notTake = Recursive(i - 1, w);

        // Берём текущий предмет, если помещается
        int take = 0;
        if (Items[i - 1].Weight <= w)
        {
            take = Items[i - 1].Value + Recursive(i - 1, w - Items[i - 1].Weight);
        }

        dp[i, w] = Math.Max(notTake, take);
        return dp[i, w];
    }
}

class Program
{
    static void Main()
    {
        List<int> weights = new List<int> { 10, 20, 30 };
        List<int> values  = new List<int> { 60, 100, 120 };
        int W = 50;

        List<Item> items = new List<Item>();
        for (int i = 0; i < weights.Count; i++)
        {
            items.Add(new Item(weights[i], values[i]));
        }

        Knapsack knapsack = new Knapsack(W, items);
        int maxValue = knapsack.Solve();
        Console.WriteLine("Максимальная стоимость: " + maxValue); // Ожидаемый вывод: 220
    }
}
