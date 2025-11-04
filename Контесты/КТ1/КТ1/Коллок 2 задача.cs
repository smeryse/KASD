using System;
class Program
{
    static void Main(string[] args)
    {
        int[] values = { 60, 100, 120 };
        int[] weight = { 10, 20, 30};
        int W = 50;
        int n = values.Length;

        Console.WriteLine(knapSack(W, n, weight, values));
    }
    static int knapSack(int W, int n, int[] weight, int[] values)
    {
        if (n == 0 || W == 0) return 0;

        if (weight[n - 1] > W) return knapSack(W, n - 1, weight, values);
        else
        {
            return Math.Max(
                knapSack(W, n - 1, weight, values), 
                values[n - 1] + knapSack(W - weight[n - 1], n - 1, weight, values)
                );
        }
    }
}
