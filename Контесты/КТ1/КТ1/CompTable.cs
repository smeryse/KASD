using System;

class Program
{
    static int n;
    static int k;
    static int[] arr;
    static void Main(string[] args)
    {
        string[] firstString = Console.ReadLine().Split(' ');

        n = int.Parse(firstString[0]);
        k = int.Parse(firstString[1]);

        Console.WriteLine(BinarySearch(1, n*n));
    }

    static int BinarySearch(int left, int right)
    {
        if (left == right) return left;
        int mid = (left + right) / 2;
        if (CountBefore(mid) > k) return BinarySearch(left, mid - 1);
        else return BinarySearch(mid + 1, right);
    }
    static int CountBefore(int num)
    {
        int result = 0;
        for (int i = 1; i <= n; i++)
        {
            result += Math.Min(n, (num - 1) / i);
        }
        return result - 1;
    }
}