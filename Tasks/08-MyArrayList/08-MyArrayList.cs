class MyArrayList<T>
{
    T[] elementData;
    private int size;
    const int DEFAULT_CAPACITY = 10;

    // ====== Конструкторы ======
    // Конструктор по умолчанию
    public MyArrayList()
    {
        elementData = new T[DEFAULT_CAPACITY];
        size = 0;
    }

    // Конструктор с инициализацией из массива
    public MyArrayList(T[] arr)
    {
        if (arr == null)
            throw new ArgumentNullException("Array cannot be null");
        elementData = new T[arr.Length];
        Array.Copy(arr, elementData, arr.Length);
        size = arr.Length;
    }

    // Конструктор с указанием начальной емкости
    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException("Capacity must be non-negative");
        elementData = new T[capacity];
        size = 0;
    }


    // ====== Вспомогательные методы ======
    // Увеличение емкости внутреннего массива
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

    // Проверка индекса
    private void CheckIndex(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException("Index out of bounds");
    }
    
    // Проверка индекса для добавления
    private void CheckIndexForAdd(int index)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException("Index out of bounds");
    }  


    // ====== Базовые методы ======
    // Добавление одного элемента
    public void Add(T elem)
    {
        EnsureCapacity(size + 1);
        elementData[size++] = elem;
    }

    // Добавление всех элементов из массива
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

    // Очистка списка (удаляет все элементы и освобождает ссылки)
    public void Clear()
    {
        for (int i = 0; i < size; i++)
        {
            elementData[i] = default(T);
        }
        size = 0;
    }

    // Проверка наличия элемента
    public bool Contains(Object o)
    {
        return IndexOf(o) >= 0;
    }

    // Проверка наличия всех элементов из массива
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

    // Проверка на пустоту
    public bool IsEmpty() => size == 0;

    // Удаление элемента
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

    // Удаление всех элементов из массива
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

    // Сохранение только тех элементов, которые есть в массиве
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
    // Получение размера списка
    public int Size() => size;

    // Преобразование в массив без параметров
    public Object[] ToArray()
    {
        Object[] result = new Object[size];
        Array.Copy(elementData, result, size);
        return result;
    }

    // Преобразование в массивы с передачей массива в параметре
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

    // Получение элемента по индексу
    public T Get(int index)
    {
        CheckIndex(index);
        return elementData[index];
    }

    // Установка элемента по индексу
    public T Set(int index, T elem)
    {
        CheckIndex(index);
        T oldValue = elementData[index];
        elementData[index] = elem;
        return oldValue;
    }

    // Поиск первого вхождения элемента
    public int IndexOf(Object o)
    {
        for (int i = 0; i < size; i++)
        {
            if ((o == null && elementData[i] == null) || (o != null && elementData[i] != null && elementData[i].Equals(o)))
                return i;
        }
        return -1;
    }

    // Поиск последнего вхождения элемента
    public int LastIndexOf(Object o)
    {
        for (int i = size - 1; i >= 0; i--)
        {
            if ((o == null && elementData[i] == null) || (o != null && elementData[i] != null && elementData[i].Equals(o)))
                return i;
        }
        return -1;
    }

    // Удаление элемента по индексу
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

    // Получение подсписка
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

    // ====== Методы с индексами ======
    // Добавление одного элемента по индексу
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
    
    // Добавление всех элементов из массива по индексу
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

}