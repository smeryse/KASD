using System;
using System.Collections.Generic;
using Task6.Collections;
using Task8.Collections;
using Task10.Collection;
using Task19.Collection;
using Task25;
using Task29.Collections;

namespace Task28
{
    public class MyPriorityQueueIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private readonly MyPriorityQueue<T> queue;
        private readonly List<T> snapshot;
        private int cursor;
        private int lastRet = -1;

        public MyPriorityQueueIterator(MyPriorityQueue<T> queue)
        {
            this.queue = queue;
            this.snapshot = new List<T>(queue.ToArray());
            this.cursor = 0;
        }

        public bool HasNext() => cursor < snapshot.Count;

        public T Next()
        {
            if (cursor >= snapshot.Count)
                throw new MyNoSuchElementException("No more elements in priority queue");
            lastRet = cursor++;
            return snapshot[lastRet];
        }

        public void Remove()
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to remove - call Next() first");
            T element = snapshot[lastRet];
            queue.Remove(element);
            snapshot.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }

    public class MyHashSetIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private readonly MyHashSet<T> set;
        private readonly List<T> snapshot;
        private int cursor;
        private int lastRet = -1;

        public MyHashSetIterator(MyHashSet<T> set)
        {
            this.set = set;
            this.snapshot = new List<T>();
            foreach (var obj in set.ToArray())
                if (obj is T t) snapshot.Add(t);
            this.cursor = 0;
        }

        public bool HasNext() => cursor < snapshot.Count;

        public T Next()
        {
            if (cursor >= snapshot.Count)
                throw new MyNoSuchElementException("No more elements in hash set");
            lastRet = cursor++;
            return snapshot[lastRet];
        }

        public void Remove()
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to remove - call Next() first");
            set.Remove(snapshot[lastRet]);
            snapshot.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }

    public class MyTreeSetIterator<T> : IMyIterator<T> where T : IComparable<T>
    {
        private readonly MyTreeSet<T> treeSet;
        private readonly List<T> snapshot;
        private int cursor;
        private int lastRet = -1;

        public MyTreeSetIterator(MyTreeSet<T> treeSet)
        {
            this.treeSet = treeSet;
            this.snapshot = new List<T>(treeSet.ToArray());
            this.cursor = 0;
        }

        public bool HasNext() => cursor < snapshot.Count;

        public T Next()
        {
            if (cursor >= snapshot.Count)
                throw new MyNoSuchElementException("No more elements in tree set");
            lastRet = cursor++;
            return snapshot[lastRet];
        }

        public void Remove()
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to remove - call Next() first");
            treeSet.Remove(snapshot[lastRet]);
            snapshot.RemoveAt(lastRet);
            cursor = lastRet;
            lastRet = -1;
        }
    }

    public class MyArrayListIterator<T> : IMyListIterator<T>
    {
        private readonly MyArrayList<T> list;
        private int cursor;
        private int lastRet = -1;

        public MyArrayListIterator(MyArrayList<T> list) { this.list = list; this.cursor = 0; }
        public MyArrayListIterator(MyArrayList<T> list, int index)
        {
            this.list = list;
            if (index < 0 || index > list.Size())
                throw new ArgumentOutOfRangeException(nameof(index));
            this.cursor = index;
        }

        public bool HasNext() => cursor < list.Size();
        public T Next()
        {
            if (cursor >= list.Size())
                throw new MyNoSuchElementException("No more elements");
            lastRet = cursor++;
            return list.Get(lastRet);
        }
        public bool HasPrevious() => cursor > 0;
        public T Previous()
        {
            if (cursor <= 0)
                throw new MyNoSuchElementException("No previous element");
            lastRet = --cursor;
            return list.Get(lastRet);
        }
        public int NextIndex() => cursor;
        public int PreviousIndex() => cursor - 1;
        public void Remove()
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to remove - call Next() or Previous() first");
            list.RemoveAt(lastRet);
            if (cursor > lastRet) cursor--;
            lastRet = -1;
        }
        public void Set(T element)
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to set - call Next() or Previous() first");
            list.Set(lastRet, element);
        }
        public void Add(T element)
        {
            list.Add(cursor, element);
            cursor++;
            lastRet = -1;
        }
    }

    public class MyVectorIterator<T> : IMyListIterator<T>
    {
        private readonly MyVector<T> vector;
        private int cursor;
        private int lastRet = -1;

        public MyVectorIterator(MyVector<T> vector) { this.vector = vector; this.cursor = 0; }
        public MyVectorIterator(MyVector<T> vector, int index)
        {
            this.vector = vector;
            if (index < 0 || index > vector.Size())
                throw new ArgumentOutOfRangeException(nameof(index));
            this.cursor = index;
        }

        public bool HasNext() => cursor < vector.Size();
        public T Next()
        {
            if (cursor >= vector.Size())
                throw new MyNoSuchElementException("No more elements");
            lastRet = cursor++;
            return vector.Get(lastRet);
        }
        public bool HasPrevious() => cursor > 0;
        public T Previous()
        {
            if (cursor <= 0)
                throw new MyNoSuchElementException("No previous element");
            lastRet = --cursor;
            return vector.Get(lastRet);
        }
        public int NextIndex() => cursor;
        public int PreviousIndex() => cursor - 1;
        public void Remove()
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to remove - call Next() or Previous() first");
            vector.RemoveAt(lastRet);
            if (cursor > lastRet) cursor--;
            lastRet = -1;
        }
        public void Set(T element)
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to set - call Next() or Previous() first");
            vector.Set(lastRet, element);
        }
        public void Add(T element)
        {
            vector.Add(cursor, element);
            cursor++;
            lastRet = -1;
        }
    }

    public class MyLinkedListIterator<T> : IMyListIterator<T>
    {
        private readonly MyLinkedList<T> list;
        private int cursor;
        private int lastRet = -1;

        public MyLinkedListIterator(MyLinkedList<T> list) { this.list = list; this.cursor = 0; }
        public MyLinkedListIterator(MyLinkedList<T> list, int index)
        {
            this.list = list;
            if (index < 0 || index > list.Size())
                throw new ArgumentOutOfRangeException(nameof(index));
            this.cursor = index;
        }

        public bool HasNext() => cursor < list.Size();
        public T Next()
        {
            if (cursor >= list.Size())
                throw new MyNoSuchElementException("No more elements");
            lastRet = cursor++;
            return list.Get(lastRet);
        }
        public bool HasPrevious() => cursor > 0;
        public T Previous()
        {
            if (cursor <= 0)
                throw new MyNoSuchElementException("No previous element");
            lastRet = --cursor;
            return list.Get(lastRet);
        }
        public int NextIndex() => cursor;
        public int PreviousIndex() => cursor - 1;
        public void Remove()
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to remove");
            list.RemoveAt(lastRet);
            if (cursor > lastRet) cursor--;
            lastRet = -1;
        }
        public void Set(T element)
        {
            if (lastRet < 0)
                throw new MyIllegalStateException("No element to set");
            list.Set(lastRet, element);
        }
        public void Add(T element)
        {
            list.Add(cursor, element);
            cursor++;
            lastRet = -1;
        }
    }
}
