# Coding Style — Smeryse

## Общие правила
- **Язык:** C# 12 / .NET 8.0
- **Идентификаторы:** English, PascalCase для классов/методов, camelCase для переменных
- **Комментарии в коде:** НЕ добавлять (только если логика неочевидна)
- **Объяснения:** отдельно в `.md` файлах, русский язык
- **Emoji:** НЕ использовать

## Структура файла

### Конкурсная задача (CT)
```csharp
using System;
using System.IO;
using System.Text;

namespace CT{N}.Tasks;

internal static class TaskName
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        // ... читать вход, решить, записать вывод

        var sb = new StringBuilder();
        Console.Write(sb.ToString());
    }

    private static ReturnType Helper(Params) { /* ... */ }

    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int len;
        private int ptr;

        public FastScanner(Stream stream, int bufferSize = 1 << 16)
        {
            this.stream = stream;
            buffer = new byte[bufferSize];
        }

        private byte Read()
        {
            if (ptr >= len)
            {
                len = stream.Read(buffer, 0, buffer.Length);
                ptr = 0;
                if (len <= 0) return 0;
            }
            return buffer[ptr++];
        }

        public int NextInt()
        {
            int c;
            do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-') { sign = -1; c = Read(); }
            int val = 0;
            while (c > ' ') { val = val * 10 + (c - '0'); c = Read(); }
            return val * sign;
        }

        public long NextLong()
        {
            int c;
            do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-') { sign = -1; c = Read(); }
            long val = 0;
            while (c > ' ') { val = val * 10 + (c - '0'); c = Read(); }
            return val * sign;
        }

        public string NextString()
        {
            int c;
            do c = Read(); while (c <= ' ');
            var sb = new StringBuilder();
            while (c > ' ') { sb.Append((char)c); c = Read(); }
            return sb.ToString();
        }
    }
}
```

### Реализация коллекции
```csharp
namespace Task{N}.Collections
{
    public class MyCollection<T> where T : IComparable<T>
    {
        private class Node { /* вложенный приватный класс */ }

        private Node? root;
        private int size;

        public MyCollection() { }

        public int Size => size;
        public bool IsEmpty() => size == 0;

        public V? Get(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            // ...
        }

        public void Put(K key, V value)
        {
            ArgumentNullException.ThrowIfNull(key);
            // ...
        }
    }
}
```

## Именование

| Элемент | Стиль | Пример |
|---------|-------|--------|
| Класс задачи | PascalCase | `SubstringCompare`, `SumSegmentTree` |
| Метод Solve | `public static void Solve()` | Единая точка входа |
| Helper-методы | PascalCase | `GetHash()`, `MergeSort()`, `SiftUp()` |
| Локальные переменные | camelCase | `hashMap`, `iterations` |
| Индексы/размеры | `i, j, k, l, r, m, n` | Однобуквенные в алгоритмах |
| Константы | UPPER_CASE | `MOD`, `BASE`, `DEFAULT_CAPACITY` |
| Private поля | Без префикса или `_` | `root`, `table`, `_items` |

## Вывод
- **Многострочный:** `StringBuilder` + `Console.Write(sb.ToString())`
- **Разделители:** `if (i < n - 1) sb.Append(' ')`
- **Массив в строку:** `string.Join(" ", collection)`

## Обработка ошибок
- Null-проверки: `ArgumentNullException.ThrowIfNull(key)`
- Пустая коллекция: `InvalidOperationException`
- Неверный индекс: `ArgumentOutOfRangeException`
- **НЕ использовать:** `throw new Exception("...")`

## Namespace convention
| Контекст | Namespace |
|----------|-----------|
| Конкурсы | `CT{N}.Tasks` |
| Коллекции | `Task{N}.Collections` |

## Форматирование
- Фигурные скобки на отдельных строках
- `var` для `new` выражений, явные типы для `int[]`, `long[]`
- File-scoped namespace (`namespace X;`) для новых файлов
- `internal static class` для конкурсных задач
