using System;
using System.Linq;

class Program
{
    static void Main()
    {
        RunAllTests();
    }

    // --- Ваша оригинальная рекурсивная реализация (без изменений) ---
    static int BinarySearchRecursive(int k, int left, int right, int[] arr)
    {
        if (left >= right) return left;
        int mid = (left + right) / 2;
        if (CanSplitRecursive(k, mid, arr)) return BinarySearchRecursive(k, left + 1, mid - 1, arr);
        else return BinarySearchRecursive(k, mid + 1, right - 1, arr);
    }

    static bool CanSplitRecursive(int k, int maxSum, int[] arr)
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

    // --- Моя итеративная реализация (использует long) ---
    static long BinarySearchIterative(int k, int[] arr)
    {
        long left = arr.Max();
        long right = arr.Select(x => (long)x).Sum();
        long result = right;

        while (left <= right)
        {
            long mid = (left + right) / 2;
            if (CanSplitIterative(k, mid, arr))
            {
                result = mid;
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }

        return result;
    }

    static bool CanSplitIterative(int k, long maxSum, int[] arr)
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

    // --- Тестирование ---
    static void RunAllTests()
    {
        var tests = new (int[] arr, int k, long expected)[]
        {
            (new int[] { 2, 4, 7, 11, 15, 21, 30 }, 2, 55),
            (new int[] { 2, 4, 7, 11, 15, 21, 30 }, 3, 36),
            (new int[] { 1, 2, 3, 4, 5 }, 2, 9),
            (new int[] { 5, 5, 5, 5, 5 }, 3, 10),
            (new int[] { 10, 20, 30 }, 1, 60),
            (new int[] { 10, 20, 30 }, 3, 30),
            (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4, 15),
            (new int[] { 1, 3, 2, 4, 10, 8, 4, 2, 5, 3 }, 4, 14),
            (new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 2, 5)
        };

        foreach (var (arr, k, expected) in tests)
        {
            int recResult = BinarySearchRecursive(k, arr.Max(), arr.Sum(), arr);
            long iterResult = BinarySearchIterative(k, arr);

            Console.WriteLine($"arr=[{string.Join(",", arr)}], k={k}, expected={expected}");
            Console.WriteLine($"  Recursive (original int): {recResult} {(recResult == expected ? "+" : "-")}");
            Console.WriteLine($"  Iterative (long): {iterResult} {(iterResult == expected ? "+" : "-")}");
            Console.WriteLine();
        }
    }
}
