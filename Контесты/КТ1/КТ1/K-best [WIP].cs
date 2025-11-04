using System;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] first = Console.ReadLine().Split();
        int n = int.Parse(first[0]);
        int k = int.Parse(first[1]);

        long[] v = new long[n];
        long[] w = new long[n];

        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine().Split();
            v[i] = long.Parse(parts[0]);
            w[i] = long.Parse(parts[1]);
        }

        int[] answer = new int[k];
        double left = 0;
        double right = 1e6;

        // Бинарный поиск по среднему значению
        for (int iter = 0; iter < 100; iter++)
        {
            double mid = (left + right) / 2;
            if (IsPossible(v, w, n, k, mid, out int[] chosen))
            {
                left = mid;
                answer = chosen;
            }
            else
            {
                right = mid;
            }
        }

        // Выводим индексы в 1-базе
        for (int i = 0; i < k; i++)
            Console.Write((answer[i] + 1) + " ");
    }

    // Проверка, достижимо ли отношение X
    static bool IsPossible(long[] v, long[] w, int n, int k, double X, out int[] chosen)
    {
        double[] scores = new double[n];
        for (int i = 0; i < n; i++)
            scores[i] = v[i] - X * w[i];

        int[] idx = Enumerable.Range(0, n).ToArray();
        Array.Sort(idx, (i, j) => scores[j].CompareTo(scores[i]));

        double sum = 0;
        for (int i = 0; i < k; i++)
            sum += scores[idx[i]];

        chosen = new int[k];
        Array.Copy(idx, chosen, k);

        return sum >= 0;
    }
}
