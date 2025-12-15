using System;
using System.Globalization;

namespace CT1.Tasks
{
    // Троичный поиск оптимальной точки
    static class GladeOfFirewood
    {
        public static void Solve()
        {
            var v = Console.ReadLine().Split(' ');
            double Vp = double.Parse(v[0], CultureInfo.InvariantCulture);
            double Vf = double.Parse(v[1], CultureInfo.InvariantCulture);
            double a = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            double res = FindPoint(Vp, Vf, a);
            Console.WriteLine(res.ToString("F7", CultureInfo.InvariantCulture));
        }

        private static double FindPoint(double Vp, double Vf, double a)
        {
            double l = 0, r = 1;
            double eps = 1e-8;
            while (r - l > eps)
            {
                double m1 = l + (r - l) / 3;
                double m2 = r - (r - l) / 3;

                double t1 = Time(m1, a, Vp, Vf);
                double t2 = Time(m2, a, Vp, Vf);

                if (t1 > t2) l = m1;
                else r = m2;
            }
            return (l + r) / 2;
        }

        private static double Time(double x, double a, double Vp, double Vf)
        {
            double toBorder = Math.Sqrt(x * x + (1 - a) * (1 - a));
            double fromBorder = Math.Sqrt((1 - x) * (1 - x) + a * a);
            return toBorder / Vp + fromBorder / Vf;
        }
    }
}


