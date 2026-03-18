using System;
using Task8.Collections;
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Тест MyArrayList ===");

        // 1. Создание и добавление элементов
        MyArrayList<int> list = new MyArrayList<int>();
        list.Add(10);
        list.Add(20);
        list.Add(30);
        Console.WriteLine("После Add: " + string.Join(", ", list.ToArray()));

        // 2. Добавление элемента по индексу
        list.Add(1, 15); // вставляем 15 на индекс 1
        Console.WriteLine("После Add(1,15): " + string.Join(", ", list.ToArray()));

        // 3. Получение и изменение элемента
        int value = list.Get(2);
        Console.WriteLine("Элемент на позиции 2: " + value);
        list.Set(2, 25);
        Console.WriteLine("После Set(2,25): " + string.Join(", ", list.ToArray()));

        // 4. Удаление элемента
        list.RemoveAt(0); // удаляем первый элемент
        Console.WriteLine("После RemoveAt(0): " + string.Join(", ", list.ToArray()));

        // 5. Проверка Contains
        Console.WriteLine("Содержит 25? " + list.Contains(25));
        Console.WriteLine("Содержит 100? " + list.Contains(100));

        // 6. AddAll
        int[] arr = { 50, 60, 70 };
        list.AddAll(arr);
        Console.WriteLine("После AddAll([50,60,70]): " + string.Join(", ", list.ToArray()));

        // 7. Size и IsEmpty
        Console.WriteLine("Размер списка: " + list.Size());
        Console.WriteLine("Пустой список? " + list.IsEmpty());

        // 8. Clear
        list.Clear();
        Console.WriteLine("После Clear, пустой список? " + list.IsEmpty());
    }
}
