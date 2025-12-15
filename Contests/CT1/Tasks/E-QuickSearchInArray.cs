using System;

namespace CT1.Tasks
{
    // Кол-во элементов в диапазоне [l, r]
    static class QuickSearchInArray
    {
        public static void Solve()
        {
            int n = int.Parse(Console.ReadLine());
            int[] a = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
            Array.Sort(a);

            int q = int.Parse(Console.ReadLine());
            for (int i = 0; i < q; i++)
            {
                var parts = Console.ReadLine().Split();
                int l = int.Parse(parts[0]);
                int r = int.Parse(parts[1]);

                int left = LowerBound(a, l);
                int right = LowerBound(a, r + 1);
                Console.WriteLine(right - left);
            }
        }

        private static int LowerBound(int[] arr, int x)
        {
            int l = 0, r = arr.Length;
            while (l < r)
            {
                int m = (l + r) / 2;
                if (arr[m] >= x) r = m;
                else l = m + 1;
            }
            return l;
        }
    }
}


