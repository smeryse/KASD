using Task25;
using Task19.Collection;
using Task18.Collection;

namespace Task28
{
    public interface IMyIterator<T>
    {
        bool HasNext();
        T Next();
        void Remove();
    }

    // Расширение для MyHashSet с итератором
    public static class MyHashSetExtensions
    {
        public static IMyIterator<T> GetIterator<T>(this MyHashSet<T> set) where T : IComparable<T>
        {
            return new MyHashSetIterator<T>(set);
        }
    }

    public class MyHashSetIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private readonly MyHashSet<T> set;
        private readonly List<T> elements;
        private int cursor;
        private int lastRet = -1;

        public MyHashSetIterator(MyHashSet<T> set)
        {
            this.set = set;
            // Используем рефлексивный доступ или сериализацию для получения элементов
            // В реальном решении нужен доступ к внутреннему полю map.KeySet()
            this.elements = new List<T>();
            var objArr = set.ToArray();
            foreach (var obj in objArr)
            {
                if (obj is T t)
                    elements.Add(t);
            }
            this.cursor = 0;
            this.lastRet = -1;
        }

        public bool HasNext() => cursor < elements.Count;

        public T Next()
        {
            if (cursor >= elements.Count)
                throw new InvalidOperationException("No more elements");
            lastRet = cursor++;
            return elements[lastRet];
        }

        public void Remove()
        {
            if (lastRet < 0)
                throw new InvalidOperationException("No element to remove");
            set.Remove(elements[lastRet]);
            elements.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }

    public class MyTreeSetIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private readonly MyTreeSet<T> treeSet;
        private readonly List<T> elements;
        private int cursor;
        private int lastRet = -1;

        public MyTreeSetIterator(MyTreeSet<T> treeSet)
        {
            this.treeSet = treeSet;
            this.elements = treeSet.ToArray().ToList();
            this.cursor = 0;
            this.lastRet = -1;
        }

        public bool HasNext() => cursor < elements.Count;

        public T Next()
        {
            if (cursor >= elements.Count)
                throw new InvalidOperationException("No more elements");
            lastRet = cursor++;
            return elements[lastRet];
        }

        public void Remove()
        {
            if (lastRet < 0)
                throw new InvalidOperationException("No element to remove");
            treeSet.Remove(elements[lastRet]);
            elements.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 28: Итераторы для коллекций ===\n");

            Console.WriteLine("--- MyHashSet<T> Iterator ---");
            var hashSet = new MyHashSet<int>();
            hashSet.AddAll(new[] { 5, 3, 8, 1, 9, 2 });
            Console.WriteLine($"Множество: {hashSet}");

            var hashSetItr = hashSet.GetIterator();
            Console.Write("Итерация: ");
            while (hashSetItr.HasNext())
            {
                Console.Write(hashSetItr.Next() + " ");
            }
            Console.WriteLine();

            Console.WriteLine("\nИтерация с удалением:");
            hashSetItr = hashSet.GetIterator();
            while (hashSetItr.HasNext())
            {
                var val = hashSetItr.Next();
                if (val % 2 == 0)
                {
                    Console.WriteLine($"  Удаляем: {val}");
                    hashSetItr.Remove();
                }
            }
            Console.WriteLine($"Множество после удаления чётных: {hashSet}");

            Console.WriteLine("\n--- MyTreeSet<T> Iterator ---");
            var treeSet = new MyTreeSet<int>();
            treeSet.Add(5);
            treeSet.Add(3);
            treeSet.Add(8);
            treeSet.Add(1);
            treeSet.Add(9);
            treeSet.Add(2);
            Console.WriteLine($"Множество: ");
            treeSet.Print();

            var treeSetItr = new MyTreeSetIterator<int>(treeSet);
            Console.Write("Итерация: ");
            while (treeSetItr.HasNext())
            {
                Console.Write(treeSetItr.Next() + " ");
            }
            Console.WriteLine();

            Console.WriteLine("\nИтерация с удалением:");
            treeSetItr = new MyTreeSetIterator<int>(treeSet);
            while (treeSetItr.HasNext())
            {
                var val = treeSetItr.Next();
                if (val > 5)
                {
                    Console.WriteLine($"  Удаляем: {val}");
                    treeSetItr.Remove();
                }
            }
            Console.WriteLine("Множество после удаления элементов > 5: ");
            treeSet.Print();

            Console.WriteLine("\n=== Готово ===");
        }
    }
}
