using System;
using System.Collections.Generic;
using System.Linq;

namespace Task6.Collections
{
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

        private void ShiftUp(int current)
        {
            while (current > 0)
            {
                int parent = (current - 1) / 2;
                if (GreaterOrEq(parent, current))
                    break;
                else
                {
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

                if (right >= Count) current = left;
                else current = GreaterOrEq(right, left) ? right : left;

                if (GreaterOrEq(index, current)) return;
                else
                {
                    Swap(index, current);
                    index = current;
                    left = index * 2 + 1;
                    right = index * 2 + 2;
                }
            }
        }

        public T ExtractMax()
        {
            if (IsEmpty()) throw new InvalidOperationException("Куча пустая");
            T root = _items[0];
            int last = Count - 1;
            Swap(0, last);
            _items.RemoveAt(last);
            if (Count > 0) ShiftDown(0);
            return root;
        }
        public T Peek()
        {
            if (IsEmpty()) throw new InvalidOperationException("Куча пустая");
            return _items[0];
        }

        public void IncreaseKey(int index, T newValue)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException();

            T oldValue = _items[index];
            _items[index] = newValue;

            if (GreaterOrEq(newValue, oldValue)) ShiftUp(index);
            else ShiftDown(index);
        }

        public T[] ToArray()
        {
            return _items.ToArray();
        }

        public bool Contains(T item)
        {
            if (item == null) return false;
            return _items.Contains(item);
        }

        public bool Remove(T item)
        {
            if (item == null) return false;
            int index = _items.FindIndex(x => EqualityComparer<T>.Default.Equals(x, item));
            if (index == -1) return false;
            int last = Count - 1;
            if (index != last)
            {
                Swap(index, last);
                _items.RemoveAt(last);
                if (index < Count)
                {
                    ShiftDown(index);
                    ShiftUp(index);
                }
            }
            else
            {
                _items.RemoveAt(last);
            }
            return true;
        }

        public void Clear() => _items.Clear();

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
                Console.WriteLine("\n");
            }
            Console.WriteLine("====================");
        }
    }
}
