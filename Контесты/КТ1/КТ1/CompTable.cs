using System;

class Program
{
    static void Main(string[] args)
    {
        int n = 3;
        CountingSort(n);
    }
    static void CountingSort(int n)
    {
        if (n == 0) return;

        int min = 1;
        int max = n*n;

        int[] count = new int[max + 1];

        for (int val = 1; val <= n * n; val++)
            for (int k = 1; k < count[val]; k++)
                Console.WriteLine(val);
    }

}
