//using System;

//class Program
//{
//    static void Main()
//    {
//        double C = double.Parse(Console.ReadLine());
//        double x = EquationSolver.Solve(C);
//        Console.WriteLine(x.ToString("F7"));
//    }
//}

//static class EquationSolver
//{
//    public static double Solve(double C)
//    {
//        double left = 0;
//        double right = Math.Max(1, C);
//        double eps = 1e-7;

//        while (right - left > eps)
//        {
//            double mid = (left + right) / 2.0;
//            double f = mid * mid + Math.Sqrt(mid);

//            if (f < C)
//                left = mid;
//            else
//                right = mid;
//        }

//        return left;
//    }
//}
