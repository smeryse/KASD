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
        // Default constructor
        public MaxHeap() { }

        // Constructor with initial capacity
        public MaxHeap(int capacity)
        {
            _items = new List<T>(capacity);
        }

        // Constructor with initial capacity and comparer
        public MaxHeap(int capacity, IComparer<T> comparer)
        {
            _items = new List<T>(capacity);
            _comparer = comparer;
        }

        // Constructor from enumerable
        public MaxHeap(IEnumerable<T> values)
        {
            if (values != null)
            {
                _items.AddRange(values);
                for (int i = (Count / 2) - 1; i >= 0; i--)
                    ShiftDown(i);
            }

        }
        // Constructor from enumerable with comparer
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
        // Comparing two elements using a comparator
        private int CompareItems(T a, T b)
        {
            if (_comparer != null) return _comparer.Compare(a, b);
            return a.CompareTo(b);
        }

        // Returns true if item at index i is greater than or equal to item at index j
        private bool GreaterOrEq(int i, int j)
        {
            return CompareItems(_items[i], _items[j]) >= 0;
        }

        // Returns true if item a is greater than or equal to item b
        private bool GreaterOrEq(T a, T b)
        {
            return CompareItems(a, b) >= 0;
        }


        // Swaps two elements in the heap
        private void Swap(int i, int j)
        {
            T temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;
        }

        // Checks if the heap is empty
        private bool IsEmpty() => Count == 0;

        // Restores the heap property by shifting an element up
        private void ShiftUp(int current)
        {
            while (current > 0)
            {
                int parent = (current - 1) / 2;
                if (GreaterOrEq(parent, current))
                    break; // heap property restored
                else
                {
                    // move current element up
                    Swap(current, parent);
                    current = parent;
                }
            }
        }

        // Restores the heap property by shifting an element down
        private void ShiftDown(int index)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;

            while (left < Count)
            {
                int current;

                // determine the larger child
                if (right >= Count) current = left;
                else current = GreaterOrEq(right, left) ? right : left;

                // check heap condition
                if (GreaterOrEq(index, current)) return;
                else
                {
                    // swap current and larger child
                    Swap(index, current);
                    index = current;
                    left = index * 2 + 1;
                    right = index * 2 + 2;
                }
            }
        }
        #endregion
        
        #region Public operations (ExtractMax, Peek, IncreaseKey, Print, Remove, Insert, Contains, Clear, ToArray)
        // Extracts and returns the maximum element from the heap
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

        // Returns the maximum element without removing it
        public T Peek()
        {
            if (IsEmpty()) throw new Exception("Heap is empty");
            else return _items[0];
        }

        // Updates the value at given index and restores heap property
        public void IncreaseKey(int index, T newValue)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException();

            T oldValue = _items[index];
            _items[index] = newValue;

            if (GreaterOrEq(newValue, oldValue)) ShiftUp(index);
            else ShiftDown(index);
        }

        // Prints heap contents in a tree-like layout
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

        // Removes the first occurrence of the specified item
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

        // Inserts a new element into the heap
        public void Insert(T value)
        {
            _items.Add(value);
            ShiftUp(Count - 1);
        }

        // Checks whether the heap contains the specified item
        public bool Contains(T item) => _items.Contains(item);

        // Removes all elements from the heap
        public void Clear() => _items.Clear();

        // Returns heap elements as an array
        public T[] ToArray() => _items.ToArray();
        #endregion
    }
}