using System;
using System.Collections.Generic;
using System.Linq;

namespace Task5.Collections
{
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

    public class MaxHeap<T> where T : IComparable<T>
    {
        public List<T> _items = new List<T>();
        public int Count => _items.Count;

        public MaxHeap() { }
        public MaxHeap(IEnumerable<T> values)
        {
            if (values != null)
            {
                _items.AddRange(values);
                for (int i = (Count / 2) - 1; i >= 0; i--)
                    ShiftDown(i);
            }

        }

        private bool GreaterOrEq(int i, int j)
        {
            return _items[i].CompareTo(_items[j]) >= 0;
        }
        private bool GreaterOrEq(T a, T b)
        {
            return a.CompareTo(b) >= 0;
        }

        private void Swap(int i, int j)
        {
            T temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;
        }

        private bool IsEmpty() => Count == 0;
        public void Insert(T value)
        {
            _items.Add(value);
            ShiftUp(Count - 1);
        }

        // Проверить корректность работы
        private void ShiftUp(int current)
        {
            while (current > 0)
            {
                int parent = (current - 1) / 2;
                if (GreaterOrEq(parent, current))
                    break; // Свойство кучи восстановлено
                else
                {
                    // Поднимаем текущий элемент
                    Swap(current, parent);
                    current = parent;
                }
            }
        }
        private void ShiftDown(int index)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;

            while (left < Count)
            {
                int current;

                // Вычисление бОльшего потомка
                if (right >= Count) current = left;
                else current = GreaterOrEq(right, left) ? right : left;

                // Проверка условия кучи
                if (GreaterOrEq(index, current)) return;
                else
                {
                    // Меняем местами текущий и бОльший
                    Swap(index, current);
                    index = current;
                    left = index * 2 + 1;
                    right = index * 2 + 2;
                }
            }
        }

        public T ExtractMax()
        {
            if (IsEmpty()) throw new Exception("Куча пустая");
            else
            {
                T root = _items[0];
                int last = Count - 1;
                Swap(0, last);
                _items.RemoveAt(last);
                ShiftDown(0);
                return root;
            }
        }
        public T Peek()
        {
            if (IsEmpty()) throw new Exception("Куча пустая");
            else return _items[0];
        }

        public void IncreaseKey(int index, T newValue)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException();
            
            T oldValue = _items[index];
            _items[index] = newValue;

            if (GreaterOrEq(newValue, oldValue)) ShiftUp(index);
            else ShiftDown(index);
        }
        public void Print()
        {
            if (Count == 0) return;

            int levels = (int)Math.Ceiling(Math.Log(Count + 1, 2));

            for (int level = 0; level < levels; level++)
            {
                int start = (int)Math.Pow(2, level) - 1;
                int end = Math.Min((int)Math.Pow(2, level + 1) - 2, Count - 1);

                int maxWidth = (int)Math.Pow(2, levels) * 2;
                int spaces = maxWidth / (int)Math.Pow(2, level + 1);

                Console.Write(new string(' ', spaces));

                for (int i = start; i <= end; i++)
                {
                    Console.Write(_items[i]);
                    if (i < end)
                        Console.Write(new string(' ', spaces * 2));
                }
                Console.WriteLine();
            }
            Console.WriteLine("====================");
        }
    }
}