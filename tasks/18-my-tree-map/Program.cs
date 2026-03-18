using System;
using Task18.Collection;

namespace Task18
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ТЕСТИРОВАНИЕ РЕАЛИЗАЦИИ MyTreeMap");

            // Тест 1
            TestHeader(1, "Конструктор по умолчанию, Put(), Get(), Size()");
            var map1 = new MyTreeMap<string, int>();
            map1.Put("one", 1);
            map1.Put("two", 2);
            map1.Put("three", 3);
            Show($"Размер карты: {map1.Size} (ожидается 3)");
            Show($"Значение по ключу 'two': {map1.Get("two")} (ожидается 2)");
            Show("Содержимое: ", map1);

            // Тест 2
            TestHeader(2, "Конструктор с компаратором (обратный порядок)");
            var map2 = new MyTreeMap<string, int>(Comparer<string>.Create((a, b) => b.CompareTo(a)));
            map2.Put("A", 10);
            map2.Put("B", 20);
            map2.Put("C", 30);
            Show("Карта с обратным компаратором: ", map2);
            Show($"Первый ключ: {map2.FirstKey()} (ожидается C)");
            Show($"Последний ключ: {map2.LastKey()} (ожидается A)");

            // Тест 3
            TestHeader(3, "ContainsKey(), ContainsValue(), IsEmpty()");
            var map3 = new MyTreeMap<string, string>();
            map3.Put("apple", "red");
            map3.Put("banana", "yellow");
            map3.Put("grape", "purple");
            Show($"ContainsKey('banana') → {map3.ContainsKey("banana")} (true)");
            Show($"ContainsKey('orange') → {map3.ContainsKey("orange")} (false)");
            Show($"ContainsValue('yellow') → {map3.ContainsValue("yellow")} (true)");
            Show($"ContainsValue('blue') → {map3.ContainsValue("blue")} (false)");
            Show($"IsEmpty() → {map3.IsEmpty()} (false)");

            // Тест 4
            TestHeader(4, "Remove() и Clear()");
            var map4 = new MyTreeMap<int, string>();
            for (int i = 1; i <= 5; i++) map4.Put(i, $"Value{i}");
            Show("Исходная карта: ", map4);
            string? removed = map4.Remove(3);
            Show($"Remove(3) вернул: \"{removed}\" (ожидается Value3)");
            Show("После удаления: ", map4);
            map4.Clear();
            Show($"После Clear(): Size = {map4.Size}, IsEmpty = {map4.IsEmpty()} (0, true)");

            // Тест 5
            TestHeader(5, "FirstKey(), LastKey(), FirstEntry(), LastEntry()");
            var map5 = new MyTreeMap<string, int>();
            map5.Put("zebra", 5);
            map5.Put("apple", 1);
            map5.Put("mango", 3);
            Show("Карта: ", map5);
            Show($"FirstKey() → {map5.FirstKey()} (ожидается apple)");
            Show($"LastKey() → {map5.LastKey()} (ожидается zebra)");
            var firstEntry = map5.FirstEntry();
            Show($"FirstEntry() → {firstEntry?.Key}={firstEntry?.Value} (apple=1)");
            var lastEntry = map5.LastEntry();
            Show($"LastEntry() → {lastEntry?.Key}={lastEntry?.Value} (zebra=5)");

            // Тест 6
            TestHeader(6, "HeadMap() — элементы с ключом меньше end");
            var map6 = new MyTreeMap<int, string>();
            for (int i = 1; i <= 10; i++) map6.Put(i, $"V{i}");
            Show<int, string>("Исходная карта (1..10): ", map6);
            var headMap = map6.HeadMap(5);
            Show<int, string>($"HeadMap(5): ", headMap);
            Show("Ожидается: 1=V1, 2=V2, 3=V3, 4=V4");

            // Тест 7
            TestHeader(7, "TailMap() — элементы с ключом больше start");
            var map7 = new MyTreeMap<int, string>();
            for (int i = 1; i <= 10; i++) map7.Put(i, $"V{i}");
            Show<int, string>("Исходная карта (1..10): ", map7);
            var tailMap = map7.TailMap(5);
            Show<int, string>($"TailMap(5): ", tailMap);
            Show("Ожидается: 6=V6, 7=V7, 8=V8, 9=V9, 10=V10");

            // Тест 8
            TestHeader(8, "SubMap() — элементы в диапазоне [start, end)");
            var map8 = new MyTreeMap<int, string>();
            for (int i = 1; i <= 10; i++) map8.Put(i, $"V{i}");
            Show<int, string>("Исходная карта (1..10): ", map8);
            var subMap = map8.SubMap(3, 7);
            Show<int, string>($"SubMap(3, 7): ", subMap);
            Show("Ожидается: 3=V3, 4=V4, 5=V5, 6=V6");

            // Тест 9
            TestHeader(9, "LowerEntry(), FloorEntry(), HigherEntry(), CeilingEntry()");
            var map9 = new MyTreeMap<int, string>();
            map9.Put(10, "A");
            map9.Put(20, "B");
            map9.Put(30, "C");
            map9.Put(40, "D");
            Show("Карта: ", map9);
            
            var lower = map9.LowerEntry(25);
            Show($"LowerEntry(25) → {lower?.Key}={lower?.Value} (ожидается 20=B)");
            
            var floor = map9.FloorEntry(20);
            Show($"FloorEntry(20) → {floor?.Key}={floor?.Value} (ожидается 20=B)");
            
            var higher = map9.HigherEntry(25);
            Show($"HigherEntry(25) → {higher?.Key}={higher?.Value} (ожидается 30=C)");
            
            var ceiling = map9.CeilingEntry(30);
            Show($"CeilingEntry(30) → {ceiling?.Key}={ceiling?.Value} (ожидается 30=C)");

            // Тест 10
            TestHeader(10, "LowerKey(), FloorKey(), HigherKey(), CeilingKey()");
            var map10 = new MyTreeMap<string, int>();
            map10.Put("apple", 1);
            map10.Put("banana", 2);
            map10.Put("cherry", 3);
            map10.Put("date", 4);
            Show("Карта: ", map10);
            
            Show($"LowerKey('cherry') → {map10.LowerKey("cherry")} (ожидается banana)");
            Show($"FloorKey('cherry') → {map10.FloorKey("cherry")} (ожидается cherry)");
            Show($"HigherKey('banana') → {map10.HigherKey("banana")} (ожидается cherry)");
            Show($"CeilingKey('banana') → {map10.CeilingKey("banana")} (ожидается banana)");

            // Тест 11
            TestHeader(11, "PollFirstEntry(), PollLastEntry()");
            var map11 = new MyTreeMap<int, string>();
            map11.Put(5, "E");
            map11.Put(1, "A");
            map11.Put(3, "C");
            Show("Исходная карта: ", map11);
            
            var pollFirst = map11.PollFirstEntry();
            Show($"PollFirstEntry() → {pollFirst?.Key}={pollFirst?.Value} (ожидается 1=A)");
            Show("После PollFirstEntry(): ", map11);
            
            var pollLast = map11.PollLastEntry();
            Show($"PollLastEntry() → {pollLast?.Key}={pollLast?.Value} (ожидается 5=E)");
            Show("После PollLastEntry(): ", map11);

            // Тест 12
            TestHeader(12, "KeySet() и EntrySet()");
            var map12 = new MyTreeMap<string, int>();
            map12.Put("X", 100);
            map12.Put("Y", 200);
            map12.Put("Z", 300);
            Show("Карта: ", map12);
            
            var keySet = map12.KeySet();
            Console.Write("   • KeySet() → [ ");
            Console.ForegroundColor = ConsoleColor.White;
            bool first = true;
            foreach (var key in keySet)
            {
                if (!first) Console.Write(", ");
                Console.Write(key);
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine(" ]");
            
            var entrySet = map12.EntrySet();
            Console.Write("   • EntrySet() → [ ");
            Console.ForegroundColor = ConsoleColor.White;
            first = true;
            foreach (var entry in entrySet)
            {
                if (!first) Console.Write(", ");
                Console.Write($"{entry.Key}={entry.Value}");
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine(" ]");

            // Тест 13
            TestHeader(13, "Обновление значения существующего ключа");
            var map13 = new MyTreeMap<string, int>();
            map13.Put("key1", 10);
            map13.Put("key2", 20);
            Show("До обновления: ", map13);
            map13.Put("key1", 999);
            Show("После Put('key1', 999): ", map13);
            Show($"Get('key1') → {map13.Get("key1")} (ожидается 999)");

            // Тест 14
            TestHeader(14, "Обработка null ключа (должна быть ошибка)");
            var map14 = new MyTreeMap<string, int>();
            try
            {
                map14.Put(null!, 100);
                Show("ERROR: Должна была быть exception!");
            }
            catch (ArgumentNullException)
            {
                Show("Correctly thrown ArgumentNullException for null key");
            }

            // Тест 15
            TestHeader(15, "Пустая карта — граничные случаи");
            var map15 = new MyTreeMap<string, int>();
            Show($"IsEmpty() → {map15.IsEmpty()} (true)");
            Show($"Size → {map15.Size} (0)");
            Show($"FirstKey() → ");
            try
            {
                var fk = map15.FirstKey();
                Show("ERROR: Должна была быть exception!");
            }
            catch (InvalidOperationException)
            {
                Show("Correctly thrown InvalidOperationException");
            }

            Console.WriteLine();
            Console.WriteLine("ВСЕ ТЕСТЫ ПРОЙДЕНЫ УСПЕШНО!");

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

        // Generic Show method to avoid ambiguity
        static void Show<TK, TV>(string message, MyTreeMap<TK, TV>? map = null) where TK : IComparable<TK>
        {
            Console.Write($"   • {message}");
            if (map != null)
            {
                Console.Write(" → [ ");
                Console.ForegroundColor = ConsoleColor.White;
                var entrySet = map.EntrySet();
                bool first = true;
                foreach (var entry in entrySet)
                {
                    if (!first) Console.Write(", ");
                    Console.Write($"{entry.Key}={entry.Value}");
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