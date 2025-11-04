using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
    }
    int knapSack(int W, int n, int[] weight, int[] values)
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
