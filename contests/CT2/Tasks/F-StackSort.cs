using System;
using System.Collections.Generic;

namespace CT2.Tasks
{
    static class StackSort
    {
        public static void Solve()
        {
            int n = int.Parse(Console.ReadLine());
            var parts = Console.ReadLine().Split();

            if (parts.Length != n)
            {
                Console.WriteLine("impossible");
                return;
            }

            var stackA = new Stack<int>();
            for (int i = n - 1; i >= 0; i--)
                stackA.Push(int.Parse(parts[i]));

            var stackB = new Stack<int>();
            var ops = new List<string>();

            int need = 1;

            // push: A.Pop() -> B.Push()
            // pop : B.Pop() -> output
            while (stackA.Count > 0)
            {
                stackB.Push(stackA.Pop());
                ops.Add("push");

                while (stackB.Count > 0 && stackB.Peek() == need)
                {
                    stackB.Pop();
                    ops.Add("pop");
                    need++;
                }
            }

            while (stackB.Count > 0 && stackB.Peek() == need)
            {
                stackB.Pop();
                ops.Add("pop");
                need++;
            }

            if (need == n + 1)
                Console.WriteLine(string.Join("\n", ops));
            else
                Console.WriteLine("impossible");
        }
    }
}
