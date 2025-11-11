//using System;
//using System.Linq;

//class Program
//{
//    static int n;
//    static int k;

//    static long[] v;
//    static long[] w;
//    static void Main()
//    {
//        string[] first = Console.ReadLine().Split();
//        n = int.Parse(first[0]);
//        k = int.Parse(first[1]);

//        v = new long[n];
//        w = new long[n];

//        for (int i = 0; i < n; i++)
//        {
//            string[] parts = Console.ReadLine().Split();
//            v[i] = long.Parse(parts[0]);
//            w[i] = long.Parse(parts[1]);
//        }

//        int[] answer = new int[k];
//        double left = 0;
//        double right = 1e6;

//        double eps = 1e-9;

//        while (right - left > eps)
//        {
//            double mid = (left + right) / 2;
//            if (IsPossible(mid, out int[] chosen))
//            {
//                left = mid;
//                answer = chosen;
//            }
//            else
//            {
//                right = mid;
//            }
//        }

//        for (int i = 0; i < k; i++)
//            Console.WriteLine(answer[i] + 1);
//    }

//    static bool IsPossible(double X, out int[] chosen)
//    {
//        double[] scores = new double[n];
//        for (int i = 0; i < n; i++)
//            scores[i] = v[i] - X * w[i];

//        int[] idx = new int[n];
//        for (int i = 0; i < n; i++)
//            idx[i] = i;

//        Array.Sort(idx, (i, j) => scores[j].CompareTo(scores[i]));

//        double sum = 0;
//        for (int i = 0; i < k; i++)
//            sum += scores[idx[i]];

//        chosen = new int[k];
//        for (int i = 0; i < k; i++)
//            chosen[i] = idx[i];

//        return sum >= 0;
//    }
//}
