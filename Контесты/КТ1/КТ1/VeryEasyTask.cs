//using System;

//class Program
//{
//    static void Main(string[] args)
//    {
//        string[] input = Console.ReadLine().Split(' ');
//        int n = int.Parse(input[0]);
//        int x = int.Parse(input[1]);
//        int y = int.Parse(input[2]);

//        long minTime = Copier.CalculateMinTime(n, x, y);
//        Console.WriteLine(minTime);
//    }
//}

//static class Copier
//{
//    public static long CalculateMinTime(int n, int x, int y)
//    {
//        int firstCopyTime = Math.Min(x, y);

//        if (n == 1)
//            return firstCopyTime;

//        n--;

//        long left = 0;
//        long right = (long)n * Math.Min(x, y);

//        while (left < right)
//        {
//            long mid = (left + right) / 2;
//            long copiesMade = mid / x + mid / y;

//            if (copiesMade >= n)
//                right = mid;
//            else
//                left = mid + 1;
//        }

//        return firstCopyTime + left;
//    }
//}
