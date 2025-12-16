using System;
class Deque
{
    private readonly int[] data;
    private int head; // индекс первого элемента
    private int tail; // индекс позиции после последнего
    public int Count { get; private set; }

    public Deque(int capacity)
    {
        data = new int[capacity];
        head = 0;
        tail = 0;
        Count = 0;
    }

    public void PushBack(int x)
    {
        data[tail] = x;
        tail = (tail + 1) % data.Length;
        Count++;
    }

    public void PushFront(int x)
    {
        head = (head - 1 + data.Length) % data.Length;
        data[head] = x;
        Count++;
    }

    public int PopFront()
    {
        int x = data[head];
        head = (head + 1) % data.Length;
        Count--;
        return x;
    }

    public int PopBack()
    {
        tail = (tail - 1 + data.Length) % data.Length;
        int x = data[tail];
        Count--;
        return x;
    }

    public int Front()
    {
        return data[head];
    }

    public int Back()
    {
        int idx = (tail - 1 + data.Length) % data.Length;
        return data[idx];
    }
}

class Program
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());

        // Максимальное количество гоблинов в очереди не превышает n,
        // поэтому для каждой половины достаточно емкости n.
        Deque left = new Deque(n);   // Первая половина
        Deque right = new Deque(n);  // Вторая половина

        for (int i = 0; i < n; i++)
        {
            string[] input = Console.ReadLine().Split();
            char eventType = input[0][0];

            switch (eventType)
            {
                case '+':
                {
                    int id = int.Parse(input[1]);
                    right.PushBack(id);
                    Balance(left, right);
                    break;
                }

                case '*':
                {
                    int id = int.Parse(input[1]);
                    left.PushBack(id);
                    Balance(left, right);
                    break;
                }

                case '-':
                {
                    int first = left.PopFront();
                    Console.WriteLine(first);
                    Balance(left, right);
                    break;
                }
            }
        }
    }

    static void Balance(Deque left, Deque right)
    {
        // Поддерживаем: left.Count == right.Count или left.Count == right.Count + 1
        while (left.Count > right.Count + 1)
        {
            int val = left.PopBack();
            right.PushFront(val);
        }

        while (left.Count < right.Count)
        {
            int val = right.PopFront();
            left.PushBack(val);
        }
    }
}