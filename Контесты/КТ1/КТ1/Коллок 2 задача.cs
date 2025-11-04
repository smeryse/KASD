using System;
using System.Linq;
using System.Security.AccessControl;

class Program
{
    static void Main()
    {
        RunTests();
    }

    static void RunTests()
    {
        Test(new int[] { 2, 4, 7, 11, 15, 21, 30 }, 2, 55);
        Test(new int[] { 2, 4, 7, 11, 15, 21, 30 }, 3, 36);
        Test(new int[] { 1, 2, 3, 4, 5 }, 2, 9);
        Test(new int[] { 5, 5, 5, 5, 5 }, 3, 10);
        Test(new int[] { 10, 20, 30 }, 1, 60);
        Test(new int[] { 10, 20, 30 }, 3, 30);
        Test(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4, 15);
    }

    static void Test(int[] arr, int k, int expected)
    {
        int left = arr.Max(), right = arr.Sum();

        int result = BinarySearch(k, left, right, arr);

        Console.WriteLine($"arr=[{string.Join(",", arr)}], k={k} => min max sum: {result} (expected {expected}) " +
                          $"{(result == expected ? "✅" : "❌")}");
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