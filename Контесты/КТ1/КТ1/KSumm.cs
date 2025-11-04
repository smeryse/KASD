using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {
        int n = 5;
        int k = 10;
        List<int> a = new List<int>() { 4, 2, 6, 4, 8 };
        List<int> b = new List<int>() { 7, 3, 1, 9, 5 };

        a.Sort(); // 2, 4, 5, 6, 8
        b.Sort(); // 1, 3, 5, 7, 9

        List<int> result = new List<int>();
        Console.WriteLine(BinaryCount(a[0], b));
    }
    static int BinaryCount(int a, List<int> b)
    {
        int left = 0;
        int right = b.Count;
        while (left < right)
        {
            int mid = (left + right) / 2;
            if (b[mid] <= a) left = mid + 1;
            else right = mid;
        }
        return left;
    }
}
