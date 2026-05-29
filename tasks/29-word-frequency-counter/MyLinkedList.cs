using System;

namespace Task29.Collections
{
    public class MyLinkedList<T>
    {
        #region Fields
        private Node? head;
        private Node? tail;
        private int size;
        #endregion

        #region Nested Node class
        private class Node
        {
            public T data;
            public Node? prev;
            public Node? next;

            public Node(T data)
            {
                this.data = data;
                prev = null;
                next = null;
            }
        }
        #endregion

        #region Constructors
        public MyLinkedList()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public MyLinkedList(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            head = null;
            tail = null;
            size = 0;

            for (int i = 0; i < a.Length; i++)
            {
                Add(a[i]);
            }
        }
        #endregion

        #region Private helper methods
        private void CheckIndex(int index)
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException("Index out of bounds");
        }

        private void CheckIndexForAdd(int index)
        {
            if (index < 0 || index > size)
                throw new ArgumentOutOfRangeException("Index out of bounds");
        }

        private Node GetNode(int index)
        {
            CheckIndex(index);

            Node current;
            if (index < size / 2)
            {
                current = head!;
                for (int i = 0; i < index; i++)
                    current = current.next!;
            }
            else
            {
                current = tail!;
                for (int i = size - 1; i > index; i--)
                    current = current.prev!;
            }
            return current;
        }

        private void RemoveNode(Node node)
        {
            Node? prevNode = node.prev;
            Node? nextNode = node.next;

            if (prevNode == null)
                head = nextNode;
            else
                prevNode.next = nextNode;

            if (nextNode == null)
                tail = prevNode;
            else
                nextNode.prev = prevNode;

            size--;
        }
        #endregion

        #region Public operations
        public void Add(T e)
        {
            Node newNode = new Node(e);
            if (tail == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.next = newNode;
                newNode.prev = tail;
                tail = newNode;
            }
            size++;
        }

        public void Add(int index, T e)
        {
            CheckIndexForAdd(index);

            if (index == size)
            {
                Add(e);
                return;
            }

            Node newNode = new Node(e);
            Node current = GetNode(index);
            Node? prevNode = current.prev;

            newNode.next = current;
            newNode.prev = prevNode;
            current.prev = newNode;

            if (prevNode == null)
                head = newNode;
            else
                prevNode.next = newNode;

            size++;
        }

        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            for (int i = 0; i < a.Length; i++)
            {
                Add(a[i]);
            }
        }

        public void AddAll(int index, T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            for (int i = a.Length - 1; i >= 0; i--)
            {
                Add(index, a[i]);
            }
        }

        public void Clear()
        {
            head = null;
            tail = null;
            size = 0;
        }

        public bool Contains(object o)
        {
            return IndexOf(o) >= 0;
        }

        public bool ContainsAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            foreach (T item in a)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public T Get(int index)
        {
            return GetNode(index).data;
        }

        public int IndexOf(object o)
        {
            Node current = head;
            int index = 0;
            while (current != null)
            {
                if ((o == null && current.data == null) || (o != null && current.data != null && current.data.Equals(o)))
                    return index;
                current = current.next;
                index++;
            }
            return -1;
        }

        public int LastIndexOf(object o)
        {
            Node current = tail;
            int index = size - 1;
            while (current != null)
            {
                if ((o == null && current.data == null) || (o != null && current.data != null && current.data.Equals(o)))
                    return index;
                current = current.prev;
                index--;
            }
            return -1;
        }

        public bool Remove(object o)
        {
            Node current = head;
            while (current != null)
            {
                if ((o == null && current.data == null) || (o != null && current.data != null && current.data.Equals(o)))
                {
                    RemoveNode(current);
                    return true;
                }
                current = current.next;
            }
            return false;
        }

        public void RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            foreach (T item in a)
            {
                while (Remove(item)) { }
            }
        }

        public void RetainAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            Node current = head;
            while (current != null)
            {
                Node next = current.next;
                bool found = false;
                foreach (T item in a)
                {
                    if ((current.data == null && item == null) || (current.data != null && current.data.Equals(item)))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    RemoveNode(current);
                }
                current = next;
            }
        }

        public T RemoveAt(int index)
        {
            Node node = GetNode(index);
            T data = node.data;
            RemoveNode(node);
            return data;
        }

        public T Set(int index, T e)
        {
            Node node = GetNode(index);
            T oldValue = node.data;
            node.data = e;
            return oldValue;
        }

        public int Size()
        {
            return size;
        }

        public MyLinkedList<T> SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
                throw new ArgumentOutOfRangeException("Invalid index range");

            MyLinkedList<T> subList = new MyLinkedList<T>();
            Node current = head;
            int index = 0;
            while (current != null && index < toIndex)
            {
                if (index >= fromIndex)
                {
                    subList.Add(current.data);
                }
                current = current.next;
                index++;
            }
            return subList;
        }

        public object[] ToArray()
        {
            object[] result = new object[size];
            Node current = head;
            int index = 0;
            while (current != null)
            {
                result[index] = current.data!;
                current = current.next;
                index++;
            }
            return result;
        }

        public T[] ToArray(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");

            if (a.Length < size)
            {
                a = new T[size];
            }

            Node current = head;
            int index = 0;
            while (current != null)
            {
                a[index] = current.data;
                current = current.next;
                index++;
            }

            if (a.Length > size)
                a[size] = default(T);

            return a;
        }

        public T First()
        {
            if (head == null)
                throw new InvalidOperationException("List is empty");
            return head.data;
        }

        public T Last()
        {
            if (tail == null)
                throw new InvalidOperationException("List is empty");
            return tail.data;
        }
        #endregion
    }
}
