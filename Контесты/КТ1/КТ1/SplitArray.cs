//using System;
//using System.Linq;

//class Program
//{
//    static int[] arr;
//    static void Main()
//    {
//        string[] firstLine = Console.ReadLine().Split();
//        int n = int.Parse(firstLine[0]);
//        int k = int.Parse(firstLine[1]);

//        arr = Console.ReadLine().Split().Select(int.Parse).ToArray();

//        long result = BinarySearch(k, arr.Max(), arr.Select(x => (long)x).Sum());

//        Console.WriteLine(result);
//    }

//    static long BinarySearch(int k, long left, long right)
//    {
//        if (left >= right) return left;

//        long mid = left + (right - left) / 2;

//        if (CanSplit(k, mid))
//        {
//            return BinarySearch(k, left, mid);
//        }
//        else
//        {
//            return BinarySearch(k, mid + 1, right);
//        }
//    }

//    static bool CanSplit(int k, long maxSum)
//    {
//        long temp = 0;
//        int count = 1;
//        foreach (int i in arr)
//        {
//            if (i > maxSum) return false;

//            if (temp + i > maxSum)
//            {
//                count++;
//                temp = i;
//            }
//            else temp += i;
//        }
//        return count <= k;
//    }
//}
