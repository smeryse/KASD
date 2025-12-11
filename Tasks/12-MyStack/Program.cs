using System;
using Task12.Collection;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ТЕСТ КЛАССА MyStack<T> ===\n");

        MyStack<int> stack = new MyStack<int>();

        // 1. Проверяем, что стек пуст
        Console.WriteLine("1. Проверка Empty() на пустом стеке:");
        Console.WriteLine("Стек пуст? " + stack.Empty());
        Console.WriteLine();

        // 2. Push
        Console.WriteLine("2. Добавляем элементы (Push): 10, 20, 30, 20");
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        stack.Push(20);
        Console.Write("Текущее содержимое стека: ");
        stack.Print();
        Console.WriteLine();

        // 3. Peek
        Console.WriteLine("3. Проверяем Peek():");
        Console.WriteLine("Верхний элемент: " + stack.Peek());
        Console.Write("Содержимое после Peek(): ");
        stack.Print();
        Console.WriteLine();

        // 4. Pop
        Console.WriteLine("4. Снимаем элементы методом Pop():");
        Console.WriteLine("Pop() -> " + stack.Pop());
        Console.WriteLine("Pop() -> " + stack.Pop());
        Console.Write("Стек после двух Pop(): ");
        stack.Print();
        Console.WriteLine();

        // 5. Поиск элемента Search
        Console.WriteLine("5. Тестируем Search():");
        stack.Push(50);
        stack.Push(60);
        Console.Write("Стек для теста Search(): ");
        stack.Print();
        Console.WriteLine("Search(50) = " + stack.Search(50) + " (позиция от вершины)");
        Console.WriteLine("Search(10) = " + stack.Search(10) + " (позиция от вершины)");
        Console.WriteLine("Search(999) = " + stack.Search(999));
        Console.WriteLine();

        // 6. Проверка Pop и Peek на пустом стеке (исключения)
        Console.WriteLine("6. Тест исключений Pop/Peek на пустом стеке:");
        try
        {
            Console.WriteLine("Очищаем стек...");
            while (!stack.Empty())
                stack.Pop();

            Console.WriteLine("Выполняем Pop() на пустом стеке:");
            stack.Pop(); // должно быть исключение
        }
        catch (Exception ex)
        {
            Console.WriteLine("Поймано исключение (Pop): " + ex.Message);
        }

        try
        {
            Console.WriteLine("Выполняем Peek() на пустом стеке:");
            stack.Peek(); // должно быть исключение
        }
        catch (Exception ex)
        {
            Console.WriteLine("Поймано исключение (Peek): " + ex.Message);
        }

        Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===");
    }
}