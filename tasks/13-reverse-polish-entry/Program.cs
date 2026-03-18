using System;
using System.Collections.Generic;
using System.Globalization;

namespace Task13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение:");
            string expr = Console.ReadLine()!;

            Console.WriteLine("Введите переменные через пробел (например: a=5 b=10), либо пустую строку:");
            string varsInput = Console.ReadLine()!;

            var vars = ParseVariables(varsInput);

            try
            {
                var rpn = ExpressionEvaluator.ToRpn(expr);
                double result = ExpressionEvaluator.EvalRpn(rpn, vars);
                Console.WriteLine(result.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static Dictionary<string, double> ParseVariables(string s)
        {
            Dictionary<string, double> result = new();
            if (string.IsNullOrWhiteSpace(s)) return result;

            var parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                var kv = p.Split('=');
                if (kv.Length != 2) throw new Exception("Ошибка формата переменных");
                result[kv[0]] = double.Parse(kv[1], CultureInfo.InvariantCulture);
            }
            return result;
        }
    }
}
