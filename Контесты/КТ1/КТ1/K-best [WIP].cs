//using System;
//using System.Collections.Generic;
//using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        var args = Console.ReadLine().Split(' ');
//        int n = int.Parse(args[0]);
//        int k = int.Parse(args[1]);

//        var gems = new List<(int value, int weight, int index)>();

//        for (int i = 0; i < n; i++)
//        {
//            var parts = Console.ReadLine().Split(' ');
//            int v = int.Parse(parts[0]);
//            int w = int.Parse(parts[1]);
//            gems.Add((v, w, i + 1));
//        }

//        double left = 0;
//        double right = gems.Max(g => (double)g.value / g.weight);
//        double eps = 1e-7;

//        List<int> answerIndices = null;

//        while (right - left > eps)
//        {
//            double mid = (left + right) / 2;

//            var b = gems.Select(g => (g.value - mid * g.weight, g.index)).ToList();

//            b.Sort((a, c) => c.Item1.CompareTo(a.Item1));
//            double sum = 0;
//            for (int i = 0; i < k; i++) sum += b[i].Item1;

//            if (sum >= 0)
//            {
//                left = mid;
//                answerIndices = b.Take(k).Select(x => x.Item2).ToList();
//            }
//            else
//            {
//                right = mid;
//            }
//        }

//        foreach (var idx in answerIndices)
//            Console.WriteLine(idx);
//    }
//}
