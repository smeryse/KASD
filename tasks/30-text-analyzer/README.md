# Задача 30: MyString — своя реализация строки

## Зачем своя строка, если есть `string`?

В C# есть встроенный тип `string`, и он отличный. Но чтобы понять, как строки работают под капотом, полезно написать свою реализацию. Это как разобрать будильник — ты начинаешь понимать, как шестерёнки крутятся.

У строки внутри — просто **массив символов**. Всё. Любая операция (поиск подстроки, замена символов, перевод в верхний регистр) — это проход по этому массиву.

---

## Внутреннее устройство

```csharp
public class MyString
{
    private char[] value;  // массив символов — всё наше богатство
    private int length;    // длина строки (количество символов)
}
```

**Почему `char[]`, а не `List<char>`?** Потому что так делают в Java и в старом C/C++. Массив даёт прямой доступ по индексу за O(1) и не жрёт лишней памяти на служебные поля List'а.

**Почему отдельное поле `length`?** У массива есть своё `.Length`, но строка может быть короче массива (если мы когда-нибудь будем делать pre-alloc). Для простоты у нас `value.Length == length` всегда.

---

## Конструкторы — как создать строку

```csharp
// 1. Пустая строка
MyString() → value = new char[0], length = 0

// 2. Из массива символов (копируем!)
MyString(char[] value) → новый массив + копия данных

// 3. Копия другой строки (глубокое копирование!)
MyString(MyString original) → новый массив + копия данных
```

**Важно:** мы всегда делаем копию массива. Если просто сохранить ссылку:

```csharp
this.value = value;  // ❌ опасно!
```

То вызвавший код может изменить массив снаружи, и наша строка "сломается". Копирование защищает от этого — это называется **иммутабельность** (неизменяемость).

---

## Основные методы — как они работают

### length() — длина
```csharp
public int Length() => length;
```
Просто возвращаем поле. O(1).

### charAt(int index) — символ по индексу
```csharp
public char CharAt(int index)
{
    if (index < 0 || index >= length)
        throw new IndexOutOfRangeException("Index out of range");
    return value[index];
}
```
Проверяем границы и возвращаем символ из массива. O(1).

### substring(int begin, int end) — подстрока
```csharp
public MyString Substring(int beginIndex, int endIndex)
{
    // проверка границ
    int len = endIndex - beginIndex;
    char[] sub = new char[len];
    Array.Copy(value, beginIndex, sub, 0, len);
    return new MyString(sub);
}
```
Создаёт **новую строку** из части массива. O(n), где n — длина подстроки.

**Почему новая строка?** Если бы мы вернули ссылку на часть того же массива, то изменение подстроки могло бы изменить оригинал.

### concat(MyString str) — склейка
```csharp
public MyString Concat(MyString str)
{
    char[] result = new char[length + str.length];
    Array.Copy(value, 0, result, 0, length);        // копируем первую строку
    Array.Copy(str.value, 0, result, length, str.length); // копируем вторую
    return new MyString(result);
}
```
Создаёт новый массив, копирует туда обе строки. O(n+m).

### equals(MyString str) — сравнение
```csharp
public bool Equals(MyString str)
{
    if (str == null) return false;
    if (length != str.length) return false; // разная длина → не равны
    for (int i = 0; i < length; i++)
        if (value[i] != str.value[i]) return false; // поэлементное сравнение
    return true;
}
```
Сначала проверяем длину (быстрая проверка), потом символ за символом. O(n).

### equalsIgnoreCase — сравнение без учёта регистра
То же самое, но каждый символ сравниваем через `char.ToLower()`. O(n).

### toLowerCase() / toUpperCase() — регистр
```csharp
public MyString ToLowerCase()
{
    char[] result = new char[length];
    for (int i = 0; i < length; i++)
        result[i] = char.ToLower(value[i]);
    return new MyString(result);
}
```
Проходим по всем символам, меняем регистр, возвращаем новую строку. O(n).

### trim() — удаление пробелов по краям
```csharp
public MyString Trim()
{
    int start = 0;
    int end = length - 1;
    while (start <= end && char.IsWhiteSpace(value[start])) start++; // ищем первый не-пробел
    while (end >= start && char.IsWhiteSpace(value[end])) end--;     // ищем последний не-пробел
    int len = end - start + 1;
    if (len <= 0) return new MyString();
    char[] result = new char[len];
    Array.Copy(value, start, result, 0, len);
    return new MyString(result);
}
```
Два указателя — с начала и с конца — движутся к центру, пропуская пробелы. O(n).

