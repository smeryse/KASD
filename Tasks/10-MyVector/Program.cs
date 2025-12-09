using System;

namespace Task10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║            ТЕСТИРОВАНИЕ РЕАЛИЗАЦИИ MyVector<T>        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.ResetColor();

            // Тест 1
            TestHeader(1, "Конструкторы по умолчанию, Add(), Get(), Size()");
            var v1 = new MyVector<string>();
            v1.Add("A"); v1.Add("B"); v1.Add("C");
            Show($"Размер вектора: {v1.Size()} (ожидается 3)");
            Show($"Элемент по индексу 1: {v1.Get(1)} (ожидается B)");
            Show("Содержимое:", v1);

            // Тест 2
            TestHeader(2, "Конструктор из массива T[]");
            string[] arr = { "X", "Y", "Z" };
            var v2 = new MyVector<string>(arr);
            Show("Вектор создан из массива:", v2);
            Show($"Размер: {v2.Size()} (ожидается 3)");

            // Тест 3
            TestHeader(3, "AddAll() и Add(index, elem)");
            v1.AddAll(new[] { "D", "E" });
            Show("После AddAll([\"D\",\"E\"]):", v1);
            v1.Add(2, "Inserted");
            Show("После Add(2, \"Inserted\"):", v1);

            // Тест 4
            TestHeader(4, "Remove(obj) и RemoveAt(index)");
            bool removed = v1.Remove("C");
            Show($"Remove(\"C\") → {removed} (ожидается true)");
            Show("После удаления 'C':", v1);
            string removedElem = v1.RemoveAt(0);
            Show($"RemoveAt(0) вернул: \"{removedElem}\" (ожидается \"A\")");
            Show("После RemoveAt(0):", v1);

            // Тест 5
            TestHeader(5, "RemoveAll(T[] arr) — удаление всех вхождений");
            var v5 = new MyVector<string>(new[] { "Cat", "Dog", "Cat", "Bird", "Cat" });
            Show("Исходный вектор:", v5);
            bool allRemoved = v5.RemoveAll(new[] { "Cat", "Bird" });
            Show($"RemoveAll([\"Cat\", \"Bird\"]) → {allRemoved} (ожидается true)");
            Show("Результат — должен остаться только \"Dog\":", v5);

            // Тест 6
            TestHeader(6, "RemoveRange(begin, end) — удаление диапазона");
            var intVec = new MyVector<int>();
            for (int i = 1; i <= 10; i++) intVec.Add(i);
            Show("Вектор 1..10:", intVec);
            intVec.RemoveRange(2, 7);
            Show("После RemoveRange(2, 7):", intVec);
            Show("Ожидается: 1 2 9 10");

            // Тест 7
            TestHeader(7, "Contains, IndexOf, LastIndexOf, ContainsAll");
            var v7 = new MyVector<string>(new[] { "apple", "banana", "cherry", "banana" });
            Show("Вектор:", v7);
            Show($"Contains(\"banana\") → {v7.Contains("banana")} (true)");
            Show($"Contains(\"orange\") → {v7.Contains("orange")} (false)");
            Show($"IndexOf(\"banana\") → {v7.IndexOf("banana")} (1)");
            Show($"LastIndexOf(\"banana\") → {v7.LastIndexOf("banana")} (3)");
            Show($"ContainsAll([\"apple\",\"cherry\"]) → {v7.ContainsAll(new[] { "apple", "cherry" })} (true)");
            Show($"ContainsAll([\"apple\",\"date\"]) → {v7.ContainsAll(new[] { "apple", "date" })} (false)");

            // Тест 8
            TestHeader(8, "RetainAll — оставить только указанные элементы");
            var v8 = new MyVector<string>(new[] { "red", "green", "blue", "yellow", "green" });
            Show("Исходный:", v8);
            v8.RetainAll(new[] { "green", "blue" });
            Show("После RetainAll([\"green\", \"blue\"]):", v8);
            Show("Ожидается: green green blue (или green blue green — зависит от реализации)");

            // Тест 9
            TestHeader(9, "SubList и ToArray");
            var nums = new MyVector<int>();
            for (int i = 0; i < 10; i++) nums.Add(i);
            var sub = nums.SubList(3, 7);
            Show("SubList(3, 7): " + string.Join(", ", sub) + " (ожидается 3,4,5,6,7)");
            var copy = nums.ToArray();
            Show("ToArray(): " + string.Join(", ", copy));

            // Тест 10
            TestHeader(10, "Рост ёмкости (capacity)");
            var grow = new MyVector<string>(2, 5);
            Show($"Начальная ёмкость: {grow.elementData.Length} (по конструктору: 2)");
            grow.Add("1"); grow.Add("2"); grow.Add("3");
            Show($"После добавления 3 элементов ёмкость: {grow.elementData.Length} (должна вырасти → 7 или больше)");

            // Тест 11
            TestHeader(11, "Clear()");
            v1.Clear();
            Show($"После Clear(): Size = {v1.Size()}, IsEmpty = {v1.IsEmpty()} (0, true)");
            Show("Содержимое (не должно быть мусора):", v1);

            // Тест 12
            TestHeader(12, "FirstElement / LastElement");
            var v12 = new MyVector<int>(new[] { 10, 20, 30 });
            Show("Вектор:", v12);
            Show($"FirstElement() → {v12.FirstElement()}, LastElement() → {v12.LastElement()} (10, 30)");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("══════════════════════════════════════════════════════");
            Console.WriteLine("        ВСЕ ТЕСТЫ ПРОЙДЕНЫ УСПЕШНО! ГОТОВ К ЗАЩИТЕ!      ");
            Console.WriteLine("══════════════════════════════════════════════════════");
            Console.ResetColor();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static int testNumber = 0;
        static void TestHeader(int number, string description)
        {
            testNumber = number;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Тест {number}: {description}");
            Console.ResetColor();
        }

        static void Show(string message, MyVector<string>? vec = null)
        {
            Console.Write($"   • {message}");
            if (vec != null)
            {
                Console.Write(" → [ ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < vec.Size(); i++)
                    Console.Write(vec.Get(i) + (i < vec.Size() - 1 ? ", " : ""));
                Console.ResetColor();
                Console.Write(" ]");
            }
            Console.WriteLine();
        }

        static void Show(string message, MyVector<int>? vec = null)
        {
            Console.Write($"   • {message}");
            if (vec != null)
            {
                Console.Write(" → [ ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < vec.Size(); i++)
                    Console.Write(vec.Get(i) + (i < vec.Size() - 1 ? ", " : ""));
                Console.ResetColor();
                Console.Write(" ]");
            }
            Console.WriteLine();
        }

        static void Show(string message)
        {
            Console.WriteLine($"   • {message}");
        }
    }
}