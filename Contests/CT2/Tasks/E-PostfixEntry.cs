using System;
using System.Collections.Generic;

namespace CT2.Tasks
{
    static class PostfixEntry
    {
        public static void Solve()
        {
            string line = Console.ReadLine();

            string[] tokens = line.Split();
            Stack<long> stack = new Stack<long>();

            foreach (var token in tokens)
            {
                if (long.TryParse(token, out long value))
                {
                    stack.Push(value);
                }
                else
                {
                    long b = stack.Pop();
                    long a = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;
                        case "-":
                            stack.Push(a - b);
                            break;
                        case "*":
                            stack.Push(a * b);
                            break;
                        default:
                            throw new InvalidOperationException($"Неизвестный оператор '{token}'");
                    }
                }
            }

            Console.WriteLine(stack.Pop());
        }
    }
}
