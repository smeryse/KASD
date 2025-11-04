using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {
        int n = 5;
        int k = 10;
        int[] a = { 4, 2, 6, 4, 8 };
        int[] b = { 7, 3, 1, 9, 5 };

        List<int> result = new List<int>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result.Add(a[i] + b[j]);
            }
        }
        
        result.Sort();
        Console.WriteLine(result[k]);
    }
}