using System;
using System.Linq;
using System.Security.AccessControl;

class Program
{
    static void Main()
    {
        int[] arr = { 1, 8, 24, 84, 13, 40, 3 };
        int result = 3;
        BinarySearch(result, arr.Max(), arr.Sum(), arr);

        Console.WriteLine(BinarySearch(result, 0, arr.Length - 1, arr));
    }

    static int BinarySearch(int k, int left, int right, int[] arr)
    {
        if (left == right) return left;

        int mid = (left + right) / 2;
        if (CanSplit(k, mid, arr)) return BinarySearch(k, left, mid - 1, arr);
        else return BinarySearch(k, mid + 1, right, arr);
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