using System;

namespace Task8.Collections
{
    class MyArrayList<T>
    {
        #region Поля
        T[] elementData;
        private int size;
        const int DEFAULT_CAPACITY = 10;
        #endregion

        #region Конструкторы
        public MyArrayList()
        {
            elementData = new T[DEFAULT_CAPACITY];
            size = 0;
        }

        public MyArrayList(T[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException("Array cannot be null");
            elementData = new T[arr.Length];
            Array.Copy(arr, elementData, arr.Length);
            size = arr.Length;
        }

        public MyArrayList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Capacity must be non-negative");
            elementData = new T[capacity];
            size = 0;
        }
        #endregion

        #region Вспомогательные методы (приватные)
        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elementData.Length)
            {
                int newCapacity = (elementData.Length * 3) / 2 + 1;
                if (newCapacity < minCapacity)
                    newCapacity = minCapacity;
                T[] newArray = new T[newCapacity];
                Array.Copy(elementData, newArray, size);
                elementData = newArray;
            }
        }

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
        #endregion

        #region Базовые методы (Add, Clear, Contains, Remove)
        public void Add(T elem)
        {
            EnsureCapacity(size + 1);
            elementData[size++] = elem;
        }

        public void AddAll(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("Array cannot be null");
            if (array.Length == 0)
                return;
            EnsureCapacity(size + array.Length);
            Array.Copy(array, 0, elementData, size, array.Length);
            size += array.Length;
        }

        public void Clear()
        {
            for (int i = 0; i < size; i++)
            {
                elementData[i] = default(T);
            }
            size = 0;
        }

        public bool Contains(Object o)
        {
            return IndexOf(o) >= 0;
        }

        public bool ContainsAll(Object[] a)
        {
            if (a == null)
                throw new ArgumentNullException("Array cannot be null");
            foreach (var item in a)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        public bool IsEmpty() => size == 0;

        public bool Remove(Object obj)
        {
            int index = IndexOf(obj);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAll(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("Array cannot be null");
            if (array.Length == 0)
                return;
            foreach (var item in array)
            {
                while (Remove(item)) { }
            }
        }

        public void RetainAll(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("Array cannot be null");

            for (int i = size - 1; i >= 0; i--)
            {
                bool found = false;
                foreach (var item in array)
                {
                    if ((elementData[i] == null && item == null) || (elementData[i] != null && elementData[i].Equals(item)))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    RemoveAt(i);
                }
            }
        }
        #endregion

        #region Запросы и преобразования (Size, ToArray, Get, Set, Index)
        public int Size() => size;

        public Object[] ToArray()
        {
            Object[] result = new Object[size];
            Array.Copy(elementData, result, size);
            return result;
        }

        public T[] ToArray(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("Array cannot be null");
            if (array.Length < size)
            {
                T[] newArray = new T[size];
                Array.Copy(elementData, newArray, size);
                return newArray;
            }
            Array.Copy(elementData, array, size);
            if (array.Length > size)
                array[size] = default(T);
            return array;
        }

        public T Get(int index)
        {
            CheckIndex(index);
            return elementData[index];
        }

        public T Set(int index, T elem)
        {
            CheckIndex(index);
            T oldValue = elementData[index];
            elementData[index] = elem;
            return oldValue;
        }

        public int IndexOf(Object o)
        {
            for (int i = 0; i < size; i++)
            {
                if ((o == null && elementData[i] == null) || (o != null && elementData[i] != null && elementData[i].Equals(o)))
                    return i;
            }
            return -1;
        }

        public int LastIndexOf(Object o)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if ((o == null && elementData[i] == null) || (o != null && elementData[i] != null && elementData[i].Equals(o)))
                    return i;
            }
            return -1;
        }

        public T RemoveAt(int index)
        {
            CheckIndex(index);
            T oldValue = elementData[index];
            for (int j = index; j < size - 1; j++)
            {
                elementData[j] = elementData[j + 1];
            }
            elementData[size - 1] = default(T);
            size--;
            return oldValue;
        }

        public MyArrayList<T> SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
                throw new ArgumentOutOfRangeException("Invalid index range");

            MyArrayList<T> subList = new MyArrayList<T>(toIndex - fromIndex);
            for (int i = fromIndex; i < toIndex; i++)
            {
                subList.Add(elementData[i]);
            }
            return subList;
        }
        #endregion

        #region Методы с индексами (Add, AddAll)
        public void Add(int index, T elem)
        {
            CheckIndexForAdd(index);
            EnsureCapacity(size + 1);
            for (int i = size; i > index; i--)
            {
                elementData[i] = elementData[i - 1];
            }
            elementData[index] = elem;
            size++;
        }

        public void AddAll(int index, T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("Array cannot be null");

            CheckIndexForAdd(index);

            if (array.Length == 0)
                return;

            EnsureCapacity(size + array.Length);

            for (int i = size - 1; i >= index; i--)
            {
                elementData[i + array.Length] = elementData[i];
            }

            Array.Copy(array, 0, elementData, index, array.Length);
            size += array.Length;
        }
        #endregion
    }
}


