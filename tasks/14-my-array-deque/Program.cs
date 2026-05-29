namespace Task14
{
    class Program
    {
        static void Main(string[] args)
        {
            MyArrayDeque<int> deque = new MyArrayDeque<int>();
            deque.Add(1);
            deque.Add(2);
            deque.Add(3);
            Console.WriteLine("Size: " + deque.Size());
            Console.WriteLine("Contains 2: " + deque.Contains(2));
            Console.WriteLine("Contains 5: " + deque.Contains(5));
            deque.Remove(2);
            Console.WriteLine("Size after remove: " + deque.Size());
            Console.WriteLine("Contains 2 after remove: " + deque.Contains(2));
            deque.Clear();
            Console.WriteLine("Size after clear: " + deque.Size());

            MyArrayDeque<int> deque2 = new MyArrayDeque<int>(new int[] { 10, 20, 30, 40, 50 });
            Console.WriteLine("Deque2 size: " + deque2.Size());
            Console.WriteLine("Contains all 10,20,30: " + deque2.ContainsAll(new int[] { 10, 20, 30 }));
            Console.WriteLine("Is empty: " + deque2.IsEmpty());
        }
    }
}