namespace Task14
{
    public class MyArrayDeque<T>
    {
        private T[] elements;
        private int head;
        private int tail;
        private const int DEFAULT_CAPACITY = 16;

        public MyArrayDeque()
        {
            elements = new T[DEFAULT_CAPACITY];
            head = 0;
            tail = 0;
        }

        public MyArrayDeque(T[] a)
        {
            elements = new T[a.Length];
            head = 0;
            tail = 0;
            AddAll(a);
        }

        public MyArrayDeque(int numElements)
        {
            elements = new T[numElements];
            head = 0;
            tail = 0;
        }

        public void Add(T e)
        {
            EnsureCapacity();
            elements[tail] = e;
            tail = (tail + 1) % elements.Length;
        }

        public void AddAll(T[] a)
        {
            foreach (T item in a)
                Add(item);
        }

        public void Clear()
        {
            for (int i = 0; i < elements.Length; i++)
                elements[i] = default;
            head = 0;
            tail = 0;
        }

        public bool Contains(object o)
        {
            int i = head;
            while (i != tail)
            {
                if (Equals(elements[i], o))
                    return true;
                i = (i + 1) % elements.Length;
            }
            return false;
        }

        public bool ContainsAll(T[] a)
        {
            foreach (T item in a)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        public bool IsEmpty() => head == tail;

        public bool Remove(object o)
        {
            int i = head;
            while (i != tail)
            {
                if (Equals(elements[i], o))
                {
                    int j = i;
                    while (j != tail)
                    {
                        int next = (j + 1) % elements.Length;
                        elements[j] = elements[next];
                        j = next;
                    }
                    tail = (tail - 1 + elements.Length) % elements.Length;
                    return true;
                }
                i = (i + 1) % elements.Length;
            }
            return false;
        }

        public bool RemoveAll(T[] a)
        {
            bool changed = false;
            foreach (T item in a)
            {
                while (Remove(item))
                    changed = true;
            }
            return changed;
        }

        public bool RetainAll(T[] a)
        {
            bool changed = false;
            int i = head;
            while (i != tail)
            {
                if (!ContainsInArray(elements[i], a))
                {
                    int j = i;
                    while (j != tail)
                    {
                        int next = (j + 1) % elements.Length;
                        elements[j] = elements[next];
                        j = next;
                    }
                    tail = (tail - 1 + elements.Length) % elements.Length;
                    changed = true;
                }
                else
                {
                    i = (i + 1) % elements.Length;
                }
            }
            return changed;
        }

        public int Size()
        {
            return (tail - head + elements.Length) % elements.Length;
        }

        private void EnsureCapacity()
        {
            if ((tail + 1) % elements.Length == head)
            {
                int newCapacity = elements.Length * 2;
                T[] newElements = new T[newCapacity];
                int size = Size();
                for (int i = 0; i < size; i++)
                {
                    newElements[i] = elements[(head + i) % elements.Length];
                }
                elements = newElements;
                head = 0;
                tail = size;
            }
        }

        private static bool ContainsInArray(T item, T[] array)
        {
            return Array.IndexOf(array, item) >= 0;
        }
    }
}