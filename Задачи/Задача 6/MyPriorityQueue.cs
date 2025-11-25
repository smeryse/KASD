﻿using System;
using System.Collections.Generic;
using Task5.Collections;

namespace Task6.Collections
{
    public class MyPriorityQueue<T> where T : IComparable<T>
    {
        private MaxHeap<T> heap;

        public MyPriorityQueue()
        {
            heap = new MaxHeap<T>();
        }

        public MyPriorityQueue(IEnumerable<T> items)
        {
            heap = new MaxHeap<T>(items);
        }

        public int Size() => heap.Count;

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            heap.Insert(item);
        }

        public bool Offer(T item)
        {
            Add(item);
            return true;
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

        public void Clear() => heap.Clear();

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
    }
}
