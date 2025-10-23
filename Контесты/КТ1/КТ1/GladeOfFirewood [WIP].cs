//using System;
//using System.Globalization;

// ДОписать более норм оформление
//class Program
//{
//    static void Main()
//    {
//        string[] V = Console.ReadLine().Split(' ');
//        double Vp = double.Parse(V[0]);
//        double Vf = double.Parse(V[1]);
//        double a;
//        double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out a);

//        double result = Solution(Vp, Vf, a);
//        Console.WriteLine(result.ToString("F7", System.Globalization.CultureInfo.InvariantCulture));
//    }

//    static double CalcTime(double x, double a, double Vp, double Vf)
//    {
//        double distToBorder = Math.Sqrt(x * x + (1 - a) * (1 - a));
//        double distFromBorder = Math.Sqrt((1 - x) * (1 - x) + a * a);
//        return distToBorder / Vp + distFromBorder / Vf;
//    }

//    static double Solution(double Vp, double Vf, double a)
//    {
//        double left = 0;
//        double right = 1;
//        double eps = 1e-8;

//        while (right - left > eps)
//        {
//            double m1 = left + (right - left) / 3;
//            double m2 = right - (right - left) / 3;

//            double time1 = CalcTime(m1, a, Vp, Vf);
//            double time2 = CalcTime(m2, a, Vp, Vf);

//            if (time1 > time2)
//                left = m1;
//            else
//                right = m2;
//        }

//        return (left + right) / 2;
//    }
//}