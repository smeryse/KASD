using System;
using System.Linq;

namespace CT1.Tasks
{
    // Ещё один вариант подсчёта инверсий
    static class CountInversions
    {
        public static void Solve()
        {
            Console.ReadLine(); // n
            int[] a = Console.ReadLine()
                             .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                             .Select(int.Parse)
                             .ToArray();
            Console.WriteLine(CountInv(a));
        }

        private static long CountInv(int[] a)
        {
            int[] buf = new int[a.Length];
            return Merge(a, buf, 0, a.Length);
        }

        private static long Merge(int[] a, int[] buf, int l, int r)
        {
            if (r - l <= 1) return 0;
            int m = (l + r) / 2;
            long ans = Merge(a, buf, l, m) + Merge(a, buf, m, r);
            int i = l, j = m, k = l;
            while (i < m && j < r)
            {
                if (a[i] <= a[j]) buf[k++] = a[i++];
                else
                {
                    buf[k++] = a[j++];
                    ans += m - i;
                }
            }
            while (i < m) buf[k++] = a[i++];
            while (j < r) buf[k++] = a[j++];
            Array.Copy(buf, l, a, l, r - l);
            return ans;
        }
    }
}


