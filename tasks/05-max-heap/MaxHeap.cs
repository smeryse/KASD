using System;
using System.Collections.Generic;
using System.Linq;

namespace Task5.Collections
{
    public class MaxHeap<T> where T : IComparable<T>
    {
        #region Fields
        public List<T> _items = new List<T>();
        private IComparer<T> _comparer;
        public int Count => _items.Count;
        #endregion

        #region Constructors
        
        public MaxHeap() { }

        
        public MaxHeap(int capacity)
        {
            _items = new List<T>(capacity);
        }

        
        public MaxHeap(int capacity, IComparer<T> comparer)
        {
            _items = new List<T>(capacity);
            _comparer = comparer;
        }

        
        public MaxHeap(IEnumerable<T> values)
        {
            if (values != null)
            {
                _items.AddRange(values);
                for (int i = (Count / 2) - 1; i >= 0; i--)
                    ShiftDown(i);
            }

        }
        
        public MaxHeap(IEnumerable<T> values, IComparer<T> comparer)
        {
            _comparer = comparer;
            if (values != null)
            {
                _items.AddRange(values);
                for (int i = (Count / 2) - 1; i >= 0; i--)
                    ShiftDown(i);
            }
        }
        #endregion


        #region Private helper methods (CompareItems, GreaterOrEq, Swap, IsEmpty, ShiftUp, ShiftDown)
        
        private int CompareItems(T a, T b)
        {
            if (_comparer != null) return _comparer.Compare(a, b);
            return a.CompareTo(b);
        }

        
        private bool GreaterOrEq(int i, int j)
        {
            return CompareItems(_items[i], _items[j]) >= 0;
        }

        
        private bool GreaterOrEq(T a, T b)
        {
            return CompareItems(a, b) >= 0;
        }


        
        private void Swap(int i, int j)
        {
            T temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;
        }

        
        private bool IsEmpty() => Count == 0;

        
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
        #endregion
        
        #region Public operations (ExtractMax, Peek, IncreaseKey, Print, Remove, Insert, Contains, Clear, ToArray)
        
        public T ExtractMax()
        {
            if (IsEmpty()) throw new Exception("Heap is empty");
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
            if (IsEmpty()) throw new Exception("Heap is empty");
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

        
        public bool Remove(T item)
        {
            int index = _items.IndexOf(item);
            if (index < 0) return false;

            int last = Count - 1;
            if (index != last)
            {
                Swap(index, last);
                _items.RemoveAt(last);
                if (index < Count)
                {
                    if (index > 0 && GreaterOrEq(item, _items[(index - 1) / 2]))
                        ShiftUp(index);
                    else
                        ShiftDown(index);
                }
            }
            else
            {
                _items.RemoveAt(last);
            }
            return true;
        }

        
        public void Insert(T value)
        {
            _items.Add(value);
            ShiftUp(Count - 1);
        }

        
        public bool Contains(T item) => _items.Contains(item);

        
        public void Clear() => _items.Clear();

        
        public T[] ToArray() => _items.ToArray();
        #endregion
    }
}