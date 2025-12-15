using System;

namespace CT1.Tasks
{
    // K-я порядковая статистика в таблице умножения n x n
    static class CompTable
    {
        private static long n, k;

        public static void Solve()
        {
            var first = Console.ReadLine().Split(' ');
            n = long.Parse(first[0]);
            k = long.Parse(first[1]);
            Console.WriteLine(BinarySearch(1, n * n));
        }

        private static long BinarySearch(long l, long r)
        {
            while (l < r)
            {
                long m = (l + r) / 2;
                if (CountLE(m) >= k) r = m;
                else l = m + 1;
            }
            return l;
        }

        private static long CountLE(long x)
        {
            long res = 0;
            for (long i = 1; i <= n; i++)
                res += Math.Min(n, x / i);
            return res;
        }
    }
}


