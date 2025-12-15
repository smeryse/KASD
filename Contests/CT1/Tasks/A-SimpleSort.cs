using System;
using System.Linq;

namespace CT1.Tasks
{
    // Подсчёт инверсий через сортировку слиянием
    static class SimpleSort
    {
        public static void Solve()
        {
            Console.ReadLine(); // n не обязательно использовать
            int[] arr = Console.ReadLine()
                               .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                               .Select(int.Parse)
                               .ToArray();

            long inversions = CountInversions(arr);
            Console.WriteLine(inversions);
        }

        private static long CountInversions(int[] a)
        {
            int[] buf = new int[a.Length];
            return MergeSort(a, buf, 0, a.Length);
        }

        private static long MergeSort(int[] a, int[] buf, int l, int r)
        {
            if (r - l <= 1) return 0;
            int m = (l + r) / 2;
            long inv = MergeSort(a, buf, l, m) + MergeSort(a, buf, m, r);

            int i = l, j = m, k = l;
            while (i < m && j < r)
            {
                if (a[i] <= a[j]) buf[k++] = a[i++];
                else
                {
                    buf[k++] = a[j++];
                    inv += m - i; // все оставшиеся слева больше a[j]
                }
            }
            while (i < m) buf[k++] = a[i++];
            while (j < r) buf[k++] = a[j++];
            Array.Copy(buf, l, a, l, r - l);
            return inv;
        }
    }
}


