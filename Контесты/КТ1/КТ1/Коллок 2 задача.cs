//using System;
//using System.Collections.Generic;

//class Program
//{
//    static void Main()
//    {
//        int[] arr = { 1, 1, 2, 2, 3 };
//        Dictionary<int, int> counter = new Dictionary<int, int>();

//        for (int i = 0; i < arr.Length; i++)
//        {
//            counter[arr[i]] = 0;
//        }

//        for (int i = 0; i < arr.Length; i++)
//        {
//            counter[arr[i]] += 1;
//        }

//        foreach (int i in counter.Keys)
//        {
//            if (counter[i] == 1) Console.WriteLine(i);
//        }
//    }

//}