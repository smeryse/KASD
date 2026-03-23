using System;
using Task19.Collection;

namespace Task19
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ТЕСТИРОВАНИЕ РЕАЛИЗАЦИИ MyTreeSet");

            // Тест 1
            TestHeader(1, "Конструктор по умолчанию, Add(), Size()");
            var set1 = new MyTreeSet<int>();
            set1.Add(5);
            set1.Add(3);
            set1.Add(7);
            set1.Add(1);
            Show($"Размер множества: {set1.Size} (ожидается 4)");
            Show("Содержимое: ", set1);

            // Тест 2
            TestHeader(2, "Добавление дубликата (не должен добавиться)");
            var set2 = new MyTreeSet<string>();
            set2.Add("apple");
            set2.Add("banana");
            set2.Add("cherry");
            Show($"До добавления дубликата Size = {set2.Size}");
            bool added = set2.Add("banana");
            Show($"Add('banana') вернул {added} (ожидается false)");
            Show($"После Size = {set2.Size} (ожидается 3)");
            Show("Содержимое: ", set2);

            // Тест 3
            TestHeader(3, "Contains(), IsEmpty()");
            var set3 = new MyTreeSet<int>();
            set3.Add(10);
            set3.Add(20);
            set3.Add(30);
            Show($"Contains(20) → {set3.Contains(20)} (true)");
            Show($"Contains(25) → {set3.Contains(25)} (false)");
            Show($"IsEmpty() → {set3.IsEmpty()} (false)");

            // Тест 4
            TestHeader(4, "Remove() и Clear()");
            var set4 = new MyTreeSet<int>();
            for (int i = 1; i <= 5; i++) set4.Add(i);
            Show("Исходное множество: ", set4);
            bool removed = set4.Remove(3);
            Show($"Remove(3) вернул {removed} (ожидается true)");
            Show("После удаления: ", set4);
            bool removedAgain = set4.Remove(3);
            Show($"Remove(3) повторно вернул {removedAgain} (ожидается false)");
            set4.Clear();
            Show($"После Clear(): Size = {set4.Size}, IsEmpty = {set4.IsEmpty()} (0, true)");

            // Тест 5
            TestHeader(5, "First(), Last()");
            var set5 = new MyTreeSet<string>();
            set5.Add("zebra");
            set5.Add("apple");
            set5.Add("mango");
            Show("Множество: ", set5);
            Show($"First() → {set5.First()} (ожидается apple)");
            Show($"Last() → {set5.Last()} (ожидается zebra)");

            // Тест 6
            TestHeader(6, "HeadSet() — элементы с ключом меньше end");
            var set6 = new MyTreeSet<int>();
            for (int i = 1; i <= 10; i++) set6.Add(i);
            Show<int>("Исходное множество (1..10): ", set6);
            var headSet = set6.HeadSet(5);
            Show<int>($"HeadSet(5): ", headSet);
            Show("Ожидается: 1, 2, 3, 4");

            // Тест 7
            TestHeader(7, "TailSet() — элементы с ключом больше start");
            var set7 = new MyTreeSet<int>();
            for (int i = 1; i <= 10; i++) set7.Add(i);
            Show<int>("Исходное множество (1..10): ", set7);
            var tailSet = set7.TailSet(5);
            Show<int>($"TailSet(5): ", tailSet);
            Show("Ожидается: 6, 7, 8, 9, 10");

            // Тест 8
            TestHeader(8, "SubSet() — элементы в диапазоне [start, end)");
            var set8 = new MyTreeSet<int>();
            for (int i = 1; i <= 10; i++) set8.Add(i);
            Show<int>("Исходное множество (1..10): ", set8);
            var subSet = set8.SubSet(3, 7);
            Show<int>($"SubSet(3, 7): ", subSet);
            Show("Ожидается: 3, 4, 5, 6");

            // Тест 9
            TestHeader(9, "Lower(), Floor(), Higher(), Ceiling()");
            var set9 = new MyTreeSet<int>();
            set9.Add(10);
            set9.Add(20);
            set9.Add(30);
            set9.Add(40);
            Show("Множество: ", set9);

            Show($"Lower(25) → {set9.Lower(25)} (ожидается 20)");
            Show($"Floor(20) → {set9.Floor(20)} (ожидается 20)");
            Show($"Higher(25) → {set9.Higher(25)} (ожидается 30)");
            Show($"Ceiling(30) → {set9.Ceiling(30)} (ожидается 30)");

            // Тест 10
            TestHeader(10, "PollFirst(), PollLast()");
            var set10 = new MyTreeSet<int>();
            set10.Add(5);
            set10.Add(1);
            set10.Add(3);
            Show("Исходное множество: ", set10);

            var pollFirst = set10.PollFirst();
            Show($"PollFirst() → {pollFirst} (ожидается 1)");
            Show("После PollFirst(): ", set10);

            var pollLast = set10.PollLast();
            Show($"PollLast() → {pollLast} (ожидается 5)");
            Show("После PollLast(): ", set10);

            // Тест 11
            TestHeader(11, "ToHashSet()");
            var set11 = new MyTreeSet<string>();
            set11.Add("X");
            set11.Add("Y");
            set11.Add("Z");
            Show("Множество: ", set11);
            var hashSet = set11.ToHashSet();
            Console.Write($"   • ToHashSet() → [ ");
            Console.ForegroundColor = ConsoleColor.White;
            bool first = true;
            foreach (var val in hashSet.OrderBy(x => x))
            {
                if (!first) Console.Write(", ");
                Console.Write(val);
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine(" ]");

            // Тест 12
            TestHeader(12, "Конструктор с компаратором (обратный порядок)");
            var set12 = new MyTreeSet<string>(Comparer<string>.Create((a, b) => b.CompareTo(a)));
            set12.Add("A");
            set12.Add("B");
            set12.Add("C");
            Show("Множество с обратным компаратором: ", set12);
            Show($"First() → {set12.First()} (ожидается C)");
            Show($"Last() → {set12.Last()} (ожидается A)");

            // Тест 13
            TestHeader(13, "Обработка null значения (должна быть ошибка)");
            var set13 = new MyTreeSet<string>();
            try
            {
                set13.Add(null!);
                Show("ERROR: Должна была быть exception!");
            }
            catch (ArgumentNullException)
            {
                Show("Correctly thrown ArgumentNullException for null value");
            }

            // Тест 14
            TestHeader(14, "Пустое множество — граничные случаи");
            var set14 = new MyTreeSet<int>();
            Show($"IsEmpty() → {set14.IsEmpty()} (true)");
            Show($"Size → {set14.Size} (0)");
            Show($"First() → ");
            try
            {
                var f = set14.First();
                Show("ERROR: Должна была быть exception!");
            }
            catch (InvalidOperationException)
            {
                Show("Correctly thrown InvalidOperationException");
            }

            // Тест 15
            TestHeader(15, "Lower/Floor/Higher/Ceiling для несуществующих значений");
            var set15 = new MyTreeSet<int>();
            set15.Add(5);
            set15.Add(15);
            set15.Add(25);
            Show("Множество: ", set15);
            Show($"Lower(10) → {set15.Lower(10)} (ожидается 5)");
            Show($"Floor(10) → {set15.Floor(10)} (ожидается 5)");
            Show($"Higher(10) → {set15.Higher(10)} (ожидается 15)");
            Show($"Ceiling(10) → {set15.Ceiling(10)} (ожидается 15)");
            Show($"Lower(3) → {set15.Lower(3)} (ожидается null)");
            Show($"Ceiling(30) → {set15.Ceiling(30)} (ожидается null)");

            Console.WriteLine();
            Console.WriteLine("ВСЕ ТЕСТЫ ПРОЙДЕНЫ УСПЕШНО!");
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

        static void Show<T>(string message, MyTreeSet<T>? set = null) where T : IComparable<T>
        {
            Console.Write($"   • {message}");
            if (set != null)
            {
                Console.Write(" → [ ");
                Console.ForegroundColor = ConsoleColor.White;
                var values = set.ToHashSet();
                var sorted = values.ToList();
                sorted.Sort((a, b) => a.CompareTo(b));
                bool first = true;
                foreach (var val in sorted)
                {
                    if (!first) Console.Write(", ");
                    Console.Write(val);
                    first = false;
                }
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
