using System;
using System.Collections.Generic;
using System.Globalization;
using Task12.Collection;

namespace Task13
{
    public static class ExpressionEvaluator
    {
        private static readonly Dictionary<string, int> Precedence = new()
        {
            ["+"] = 1, ["-"] = 1,
            ["*"] = 2, ["/"] = 2, ["%"] = 2, ["div"] = 2,
            ["^"] = 3,
            ["min"] = 1, ["max"] = 1,
            ["sin"] = 4, ["cos"] = 4, ["tan"] = 4,
            ["sqrt"] = 4, ["abs"] = 4, ["sign"] = 4,
            ["ln"] = 4, ["log"] = 4, ["exp"] = 4, ["trunc"] = 4
        };

        private static bool IsFunction(string t) =>
            t is "sin" or "cos" or "tan" or "sqrt" or "abs" or "sign"
            or "ln" or "log" or "exp" or "trunc" or "min" or "max";

        private static bool IsOperator(string s) => Precedence.ContainsKey(s);

        private static bool IsVariable(string s) =>
            s.Length == 1 && char.IsLetter(s[0]);

        public static List<string> ToRpn(string input)
        {
            List<string> output = new();
            MyStack<string> ops = new();

            foreach (var token in Tokenize(input))
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _)
                    || IsVariable(token))
                {
                    output.Add(token);
                }
                else if (IsFunction(token))
                {
                    ops.Push(token);
                }
                else if (IsOperator(token))
                {
                    while (!ops.Empty() && IsOperator(ops.Peek()) &&
                           Precedence[ops.Peek()] >= Precedence[token])
                    {
                        output.Add(ops.Pop());
                    }
                    ops.Push(token);
                }
                else if (token == "(")
                {
                    ops.Push(token);
                }
                else if (token == ")")
                {
                    while (!ops.Empty() && ops.Peek() != "(")
                        output.Add(ops.Pop());
                    if (ops.Empty()) throw new Exception("Несбалансированные скобки");
                    ops.Pop();
                    if (!ops.Empty() && IsFunction(ops.Peek()))
                        output.Add(ops.Pop());
                }
                else
                {
                    throw new Exception($"Неизвестный токен: {token}");
                }
            }

            while (!ops.Empty())
            {
                var t = ops.Pop();
                if (t == "(") throw new Exception("Несбалансированные скобки");
                output.Add(t);
            }

            return output;
        }

        public static double EvalRpn(List<string> rpn, Dictionary<string, double> vars)
        {
            MyStack<double> st = new();

            foreach (var token in rpn)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                {
                    st.Push(val);
                }
                else if (IsVariable(token))
                {
                    if (!vars.ContainsKey(token)) throw new Exception($"Переменная '{token}' не задана");
                    st.Push(vars[token]);
                }
                else if (IsFunction(token))
                {
                    if (token == "min" || token == "max")
                    {
                        double b = st.Pop();
                        double a = st.Pop();
                        st.Push(token == "min" ? Math.Min(a, b) : Math.Max(a, b));
                    }
                    else
                    {
                        double a = st.Pop();
                        st.Push(token switch
                        {
                            "sin" => Math.Sin(a),
                            "cos" => Math.Cos(a),
                            "tan" => Math.Tan(a),
                            "abs" => Math.Abs(a),
                            "sign" => Math.Sign(a),
                            "sqrt" => Math.Sqrt(a),
                            "ln" => Math.Log(a),
                            "log" => Math.Log10(a),
                            "exp" => Math.Exp(a),
                            "trunc" => Math.Truncate(a),
                            _ => throw new Exception($"Неизвестная функция '{token}'")
                        });
                    }
                }
                else
                {
                    double b = st.Pop();
                    double a = st.Pop();
                    st.Push(token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => b == 0 ? throw new Exception("Деление на ноль") : a / b,
                        "%" => a % b,
                        "div" => Math.Truncate(a / b),
                        "^" => Math.Pow(a, b),
                        _ => throw new Exception($"Неизвестная операция '{token}'")
                    });
                }
            }

            if (st.Size() != 1) throw new Exception("Ошибка вычисления");
            return st.Pop();
        }

        private static IEnumerable<string> Tokenize(string s)
        {
            int i = 0;
            while (i < s.Length)
            {
                if (char.IsWhiteSpace(s[i])) 
                {
                    i++;
                    continue;
                }

                if (char.IsDigit(s[i]))
                {
                    int start = i;
                    while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '.')) i++;
                    yield return s.Substring(start, i - start);
                    continue;
                }

                if (char.IsLetter(s[i]))
                {
                    int start = i;
                    while (i < s.Length && char.IsLetter(s[i])) i++;
                    yield return s.Substring(start, i - start);
                    continue;
                }

                if ("+-*/%^()".Contains(s[i]))
                {
                    yield return s[i++].ToString();
                    continue;
                }

                throw new Exception($"Неизвестный символ '{s[i]}'");
            }
        }
    }
}
