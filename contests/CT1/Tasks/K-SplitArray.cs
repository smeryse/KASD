using System;
using System.Linq;

namespace CT1.Tasks
{
    // Разбить массив на k частей с минимальным максимальным суммированием
    static class SplitArray
    {
        private static int[] arr;

        public static void Solve()
        {
            var first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int k = int.Parse(first[1]);

            arr = Console.ReadLine().Split().Select(int.Parse).ToArray();
            long lo = arr.Max();
            long hi = arr.Select(x => (long)x).Sum();

            long ans = BinarySearch(k, lo, hi);
            Console.WriteLine(ans);
        }

        private static long BinarySearch(int k, long l, long r)
        {
            while (l < r)
            {
                long m = l + (r - l) / 2;
                if (CanSplit(k, m)) r = m;
                else l = m + 1;
            }
            return l;
        }

        private static bool CanSplit(int k, long maxSum)
        {
            long cur = 0;
            int parts = 1;
            foreach (int x in arr)
            {
                if (x > maxSum) return false;
                if (cur + x > maxSum)
                {
                    parts++;
                    cur = x;
                }
                else cur += x;
            }
            return parts <= k;
        }
    }
}


