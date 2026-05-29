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
        
        public MyPriorityQueue()
        {
            heap = new MaxHeap<T>(11);
        }

        
        public MyPriorityQueue(IEnumerable<T> items)
        {
            heap = new MaxHeap<T>(items);
        }

        
        public MyPriorityQueue(T[] a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            heap = new MaxHeap<T>(a);
        }

        
        public MyPriorityQueue(int initialCapacity)
        {
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            heap = new MaxHeap<T>(initialCapacity);
        }

        
        public MyPriorityQueue(int initialCapacity, IComparer<T> comparer)
        {
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            heap = new MaxHeap<T>(initialCapacity, comparer);
        }

        
        public MyPriorityQueue(MyPriorityQueue<T> c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            heap = new MaxHeap<T>(c.ToArray());
        }
        #endregion
        
        #region Core methods (Size, Add, Offer, AddAll, Peek, Poll)
        
        public int Size() => heap.Count;

        
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            heap.Insert(item);
        }

        
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

        
        public void AddAll(T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var it in items) Add(it);
        }

        
        public T Peek()
        {
            try { return heap.Peek(); }
            catch { return default(T); }
        }

        
        public T Poll()
        {
            try { return heap.ExtractMax(); }
            catch { return default(T); }
        }
        #endregion

        #region Modification (Remove, RemoveAll, RetainAll, Clear)
        
        public bool Remove(object item)
        {
            if (item == null) return false;
            if (item is T t)
                return heap.Remove(t);
            return false;
        }

        
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

        
        public void Clear() => heap.Clear();
        #endregion

        #region Queries and Conversions (Contains, ContainsAll, ToArray)
        
        public bool Contains(object item)
        {
            if (item == null) return false;
            if (item is T t) return heap.Contains(t);
            return false;
        }

        
        public bool ContainsAll(object[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var it in items)
            {
                if (!Contains(it)) return false;
            }
            return true;
        }

        
        public T[] ToArray() => heap.ToArray();

        
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
