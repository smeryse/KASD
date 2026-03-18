using System;
using System.Collections.Generic;

namespace CT2.Tasks
{
    class MinStack : Stack<int>
    {
        private Stack<int> minStack = new Stack<int>();

        public new void Push(int item)
        {
            base.Push(item);

            if (minStack.Count == 0 || item <= minStack.Peek())
                minStack.Push(item);
        }

        public new int Pop()
        {
            int item = base.Pop();

            if (item == minStack.Peek())
                minStack.Pop();

            return item;
        }

        public new int Peek()
        {
            return base.Peek();
        }

        public int GetMin()
        {
            return minStack.Peek();
        }
    }

    static class MinOnStack
    {
        public static void Solve()
        {
            MinStack minStack = new MinStack();
            var operations = new Dictionary<int, Action<int?>>()
            {
                { 1, x => minStack.Push(x.Value) },
                { 2, _ => minStack.Pop() },
                { 3, _ => Console.WriteLine(minStack.GetMin()) }
            };

            int n = int.Parse(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                var parts = Console.ReadLine().Split();
                int t = int.Parse(parts[0]);
                int? x = parts.Length > 1 ? int.Parse(parts[1]) : (int?)null;

                operations[t](x);
            }
        }
    }
}
