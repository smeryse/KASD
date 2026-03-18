using System;
using System.Collections.Generic;

namespace CT2.Tasks
{
    static class Balloons
    {
        public static void Solve()
        {
            var input = Console.ReadLine().Split();
            int n = int.Parse(input[0]);
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
                a[i] = int.Parse(input[i + 1]);

            var stack = new Stack<(int color, int count)>();
            int totalRemoved = 0;

            for (int i = 0; i < n; )
            {
                int j = i;
                while (j < n && a[j] == a[i]) j++;
                int count = j - i;
                var color = a[i];

                if (stack.Count > 0 && stack.Peek().color == color)
                {
                    var top = stack.Pop();
                    count += top.count;
                }

                if (count >= 3)
                {
                    totalRemoved += count;
                }
                else
                {
                    stack.Push((color, count));
                }

                i = j;
            }

            Console.WriteLine(totalRemoved);
        }
    }
}
