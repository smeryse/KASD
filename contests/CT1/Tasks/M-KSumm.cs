using System;
using System.Linq;

namespace CT1.Tasks
{
    // k-я минимальная сумма пары (a[i] + b[j])
    static class KSumm
    {
        private static long[] a;
        private static long[] b;
        private static int n;
        private static long k;

        public static void Solve()
        {
            var first = Console.ReadLine().Split();
            n = int.Parse(first[0]);
            k = long.Parse(first[1]);

            a = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
            b = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
            Array.Sort(a);
            Array.Sort(b);

            Console.WriteLine(KSum());
        }

        private static long CountPairs(long curSum)
        {
            long cnt = 0;
            for (int i = 0; i < n; i++)
            {
                int l = 0, r = n;
                while (l < r)
                {
                    int m = (l + r) / 2;
                    if (b[m] + a[i] <= curSum) l = m + 1;
                    else r = m;
                }
                cnt += l;
            }
            return cnt;
        }

        private static long KSum()
        {
            long l = a[0] + b[0];
            long r = a[a.Length - 1] + b[b.Length - 1];
            while (l < r)
            {
                long m = (l + r) / 2;
                if (CountPairs(m) < k) l = m + 1;
                else r = m;
            }
            return l;
        }
    }
}


