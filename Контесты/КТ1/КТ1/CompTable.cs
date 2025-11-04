using System;

class Program
{
    static int n;
    static int k;
    static int[] arr;
    static void Main(string[] args)
    {
        int n = 3;
        int k = 4;

        Console.WriteLine(CountBefore(4));
    }

    static int BinarySearch(int left, int right)
    {
        if (left == right) return left;
        int mid = (left + right) / 2;
        if (CountBefore(mid) > k) return BinarySearch(left, mid);
        else return BinarySearch(mid, right);
    }
    static int CountBefore(int num)
    {
        int result = 0;
        for (int i = 1; i <= n; i++)
        {
            result += num / i - 1;
        }
        return result;
    }
}