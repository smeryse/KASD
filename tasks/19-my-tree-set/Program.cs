using Task19.Collection;

namespace Task19
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ MyTreeSet (Красно-чёрное дерево) ===\n");

            // Тест 1: Базовое добавление
            TestHeader(1, "Add(), Size()");
            var set1 = new MyTreeSet<int>();
            for (int i = 1; i <= 10; i++) set1.Add(i);
            Show($"Size = {set1.Size} (ожидается 10)");
            Show("Содержимое: ", set1);

            // Тест 2: Дубликаты
            TestHeader(2, "Add дубликатов");
            var set2 = new MyTreeSet<int>();
            set2.Add(5); set2.Add(3); set2.Add(7);
            bool added = set2.Add(5);
            Show($"Add(5) вернул {added} (ожидается false), Size = {set2.Size}");

            // Тест 3: Contains, Remove
            TestHeader(3, "Contains(), Remove()");
            var set3 = new MyTreeSet<string>();
            set3.AddAll(new[] { "apple", "banana", "cherry" });
            Show($"Contains('banana') = {set3.Contains("banana")} (true)");
            Show($"Contains('grape') = {set3.Contains("grape")} (false)");
            set3.Remove("banana");
            Show($"После Remove('banana'): Size = {set3.Size}");

            // Тест 4: First, Last, PollFirst, PollLast
            TestHeader(4, "First(), Last(), PollFirst(), PollLast()");
            var set4 = new MyTreeSet<int>();
            set4.AddAll(new[] { 50, 30, 70, 20, 40, 60, 80 });
            Show($"First() = {set4.First()} (20), Last() = {set4.Last()} (80)");
            Show($"PollFirst() = {set4.PollFirst()} (20)");
            Show($"PollLast() = {set4.PollLast()} (80)");
            Show($"После: First = {set4.First()}, Last = {set4.Last()}");

            // Тест 5: HeadSet, TailSet, SubSet
            TestHeader(5, "HeadSet(), TailSet(), SubSet()");
            var set5 = new MyTreeSet<int>();
            for (int i = 1; i <= 20; i++) set5.Add(i);
            var head = set5.HeadSet(10);
            var tail = set5.TailSet(15);
            var sub = set5.SubSet(5, 12);
            Show<int>($"HeadSet(10): ", head);
            Show<int>($"TailSet(15): ", tail);
            Show<int>($"SubSet(5, 12): ", sub);

            // Тест 6: Ceiling, Floor, Higher, Lower
            TestHeader(6, "Ceiling(), Floor(), Higher(), Lower()");
            var set6 = new MyTreeSet<int>();
            set6.AddAll(new[] { 10, 20, 30, 40, 50 });
            Show($"Ceiling(25) = {set6.Ceiling(25)} (30)");
            Show($"Floor(25) = {set6.Floor(25)} (20)");
            Show($"Higher(30) = {set6.Higher(30)} (40)");
            Show($"Lower(30) = {set6.Lower(30)} (20)");

            // Тест 7: ToArray, ToHashSet
            TestHeader(7, "ToArray(), ToHashSet()");
            var set7 = new MyTreeSet<string>();
            set7.AddAll(new[] { "X", "Y", "Z" });
            var arr = set7.ToArray();
            Show($"ToArray: [{string.Join(", ", arr)}]");

            // Тест 8: DescendingSet, DescendingIterator
            TestHeader(8, "DescendingSet(), DescendingIterator()");
            var set8 = new MyTreeSet<int>();
            set8.AddAll(new[] { 1, 2, 3, 4, 5 });
            Show<int>("Исходное: ", set8);
            var desc = set8.DescendingSet();
            Show<int>("DescendingSet: ", desc);
            Console.Write("   • DescendingIterator: [ ");
            bool first = true;
            foreach (var val in set8.DescendingIterator())
            {
                if (!first) Console.Write(", ");
                Console.Write(val);
                first = false;
            }
            Console.WriteLine(" ]");

            // Тест 9: Конструктор с компаратором
            TestHeader(9, "Конструктор с обратным компаратором");
            var set9 = new MyTreeSet<string>(Comparer<string>.Create((a, b) => b.CompareTo(a)));
            set9.AddAll(new[] { "A", "B", "C" });
            Show("Обратный порядок: ", set9);
            Show($"First() = {set9.First()} (C), Last() = {set9.Last()} (A)");

            // Тест 10: Clear, IsEmpty
            TestHeader(10, "Clear(), IsEmpty()");
            var set10 = new MyTreeSet<int>();
            set10.Add(1); set10.Add(2);
            Show($"До Clear: IsEmpty = {set10.IsEmpty()}");
            set10.Clear();
            Show($"После Clear: Size = {set10.Size}, IsEmpty = {set10.IsEmpty()}");

            Console.WriteLine("\n=== ВСЕ ТЕСТЫ ПРОЙДЕНЫ ===");
        }

        static void TestHeader(int number, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nТест {number}: {description}");
            Console.ResetColor();
        }

        static void Show<T>(string message, MyTreeSet<T>? set = null) where T : IComparable<T>
        {
            Console.Write($"   • {message}");
            if (set != null)
            {
                Console.Write(" → [ ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                var arr = set.ToArray();
                Console.Write(string.Join(", ", arr));
                Console.ResetColor();
                Console.Write(" ]");
            }
            Console.WriteLine();
        }

        static void Show(string message) => Console.WriteLine($"   • {message}");
    }
}
