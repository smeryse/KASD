using System;
using Task6.Collections;
using Task8.Collections;
using Task10.Collection;
using Task19.Collection;
using Task25;
using Task29.Collections;

namespace Task28
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 28: Итераторы ===\n");

            DemoPriorityQueueIterator();
            DemoHashSetIterator();
            DemoTreeSetIterator();
            DemoArrayListIterator();
            DemoVectorIterator();
            DemoLinkedListIterator();
            DemoErrorHierarchy();

            Console.WriteLine("\n=== Готово ===");
        }

        static void DemoPriorityQueueIterator()
        {
            Console.WriteLine("--- MyPriorityQueue Iterator ---");
            var pq = new MyPriorityQueue<int>();
            pq.Add(5); pq.Add(3); pq.Add(8); pq.Add(1); pq.Add(9);
            var it = new MyPriorityQueueIterator<int>(pq);
            Console.Write("Элементы: ");
            while (it.HasNext()) Console.Write(it.Next() + " ");
            Console.WriteLine();
        }

        static void DemoHashSetIterator()
        {
            Console.WriteLine("\n--- MyHashSet Iterator ---");
            var hs = new MyHashSet<int>();
            hs.AddAll(new[] { 5, 3, 8, 1, 9, 2 });
            var it = new MyHashSetIterator<int>(hs);
            Console.Write("Элементы: ");
            while (it.HasNext()) Console.Write(it.Next() + " ");
            Console.WriteLine();
        }

        static void DemoTreeSetIterator()
        {
            Console.WriteLine("\n--- MyTreeSet Iterator ---");
            var ts = new MyTreeSet<int>();
            ts.Add(5); ts.Add(3); ts.Add(8); ts.Add(1); ts.Add(9);
            var it = new MyTreeSetIterator<int>(ts);
            Console.Write("Элементы: ");
            while (it.HasNext()) Console.Write(it.Next() + " ");
            Console.WriteLine();
        }

        static void DemoArrayListIterator()
        {
            Console.WriteLine("\n--- MyArrayList ListIterator ---");
            var list = new MyArrayList<string>();
            list.Add("a"); list.Add("b"); list.Add("c"); list.Add("d");
            var it = new MyArrayListIterator<string>(list);
            Console.Write("Вперёд: ");
            while (it.HasNext()) Console.Write(it.Next() + " ");
            Console.Write("\nНазад: ");
            while (it.HasPrevious()) Console.Write(it.Previous() + " ");
            Console.WriteLine();
        }

        static void DemoVectorIterator()
        {
            Console.WriteLine("\n--- MyVector ListIterator ---");
            var vec = new MyVector<int>();
            vec.Add(10); vec.Add(20); vec.Add(30);
            var it = new MyVectorIterator<int>(vec);
            Console.Write("Вперёд: ");
            while (it.HasNext()) Console.Write(it.Next() + " ");
            Console.Write("\nНазад: ");
            while (it.HasPrevious()) Console.Write(it.Previous() + " ");
            Console.WriteLine();
        }

        static void DemoLinkedListIterator()
        {
            Console.WriteLine("\n--- MyLinkedList ListIterator ---");
            var ll = new MyLinkedList<string>();
            ll.Add("x"); ll.Add("y"); ll.Add("z");
            var it = new MyLinkedListIterator<string>(ll);
            Console.Write("Вперёд: ");
            while (it.HasNext()) Console.Write(it.Next() + " ");
            Console.Write("\nНазад: ");
            while (it.HasPrevious()) Console.Write(it.Previous() + " ");
            Console.WriteLine();
        }

        static void DemoErrorHierarchy()
        {
            Console.WriteLine("\n--- Иерархия ошибок ---");
            try { throw new MyNoSuchElementException(); }
            catch (MyCollectionException ex) { Console.WriteLine($"MyNoSuchElementException: {ex.Message}"); }

            try { throw new MyIllegalStateException(); }
            catch (MyCollectionException ex) { Console.WriteLine($"MyIllegalStateException: {ex.Message}"); }

            try { throw new MyUnsupportedOperationException(); }
            catch (MyCollectionException ex) { Console.WriteLine($"MyUnsupportedOperationException: {ex.Message}"); }
        }
    }
}
