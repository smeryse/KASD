using System;

class Program
{
    static long n;
    static long k;
    static void Main(string[] args)
    {
        string[] firstString = Console.ReadLine().Split(' ');

        n = int.Parse(firstString[0]);
        k = int.Parse(firstString[1]);

        Console.WriteLine(BinarySearch(1, n*n));
    }

    static long BinarySearch(long left, long right)
    {
        if (left == right) return left;
        long mid = (left + right) / 2;
        if (CountBefore(mid) >= k) return BinarySearch(left, mid - 1);
        else return BinarySearch(mid + 1, right);
    }
    static long CountBefore(long num)
    {
        long result = 0;
        for (long i = 1; i <= n; i++)
        {
            result += Math.Min(n, num / i);
        }
        return result - 1;
    }
}