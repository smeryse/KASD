using System;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] firstLine = Console.ReadLine().Split();
        int n = int.Parse(firstLine[0]);
        int k = int.Parse(firstLine[1]);

        int[] arr = Console.ReadLine().Split().Select(int.Parse).ToArray();

        long result = BinarySearch(k, arr.Max(), arr.Select(x => (long)x).Sum(), arr);

        Console.WriteLine(result);
    }

    static long BinarySearch(int k, long left, long right, int[] arr)
    {
        if (left >= right) return left;

        long mid = left + (right - left) / 2;

        if (CanSplit(k, mid, arr))
        {
            return BinarySearch(k, left, mid, arr);
        }
        else
        {
            return BinarySearch(k, mid + 1, right, arr);
        }
    }

    static bool CanSplit(int k, long maxSum, int[] arr)
    {
        long temp = 0;
        int count = 1;
        foreach (int i in arr)
        {
            if (i > maxSum) return false;

            if (temp + i > maxSum)
            {
                count++;
                temp = i;
            }
            else temp += i;
        }
        return count <= k;
    }
}
