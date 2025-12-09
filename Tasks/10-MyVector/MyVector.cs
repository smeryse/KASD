// FIXME: ArgumentNullException.ThrowIfNull(arr);
namespace Task10.Collection
{
    public class MyVector<T>
    {
        public T[] elementData;
        private int elementCount;
        private int capacityIncrement;
        private const int DEFAULT_CAPACITY = 10;
        private const int DEFAULT_INCREMENT = 0;

        public MyVector(int initialCapacity, int capacityIncrement) 
        {
            elementData = new T[initialCapacity];
            this.capacityIncrement = capacityIncrement;
        }

        public MyVector(int initialCapacity) 
        {
            elementData = new T[initialCapacity];
            this.capacityIncrement = DEFAULT_INCREMENT;
        }

        public MyVector() 
        {
            elementData = new T[DEFAULT_CAPACITY];
            this.capacityIncrement = DEFAULT_INCREMENT;
        }

        public MyVector(T[] arr)
        /*
            MyVector(T[] a) ->  MyVector(T[] arr)
        */
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));
            
            elementData = new T[arr.Length];
            capacityIncrement = DEFAULT_INCREMENT;
            
            arr.CopyTo(elementData, 0);
            elementCount = arr.Length;
        }

        public int Size() => elementCount;
        public bool IsEmpty() => elementCount == 0;

        public void Add(T elem) 
        /*
            add(T e) -> add(T elem)
        */
        {
            EnsureCapacity(elementData.Length + 1);
            elementData[elementCount++] = elem;
        }

        public void Add(int index, T elem)
        /*
            Add(int  index,  T  e) -> Add(int  index,  T  elem)
        */
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            EnsureCapacity(elementData.Length + 1);
            
            for (int i = elementCount; i > index; i--)
                elementData[i] = elementData[i-1];
            elementData[index] = elem;
            elementCount++;
        }

        public void AddAll(T[] arr)
        /*
            T[] a -> T[] arr
        */
        { 
            if (arr == null) 
                throw new ArgumentNullException(nameof(arr));

            for (int i = 0; i < arr.Length; i++)
                Add(arr[i]);      
        }

        public void AddAll(int index, T[] arr)
        /*
            AddAll(int index, T[] a) -> AddAll(int index, T[] arr)
        */
        {
            if (arr == null) 
                throw new ArgumentNullException(nameof(arr));
            
            for (int i = 0; i < arr.Length; i++)
                Add(index + i, arr[i]);      
        }

        public bool Remove(object obj)
        /*
            Remove(object o) -> Remove(object obj)
            Return true if removed, else false
        */
        {
        bool removedAny = false;

        int index;
        while ((index = IndexOf(obj)) != -1)
        {
            RemoveAt(index);
            removedAny = true;
        }

        return removedAny;
        }

        public T RemoveAt(int index)
        /*
        Merge Remove(int index)
        Rename: RemoveElementAt -> RemoveAt
        Rename: pos -> index
        Return type "T" (before void)
        */
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException(nameof(index));
    
            T removed = elementData[index];
            for (int i = index; i < elementCount - 1; i++)
                elementData[i] = elementData[i+1];

            elementCount--;
            return removed;
        }

        public bool RemoveAll(T[] arr)
        /*
            RemoveAll(T[] a) -> RemoveAll(T[] arr)
            Return if all removed, else false
        */
        {
            if (arr == null) 
                throw new ArgumentNullException(nameof(arr));

            bool allRemoved = true;
            foreach (T obj in arr)
            {
                bool removed = Remove(obj);
                if (!removed)
                    allRemoved = false;
            }
            return allRemoved;            
        }

        public void RemoveRange(int begin, int end)
        /*
            Remove inderval - [begin; end]
            Return array of removed elements
        */
        {
            for (int i = begin; i <= end; i++)
                RemoveAt(begin);
        }

        public void Clear()
        /*
            Delete link on elements for garbage collector
        */
        {
            elementCount = 0;
            Array.Clear(elementData, 0, elementCount);
        }

        public T Get(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new IndexOutOfRangeException($"Index {index} is out of range.");

            return elementData[index];
        }

        public void Set(int index, T elem)
        /*
            Set(int index, T e) -> Set(int index, T elem)
        */
        {
            if (index < 0 || index >= elementCount)
                throw new IndexOutOfRangeException($"Index {index} is out of range.");
            
            elementData[index] = elem;
        }

        public T FirstElement() => elementData[0];

        public T LastElement() => elementData[elementCount - 1];

        public T[] SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
                throw new IndexOutOfRangeException(
                    $"Invalid range: fromIndex {fromIndex}, toIndex {toIndex}, elementCount {elementCount}"
                );

            int count = toIndex - fromIndex;
            T[] arr = new T[count];

            for (int i = 0; i < count; i++)
                arr[i] = elementData[fromIndex + i];

            return arr;
        }

        public int IndexOf(object obj)
        /*
            IndexOf(object o) -> IndexOf(object obj)
            Return -1 if not find
        */
        {
            for (int i = 0; i < elementCount; i++)
            {
                if(Equals(obj, elementData[i]))
                    return i;
            }
            return -1;
        }

        public int LastIndexOf(object obj)
        /*
            LastIndexOf(object o) -> LastIndexOf(object obj)
        */
        {
            for (int i = elementCount - 1; i >= 0; i--)
            {
                if(Equals(obj, elementData[i]))
                    return i;
            }
            return -1;
        }

        public bool Contains(object obj)
        /*
            LastIndexOf(object o) -> LastIndexOf(object obj)
        */
        {
            for (int i = 0; i < elementCount; i++)
            {
                if(Equals(elementData[i], obj))
                    return true;
            }
            return false;
        }

        public bool ContainsAll(T[] arr)
        /*
            ContainsAll(T[] a) -> ContainsAll(T[] arr)
        */
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));
            
            foreach (object obj in arr)
            {
                if (!Contains(obj))
                    return false;
            }
            return true;
        }

        public void RetainAll(T[] arr)
        /*
            RetainAll(T[] a) -> RetainAll(T[] arr)
        */
        {
            if (arr == null) 
                throw new ArgumentNullException(nameof(arr));

            int newCount = 0;

            for (int i = 0; i < elementCount; i++)
            {
                if (ContainsInArray(elementData[i], arr))
                {
                    elementData[newCount] = elementData[i];
                    newCount++;
                }
            }
            Array.Clear(elementData, newCount, elementCount - newCount);
            elementCount = newCount;
        }

        public T[] ToArray()
        /*
            ??????
        */
        {
            T[] arr = new T[elementCount];
            for (int i = 0; i < elementCount; i++)
                arr[i] = elementData[i];
            return arr;
        }

        public void ToArray(T[] arr)
        /*
            ?????
            ToArray(T[] a) -> ToArray(T[] arr)
        */
        {
            if (arr == null || arr.Length < elementCount)
                arr = new T[elementCount];

            for (int i = 0; i < elementCount; i++)
                arr[i] = elementData[i];
        }

        public void Print()
        {
            for (int i = 0; i < elementCount; i++)
            {
                Console.Write(elementData[i] + " ");
            }
            Console.WriteLine();
        }

        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elementData.Length)
            { 
                int newCapacity = (capacityIncrement == 0) 
                    ? elementData.Length * 2 
                    : elementData.Length + capacityIncrement;

                if (newCapacity < minCapacity)
                    newCapacity = minCapacity;

                T[] newData = new T[newCapacity];
                elementData.CopyTo(newData, 0);
                elementData = newData;
            }
        }
        private bool ContainsInArray(T item, T[] array) => Array.IndexOf(array, item) >= 0;
    }
}