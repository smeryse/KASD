using System;
using System.Linq;
using Task5.Collections;


class Program
{
    static MaxHeap<int> heap1;
    static MaxHeap<int> heap2;
    static MaxHeap<int> heap3;

    static void Main(string[] args)
    {
        T1();
        Console.WriteLine();
        T2();
        Console.WriteLine();
        T3();
        Console.WriteLine();
        T4();
        Console.WriteLine();
        Console.WriteLine("Тест 5 - добавление нового элемента в кучу");
        int[] array = { 1, 2, 3, 4, 7, 3, 7 };
        heap1 = new MaxHeap<int>(array);
        heap1.Print();
        heap1.Insert(9);
        heap1.Print();
        T6();
        Console.WriteLine();
    }

    static void T1()
    {
        Console.WriteLine("Тест 1 - построение кучи из массива конструктором");
        int[] array1 = { 1, 2, 3, 5, 67, 23 };
        heap1 = new MaxHeap<int>(array1);
        heap1.Print();
    }

    static void T2()
    {
        Console.WriteLine("Тест 2 - возвращение максимума без удаления");
        Console.WriteLine(heap1.Peek());
        heap1.Print();
    }

    static void T3()
    {
        Console.WriteLine("Тест 3 - возвращение максимума с удалением");
        Console.WriteLine(heap1.ExtractMax());
        heap1.Print();
    }

    static void T4()
    {
        Console.WriteLine("Тест 4 - увеличение ключа и восстановление свойства кучи");
        heap1.IncreaseKey(3, 20);
        heap1.Print();
    }

    static void T6()
    {
        Console.WriteLine("Тест 6 - слияние двух куч в новую");
        int[] array1 = { 1, 2, 3, 5, 67, 23 };
        heap1 = new MaxHeap<int>(array1);

        int[] array2 = {56, 2, 41, 19, 5 };
        heap2 = new MaxHeap<int>(array2);

        int[] array3 = array1.Concat(array2).ToArray();
        heap3 = new MaxHeap<int>(array3);

        heap1.Print();
        heap2.Print();
        heap3.Print();
    }
}
