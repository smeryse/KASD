using System;
using System.Collections.Generic;

class MinStack : Stack<int>
{
    private Stack<int> minStack = new Stack<int>();

    public new void Push(int item)
    {
        base.Push(item);

        if (minStack.Count == 0 || item <= minStack.Peek())
            minStack.Push(item);
    }

    public int Pop()
    {
        int item = base.Pop();

        if (item == minStack.Peek())
            minStack.Pop();

        return item;
    }

    public int Peek()
    {
        return stack.Peek();
    }

    public int GetMin()
    {
        return minStack.Peek();
    }
}

class Program
{
    static void Main()
    {
        MinStack s = new MinStack();
        s.Push(5);
        s.Push(3);
        s.Push(7);
        s.Push(2);

        Console.WriteLine(s.GetMin()); // 2
        s.Pop();
        Console.WriteLine(s.GetMin()); // 3
        Console.WriteLine(s.Peek());   // 7
    }
}
