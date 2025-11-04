using System;
using System.Linq;
using System.Security.AccessControl;

class Program
{
    static void Main()
    {
        int[] arr = { 2, 4, 7, 11, 15, 21, 30 };
        int result = 11;
        BinarySearch(result, arr.Max(), arr.Sum(), arr);

        Console.WriteLine(BinarySearch(result, 0, arr.Length - 1, arr));
    }

    static int BinarySearch(int result, int left, int right, int[] arr)
    {
        int mid = (left + right) / 2;
        if (result > mid)
        {

        }
    }

    static bool CanSplit(int k, int maxSum, int[] arr)
    {
        int temp = 0;
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