﻿using System;
using System.Collections.Generic;
using Task5.Collections;

namespace Task6.Collections
{
    public class MyPriorityQueue<T> where T : IComparable<T>
    {
        #region Fields
        private MaxHeap<T> heap;
        #endregion

        #region Constructors
        // 1) Default constructor: initial capacity 11, natural ordering
        public MyPriorityQueue()
        {
            heap = new MaxHeap<T>(11);
        }

        // 2) Constructor from IEnumerable (preserves existing behavior)
        public MyPriorityQueue(IEnumerable<T> items)
        {
            heap = new MaxHeap<T>(items);
        }

        // 2b) Constructor from array
        public MyPriorityQueue(T[] a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            heap = new MaxHeap<T>(a);
        }

        // 3) Constructor with initial capacity
        public MyPriorityQueue(int initialCapacity)
        {
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            heap = new MaxHeap<T>(initialCapacity);
        }

        // 4) Constructor with initial capacity and comparer
        public MyPriorityQueue(int initialCapacity, IComparer<T> comparer)
        {
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            heap = new MaxHeap<T>(initialCapacity, comparer);
        }

        // 5) Copy constructor from another MyPriorityQueue
        public MyPriorityQueue(MyPriorityQueue<T> c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            heap = new MaxHeap<T>(c.ToArray());
        }
        #endregion
        
        #region Core methods (Size, Add, Offer, AddAll, Peek, Poll)
        // Returns the number of elements in the queue
        public int Size() => heap.Count;

        // Adds an element to the queue (throws ArgumentNullException for null)
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            heap.Insert(item);
        }

        // Add element, returns true if successful, false otherwise
        public bool Offer(T item)
        {
            try
            {
                Add(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Adds all items from an array
        public void AddAll(T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var it in items) Add(it);
        }

        // Returns the maximum element without removing it (or default on error)
        public T Peek()
        {
            try { return heap.Peek(); }
            catch { return default(T); }
        }

        // Extracts and returns the maximum element (or default on error)
        public T Poll()
        {
            try { return heap.ExtractMax(); }
            catch { return default(T); }
        }
        #endregion

        #region Modification (Remove, RemoveAll, RetainAll, Clear)
        // Removes the specified element; returns true if removed
        public bool Remove(object item)
        {
            if (item == null) return false;
            if (item is T t)
                return heap.Remove(t);
            return false;
        }

        // Removes all elements from the given array; returns true if all removed
        public bool RemoveAll(T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            bool allRemoved = true;
            foreach (var it in items)
            {
                if (!heap.Remove(it)) allRemoved = false;
            }
            return allRemoved;
        }

        // Retains only the elements contained in the provided array
        public void RetainAll(T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var set = new HashSet<T>(items);
            var arr = heap.ToArray();
            foreach (var v in arr)
            {
                if (!set.Contains(v)) heap.Remove(v);
            }
        }

        // Clears the queue
        public void Clear() => heap.Clear();
        #endregion

        #region Queries and Conversions (Contains, ContainsAll, ToArray)
        // Checks whether the queue contains the specified element
        public bool Contains(object item)
        {
            if (item == null) return false;
            if (item is T t) return heap.Contains(t);
            return false;
        }

        // Checks whether all elements from the array are contained in the queue
        public bool ContainsAll(object[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var it in items)
            {
                if (!Contains(it)) return false;
            }
            return true;
        }

        // Returns an array with the elements of the queue
        public T[] ToArray() => heap.ToArray();

        // Copies elements into the provided array or creates a new one
        public T[] ToArray(T[] a)
        {
            var arr = ToArray();
            if (a == null || a.Length < arr.Length) a = new T[arr.Length];
            Array.Copy(arr, a, arr.Length);
            if (a.Length > arr.Length)
                for (int i = arr.Length; i < a.Length; i++) a[i] = default(T);
            return a;
        }
        #endregion
    }
}
