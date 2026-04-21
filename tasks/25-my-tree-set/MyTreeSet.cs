using System;
using System.Collections.Generic;
using System.Text;
using Task18.Collection;

namespace Task25
{
    public class MyHashSet<T> where T : IComparable<T>
    {
        private static readonly object DUMMY = new object();

        private MyTreeMap<T, object> map;

        public MyHashSet()
        {
            map = new MyTreeMap<T, object>();
        }

        public MyHashSet(T[] a) : this()
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Массив не может быть null.");

            AddAll(a);
        }

        public MyHashSet(IComparer<T> comparer)
        {
            map = new MyTreeMap<T, object>(comparer);
        }

        public MyHashSet(int initialCapacity, float loadFactor)
        {
            // Дерево автоматически масштабируется, поэтому просто создаём пустое множество
            map = new MyTreeMap<T, object>();
        }

        public MyHashSet(int initialCapacity) : this(initialCapacity, 0.75f) {}

        public bool Add(T e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e), "Элемент не может быть null.");

            if (map.ContainsKey(e))
                return false;

            map.Put(e, DUMMY);
            return true;
        }

        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Массив не может быть null.");

            foreach (T item in a)
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item), "Элементы массива не могут быть null.");

                map.Put(item, DUMMY);
            }
        }

        public void Clear()
        {
            map.Clear();
        }

        public bool Contains(T o)
        {
            if (o == null)
                return false;

            return map.ContainsKey(o);
        }

        public bool ContainsAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Массив не может быть null.");

            foreach (T item in a)
            {
                if (item == null || !map.ContainsKey(item))
                    return false;
            }

            return true;
        }

        public bool IsEmpty() => map.Size == 0;

        public bool Remove(T o)
        {
            if (o == null)
                return false;

            return map.Remove(o) != null;
        }

        public bool RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Массив не может быть null.");

            bool modified = false;

            foreach (T item in a)
            {
                if (map.ContainsKey(item) && map.Remove(item) != null)
                    modified = true;
            }

            return modified;
        }

        public bool RetainAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Массив не может быть null.");

            var retain = new MyHashSet<T>(a);

            var toRemove = new List<T>();

            foreach (T key in map.KeySet())
            {
                if (!retain.Contains(key))
                    toRemove.Add(key);
            }

            if (toRemove.Count == 0)
                return false;

            foreach (T key in toRemove)
                map.Remove(key);

            return true;
        }

        public int Size() => map.Size;

        public object[] ToArray()
        {
            var keys = map.KeySet();
            object[] result = new object[keys.Count];
            int i = 0;

            foreach (T key in keys)
                result[i++] = key;

            return result;
        }

        public T[] ToArray(T[] a)
        {
            var keys = map.KeySet();
            
            if (a is null || a.Length < keys.Count)
                a = new T[keys.Count];

            int i = 0;

            foreach (T key in keys)
                a[i++] = key;

            if (a.Length > keys.Count)
                a[keys.Count] = default!;

            return a;
        }

        public override string ToString()
        {
            if (map.Size == 0)
                return "[]";

            var sb = new StringBuilder("[");
            bool first = true;

            foreach (T key in map.KeySet())
            {
                if (!first) sb.Append(", ");
                sb.Append(key);
                first = false;
            }

            sb.Append("]");
            return sb.ToString();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация MyHashSet<T> на основе MyTreeMap ===\n");

            var set1 = new MyHashSet<int>();
            Console.WriteLine($"Конструктор 1 — пустое множество: {set1}");
            Console.WriteLine($"  IsEmpty: {set1.IsEmpty()}, Size: {set1.Size()}");

            int[] initial = { 5, 3, 8, 1, 9, 3, 2 }; // дублирующиеся значения игнорируются
            var set2 = new MyHashSet<int>(initial);
            Console.WriteLine($"\nКонструктор 2 — из массива {{5,3,8,1,9,3,2}}: {set2}");
            Console.WriteLine($"  Size: {set2.Size()} (дубликаты отброшены)");

            var set3 = new MyHashSet<string>(new StringComparer());
            Console.WriteLine($"\nКонструктор 3 — с кастомным comparer: {set3}");

            var set4 = new MyHashSet<int>(16);
            Console.WriteLine($"Конструктор 4 — с initialCapacity=16: {set4}");
            Console.WriteLine($"  IsEmpty: {set4.IsEmpty()}, Size: {set4.Size()}");

            var set5 = new MyHashSet<int>(32, 0.5f);
            Console.WriteLine($"\nКонструктор 5 — с initialCapacity=32, loadFactor=0.5: {set5}");
            Console.WriteLine($"  IsEmpty: {set5.IsEmpty()}, Size: {set5.Size()}");

            var set6 = new MyHashSet<int>();
            Console.WriteLine($"\nКонструктор 6 — пустое множество: {set6}");

            Console.WriteLine("\n--- Add ---");
            var set = new MyHashSet<string>();
            Console.WriteLine($"  add(\"apple\")   : {set.Add("apple")}");
            Console.WriteLine($"  add(\"banana\")  : {set.Add("banana")}");
            Console.WriteLine($"  add(\"cherry\")  : {set.Add("cherry")}");
            Console.WriteLine($"  add(\"apple\")   : {set.Add("apple")} (дубликат)");
            Console.WriteLine($"  Множество: {set}");

            Console.WriteLine("\n--- AddAll ---");
            set.AddAll(new[] { "date", "elderberry", "banana" });
            Console.WriteLine($"  После AddAll({{\"date\",\"elderberry\",\"banana\"}}): {set}");

            Console.WriteLine("\n--- Contains ---");
            Console.WriteLine($"  Contains(\"apple\")  : {set.Contains("apple")}");
            Console.WriteLine($"  Contains(\"mango\")  : {set.Contains("mango")}");

            Console.WriteLine("\n--- ContainsAll ---");
            Console.WriteLine($"  ContainsAll({{\"apple\",\"banana\"}}): {set.ContainsAll(new[] { "apple", "banana" })}");
            Console.WriteLine($"  ContainsAll({{\"apple\",\"mango\"}}) : {set.ContainsAll(new[] { "apple", "mango" })}");

            Console.WriteLine("\n--- Remove ---");
            Console.WriteLine($"  Remove(\"banana\"): {set.Remove("banana")}");
            Console.WriteLine($"  Remove(\"mango\") : {set.Remove("mango")}");
            Console.WriteLine($"  Множество: {set}");

            Console.WriteLine("\n--- RemoveAll ---");
            set.RemoveAll(new[] { "date", "cherry" });
            Console.WriteLine($"  После RemoveAll({{\"date\",\"cherry\"}}): {set}");

            Console.WriteLine("\n--- RetainAll ---");
            set.AddAll(new[] { "fig", "grape", "honeydew" });
            Console.WriteLine($"  Перед RetainAll: {set}");
            set.RetainAll(new[] { "apple", "elderberry" });
            Console.WriteLine($"  После RetainAll({{\"apple\",\"elderberry\"}}): {set}");

            Console.WriteLine("\n--- ToArray ---");
            object[] objArr = set.ToArray();
            Console.WriteLine($"  ToArray()   : [{string.Join(", ", objArr)}]");

            string[] strArr = set.ToArray(new string[0]);
            Console.WriteLine($"  ToArray(new string[0]): [{string.Join(", ", strArr)}]");

            string[] preAlloc = new string[10];
            var result = set.ToArray(preAlloc);
            Console.WriteLine($"  ToArray(preAllocated[10]): элементы = [{string.Join(", ", result)}]");

            Console.WriteLine("\n--- Clear ---");
            set.Clear();
            Console.WriteLine($"  После Clear(): {set}");
            Console.WriteLine($"  IsEmpty: {set.IsEmpty()}");

            Console.WriteLine("\n--- Обработка ошибок ---");
            try
            {
                var bad = new MyHashSet<string>();
                bad.Add(null!);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Add(null): {ex.Message}");
            }

            try
            {
                var bad = new MyHashSet<string>();
                bad.AddAll(null!);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  AddAll(null): {ex.Message}");
            }

            try
            {
                var bad = new MyHashSet<string>(new string[] { "a", "b", null! });
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Конструктор с null элементом: {ex.Message}");
            }

            Console.WriteLine("\n=== Готово ===");
        }
    }

    class StringComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}