using Task21.Collections;

namespace Task21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ MyHashMap ===\n");

            // Тест 1: Базовое добавление
            TestHeader(1, "Put(), Get(), Size()");
            var map1 = new MyHashMap<string, int>();
            map1.Put("one", 1);
            map1.Put("two", 2);
            map1.Put("three", 3);
            Show($"Size = {map1.Size} (ожидается 3)");
            Show($"Get('two') = {map1.Get("two")} (ожидается 2)");

            // Тест 2: Обновление значения
            TestHeader(2, "Обновление значения по ключу");
            map1.Put("two", 22);
            Show($"После Put('two', 22): Get('two') = {map1.Get("two")} (ожидается 22)");
            Show($"Size = {map1.Size} (ожидается 3)");

            // Тест 3: ContainsKey, ContainsValue
            TestHeader(3, "ContainsKey(), ContainsValue()");
            Show($"ContainsKey('one') = {map1.ContainsKey("one")} (true)");
            Show($"ContainsKey('four') = {map1.ContainsKey("four")} (false)");
            Show($"ContainsValue(3) = {map1.ContainsValue(3)} (true)");
            Show($"ContainsValue(99) = {map1.ContainsValue(99)} (false)");

            // Тест 4: Remove
            TestHeader(4, "Remove()");
            var removed = map1.Remove("one");
            Show($"Remove('one') = {removed} (1)");
            Show($"Size = {map1.Size} (ожидается 2)");
            Show($"ContainsKey('one') = {map1.ContainsKey("one")} (false)");

            // Тест 5: KeySet, EntrySet
            TestHeader(5, "KeySet(), EntrySet()");
            var map2 = new MyHashMap<int, string>();
            map2.Put(1, "A");
            map2.Put(2, "B");
            map2.Put(3, "C");
            var keys = map2.KeySet();
            Show($"KeySet: [{string.Join(", ", keys)}]");
            var entries = map2.EntrySet();
            Console.Write("   • EntrySet: [ ");
            bool first = true;
            foreach (var entry in entries)
            {
                if (!first) Console.Write(", ");
                Console.Write($"({entry.Key}, {entry.Value})");
                first = false;
            }
            Console.WriteLine(" ]");

            // Тест 6: Clear, IsEmpty
            TestHeader(6, "Clear(), IsEmpty()");
            var map3 = new MyHashMap<string, int>();
            map3.Put("A", 1);
            Show($"До Clear: IsEmpty = {map3.IsEmpty()}");
            map3.Clear();
            Show($"После Clear: Size = {map3.Size}, IsEmpty = {map3.IsEmpty()}");

            // Тест 7: Автоматическое расширение (resize)
            TestHeader(7, "Автоматическое расширение таблицы");
            var map4 = new MyHashMap<string, int>(4, 0.75f); // capacity=4, threshold=3
            for (int i = 0; i < 20; i++)
            {
                map4.Put($"key{i}", i);
            }
            Show($"После 20 вставок: Size = {map4.Size}");
            Show($"Все элементы доступны: Get('key19') = {map4.Get("key19")}");

            // Тест 8: Конструкторы
            TestHeader(8, "Различные конструкторы");
            var map5 = new MyHashMap<string, int>(); // default
            Show($"Default constructor: capacity=16, loadFactor=0.75");
            var map6 = new MyHashMap<string, int>(32); // custom capacity
            Show($"Custom capacity: 32");
            var map7 = new MyHashMap<string, int>(16, 0.5f); // custom capacity + load factor
            Show($"Custom capacity + loadFactor: 16, 0.5");

            // Тест 9: Коллизии
            TestHeader(9, "Обработка коллизий");
            var map8 = new MyHashMap<string, string>();
            map8.Put("A", "Value A");
            map8.Put("B", "Value B");
            map8.Put("C", "Value C");
            map8.Put("A", "Updated A"); // коллизия — обновление
            Show($"Get('A') = {map8.Get("A")} (Updated A)");
            Show($"Get('B') = {map8.Get("B")} (Value B)");
            Show($"Size = {map8.Size} (3, т.к. 'A' обновлён)");

            // Тест 10: Удаление несуществующего ключа
            TestHeader(10, "Remove несуществующего ключа");
            var map9 = new MyHashMap<int, string>();
            var result = map9.Remove(999);
            Show($"Remove(999) = {result ?? "null"} (ожидается null)");

            Console.WriteLine("\n=== ВСЕ ТЕСТЫ ПРОЙДЕНЫ ===");
        }

        static void TestHeader(int number, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nТест {number}: {description}");
            Console.ResetColor();
        }

        static void Show(string message) => Console.WriteLine($"   • {message}");
    }
}
