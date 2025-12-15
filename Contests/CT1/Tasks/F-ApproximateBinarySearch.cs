using System;

namespace CT1.Tasks
{
    // Для каждого запроса q выводим ближайший элемент массива
    static class ApproximateBinarySearch
    {
        public static void Solve()
        {
            var first = Console.ReadLine().Split(' ');
            int n = int.Parse(first[0]);
            int k = int.Parse(first[1]);

            int[] a = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int[] queries = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            for (int i = 0; i < k; i++)
            {
                int q = queries[i];
                int idx = LowerBound(a, q);

                int cand1 = idx < n ? a[idx] : int.MaxValue;
                int cand2 = idx > 0 ? a[idx - 1] : int.MinValue;

                int result;
                if (cand1 == int.MaxValue) result = cand2;
                else if (cand2 == int.MinValue) result = cand1;
                else
                {
                    int diff1 = Math.Abs(cand1 - q);
                    int diff2 = Math.Abs(cand2 - q);
                    if (diff1 < diff2) result = cand1;
                    else if (diff2 < diff1) result = cand2;
                    else result = Math.Min(cand1, cand2);
                }
                Console.WriteLine(result);
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


