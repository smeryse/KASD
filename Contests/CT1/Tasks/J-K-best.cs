using System;

namespace CT1.Tasks
{
    // Выбор k предметов с максимальным средним v/w через бинарный поиск по ответу
    static class KBest
    {
        private static int n, k;
        private static long[] v;
        private static long[] w;

        public static void Solve()
        {
            var first = Console.ReadLine().Split();
            n = int.Parse(first[0]);
            k = int.Parse(first[1]);

            v = new long[n];
            w = new long[n];
            for (int i = 0; i < n; i++)
            {
                var parts = Console.ReadLine().Split();
                v[i] = long.Parse(parts[0]);
                w[i] = long.Parse(parts[1]);
            }

            double l = 0, r = 1e6;
            double eps = 1e-9;
            int[] answer = new int[k];

            while (r - l > eps)
            {
                double mid = (l + r) / 2;
                if (IsPossible(mid, out int[] chosen))
                {
                    l = mid;
                    answer = chosen;
                }
                else
                {
                    r = mid;
                }
            }

            for (int i = 0; i < k; i++)
                Console.WriteLine(answer[i] + 1); // индексация с 1
        }

        private static bool IsPossible(double x, out int[] chosen)
        {
            double[] score = new double[n];
            int[] idx = new int[n];
            for (int i = 0; i < n; i++)
            {
                score[i] = v[i] - x * w[i];
                idx[i] = i;
            }

            Array.Sort(idx, (i, j) => score[j].CompareTo(score[i]));

            double sum = 0;
            chosen = new int[k];
            for (int i = 0; i < k; i++)
            {
                sum += score[idx[i]];
                chosen[i] = idx[i];
            }

            return sum >= 0;
        }
    }
}


