using System;
using System.Linq;
using System.Security.AccessControl;

class Program
{
    static void Main()
    {
        int[] arr = { 2, 4, 7, 11, 15, 21, 30 };
        int num = 11;
        BinarySearch(num, arr.Max(), arr.Sum(), arr);

        Console.WriteLine(BinarySearch(num, 0, arr.Length - 1, arr));
    }

    static int BinarySearch(int num, int left, int right, int[] arr)
    {
        int mid = (left + right) / 2;
        if (num > mid)
        {

        }
    }

    static bool CanSplit(int k, int maxSum, int[] arr)
    {
        int temp = 0;
        int count = 0;
        foreach (int i in arr)
        {
            if (temp >= maxSum)
            {
                temp = 0;
                count++;
            }
            else
            {
                temp += i;
            }
        }
        return count == k;
    }
}