### replace(char oldChar, char newChar) — замена символов
```csharp
public MyString Replace(char oldChar, char newChar)
{
    char[] result = new char[length];
    for (int i = 0; i < length; i++)
        result[i] = value[i] == oldChar ? newChar : value[i];
    return new MyString(result);
}
```
Проходим по всем символам, заменяем совпавшие. O(n).

### contains(MyString substr) — проверка подстроки
```csharp
public bool Contains(MyString substr)
{
    for (int i = 0; i <= length - substr.length; i++) // бежим по всем начальным позициям
    {
        bool match = true;
        for (int j = 0; j < substr.length; j++)       // проверяем каждый символ подстроки
            if (value[i + j] != substr.value[j]) { match = false; break; }
        if (match) return true;
    }
    return false;
}
```

Это **наивный поиск подстроки** (plain search). Сложность O(n*m) в худшем случае. Для учебной задачи — ок. В реальности используют Кнута-Морриса-Пратта или Бойера-Мура.

### indexOf(MyString substr) — индекс подстроки
То же самое, но возвращает индекс, а не bool. O(n*m).

---

## Дополнительные методы

### split(char delimiter) — разбиение на строки
```csharp
public MyString[] Split(char delimiter)
{
    // Сначала считаем, сколько будет частей
    int count = 1;
    for (int i = 0; i < length; i++)
        if (value[i] == delimiter) count++;

    MyString[] result = new MyString[count];
    int idx = 0;
    int startIdx = 0;

    for (int i = 0; i <= length; i++)
    {
        if (i == length || value[i] == delimiter)
        {
            // извлекаем часть от startIdx до i
            int len = i - startIdx;
            char[] part = new char[len];
            Array.Copy(value, startIdx, part, 0, len);
            result[idx++] = new MyString(part);
            startIdx = i + 1;
        }
    }
    return result;
}
```

Пример: `"a,b,c".Split(',')` → `["a", "b", "c"]`

Бежим по строке, при каждом разделителе отрезаем кусок.

### startsWith(MyString prefix) — начинается ли с префикса
```csharp
public bool StartsWith(MyString prefix)
{
    for (int i = 0; i < prefix.length; i++)
        if (value[i] != prefix.value[i]) return false;
    return true;
}
```
Сравниваем первые `prefix.length` символов.

### endsWith(MyString suffix) — заканчивается ли суффиксом
То же самое, но сравниваем последние `suffix.length` символов.

### reverse() — разворот строки
```csharp
public MyString Reverse()
{
    char[] result = new char[length];
    for (int i = 0; i < length; i++)
        result[i] = value[length - 1 - i]; // последний → первый, и т.д.
    return new MyString(result);
}
```

Пример: `"Hello".Reverse()` → `"olleH"`

---

## Статические методы valueOf()

```csharp
public static MyString ValueOf(int i)    // 42 → "42"
public static MyString ValueOf(double d) // 3.14 → "3.14"
public static MyString ValueOf(bool b)   // true → "true"
```

Они принимают значение и возвращают `MyString`. Внутри используют `ToString()` у встроенных типов и конвертируют в `char[]`.

```csharp
public static MyString ValueOf(int i)
{
    return new MyString(i.ToString().ToCharArray());
}
```

---

## Иммутабельность

Все методы, которые как бы "меняют" строку (`ToLowerCase`, `Replace`, `Trim`, `Reverse`, `Concat`, `Substring`), возвращают **новую строку**. Исходная строка никогда не меняется.

Это сделано специально — чтобы строка вела себя как `string` в C#: после создания её нельзя изменить. Если нужно другое значение — создаём новую строку.

**Почему это хорошо:**
- Строки можно безопасно передавать в функции — никто их не испортит
- Строки можно использовать как ключи в HashMap (их хеш не поменяется)
- Проще отлаживать — строка либо есть, либо её нет

---

## Обработка ошибок

Все методы проверяют:
- `ArgumentNullException` — если передали `null`
- `IndexOutOfRangeException` — если индекс выходит за границы
- `ArgumentOutOfRangeException` — если индексы в `Substring` некорректны

```csharp
if (value == null) throw new ArgumentNullException(nameof(value));
if (index < 0 || index >= length) throw new IndexOutOfRangeException(...)
```
