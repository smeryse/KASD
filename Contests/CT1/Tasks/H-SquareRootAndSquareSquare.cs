using System;

namespace CT1.Tasks
{
    // Решение уравнения x^2 + sqrt(x) = C
    static class SquareRootAndSquareSquare
    {
        public static void Solve()
        {
            double C = double.Parse(Console.ReadLine());
            double x = SolveEquation(C);
            Console.WriteLine(x.ToString("F7"));
        }

        private static double SolveEquation(double C)
        {
            double l = 0, r = Math.Max(1, C);
            double eps = 1e-7;
            while (r - l > eps)
            {
                double m = (l + r) / 2.0;
                double f = m * m + Math.Sqrt(m);
                if (f < C) l = m;
                else r = m;
            }
            return l;
        }
    }
}


