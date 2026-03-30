# Архитектурный аудит проекта CT9

**Статус:** Учебный проект (контест по строковым алгоритмам)  
**Дата аудита:** март 2026  
**Аудитор:** Architecture Review Bot

---

## Резюме

Проект функционален, все 7 задач работают корректно. Однако с точки зрения архитектуры и инженерной культуры наблюдается **критическая дупликация кода**, **нарушение принципа единственной ответственности**, **проблемы с конфигурацией проекта** и **отсутствие границ между слоями**. Ниже приведён подробный разбор.

---

## 1. Критические проблемы

### 1.1. Дупликация кода (DRY violation) — КРИТИЧНО

**Проблема:** Класс `FastScanner` дублируется в **каждом** из 7 файлов задач (A–G). Итого 7 идентичных реализаций ~60 строк каждая = ~420 строк дубликата.

**Почему это плохо:**
- Увеличение размера кодовой базы в 6 раз без функциональной пользы
- При исправлении бага в `FastScanner` нужно править 7 файлов
- Невозможно централизованно улучшить производительность или добавить фичи
- Нарушение принципа DRY (Don't Repeat Yourself)

**Как исправить:**
```csharp
// Создать файл Infrastructure/FastScanner.cs
namespace CT9.Infrastructure;

internal sealed class FastScanner
{
    // Единая реализация для всего проекта
}

// В задачах использовать через using или полную квалификацию
using CT9.Infrastructure;
```

---

### 1.2. Несоответствие имени проекта — КРИТИЧНО

**Проблема:** В файле `CT9.csproj` указаны:
```xml
<RootNamespace>CT1</RootNamespace>
<AssemblyName>CT1</AssemblyName>
```

При этом папка называется `CT9`, namespace в коде — `CT9.Tasks`, dll на выходе — `CT1.dll`.

**Почему это плохо:**
- Путаница при отладке (какой это контест?)
- Проблемы с рефакторингом (переименование не работает корректно)
- Невозможность статического анализа (IDE теряет контекст)
- Технический долг с первого дня

**Как исправить:**
```xml
<RootNamespace>CT9</RootNamespace>
<AssemblyName>CT9</AssemblyName>
```

---

### 1.3. Отсутствие разделения на слои — ВЫСОКИЙ приоритет

**Проблема:** Весь код находится в одном namespace `CT9.Tasks`. Нет разделения на:
- **Infrastructure** (FastScanner, утилиты)
- **Algorithms** (чистые реализации алгоритмов)
- **Tasks** (обработчики ввода/вывода)

**Почему это плохо:**
- Невозможно переиспользовать алгоритмы без тянущихся зависимостей на ввод/вывод
- Невозможно протестировать алгоритмы изолированно
- Смешение бизнес-логики (алгоритм Z-функции) с инфраструктурой (консольный ввод)

**Как исправить:**
```
CT9/
├── Infrastructure/
│   ├── FastScanner.cs
│   └── StringUtils.cs
├── Algorithms/
│   ├── Hashing/
│   │   ├── PolynomialHash.cs
│   │   └── DoubleHash.cs
│   ├── StringMatching/
│   │   ├── PrefixFunction.cs
│   │   ├── ZFunction.cs
│   │   └── AhoCorasick.cs
│   └── Period/
│       └── PeriodFinder.cs
├── Tasks/
│   ├── A-SubstringCompare.cs  // только ввод/вывод
│   ├── B-PrefixFunction.cs
│   └── ...
└── Program.cs
```

Пример рефакторинга для задачи C:
```csharp
// Algorithms/StringMatching/ZFunction.cs
namespace CT9.Algorithms.StringMatching;

internal static class ZFunctionAlgorithm
{
    public static int[] Compute(string s)
    {
        // Чистая функция, без зависимостей на Console, Stream, etc.
    }
}

// Tasks/C-ZFunction.cs
namespace CT9.Tasks;

internal static class ZFunctionTask  // переименовать класс!
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        string s = fs.NextString();
        int[] z = ZFunctionAlgorithm.Compute(s);  // делегирование
        // ... вывод
    }
}
```

---

## 2. Проблемы среднего уровня

### 2.1. Нарушение Single Responsibility Principle в Program.cs

**Проблема:** Класс `Program` делает всё:
- Парсинг аргументов командной строки
- Маршрутизацию задач
- Работу с файловой системой
- Сравнение вывода с эталоном
- Нормализацию текста
- Вывод сообщений об ошибках

**Как исправить:** Выделить отдельные классы:
```csharp
internal static class TaskRunner      // запуск задачи с проверкой
internal static class InputResolver   // работа с input-файлами
internal static class OutputComparer  // сравнение с эталоном
internal static class UsagePrinter    // справка
```

---

### 2.2. Неправильное использование Dictionary для маппинга задач

**Проблема:**
```csharp
private static readonly Dictionary<string, Action> TaskMap = new(StringComparer.OrdinalIgnoreCase)
{
    ["A"] = SubstringCompare.Solve,
    // ...
};
```

**Почему это плохо:**
- `Dictionary` не гарантирует порядок (хотя сейчас не критично)
- Нет компиляционной проверки: опечатка в ключе `"A"` обнаружится только в runtime
- Нет проверки на null значения

**Как исправить:** Использовать `ImmutableDictionary` или `switch expression`:
```csharp
private static Action GetTask(string key) => key.ToUpperInvariant() switch
{
    "A" => SubstringCompare.Solve,
    "B" => PrefixFunction.Solve,
    // ...
    _ => throw new ArgumentException($"Неизвестная задача: {key}")
};
```

---

### 2.3. Утечка ресурсов в TryOpenSample

**Проблема:**
```csharp
private static bool TryOpenSample(string key, out TextReader reader)
{
    // ...
    reader = new StreamReader(path);  // кто закроет?
    return true;
}
```

`StreamReader` оборачивает `FileStream`, который не закрывается явно. В короткоживущем приложении это не критично, но это плохая привычка.

**Как исправить:**
```csharp
// Вариант 1: Возвращать IDisposable
private static bool TryOpenSample(string key, out IDisposable disposable, out TextReader reader)

// Вариант 2: Использовать File.OpenText (возвращает StreamReader, который закроется с процессом)
reader = File.OpenText(path);
```

---

### 2.4. Магические числа в хэшировании

**Проблема:**
```csharp
long MOD = 1_000_000_007L;
long BASE = 31L;
```

**Почему это плохо:**
- Нет объяснения, почему выбраны именно эти числа
- Нет проверки, что BASE < MOD (критично для корректности хэширования)
- Невозможно изменить для тестирования

**Как исправить:**
```csharp
// Infrastructure/HashConstants.cs
internal static class HashConstants
{
    // Простое число ~10^9 для уменьшения коллизий
    public const long DefaultMod = 1_000_000_007L;
    
    // Основание полиномиального хэша (должно быть > размера алфавита)
    public const long DefaultBase = 31L;  // 26 букв + запас
    
    // Второе простое число для двойного хэширования
    public const long SecondaryMod = 1_000_000_009L;
    public const long SecondaryBase = 37L;
    
    public static void Validate(long mod, long baseVal)
    {
        if (baseVal >= mod)
            throw new ArgumentException("Base must be less than modulus");
    }
}
```

---

### 2.5. Неэффективное выделение памяти в G-MultipleSearch

**Проблема:**
```csharp
public bool[] Search(string text)
{
    bool[] found = new bool[trie.Count];  // размер по количеству узлов
    // ...
    bool[] result = new bool[found.Length];  // зачем копировать?!
    for (int i = 0; i < found.Length; i++)
        result[i] = found[i];
    return result;
}
```

**Почему это плохо:**
- Бесмысленное выделение дополнительного массива
- Удвоение памяти без причины
- O(n) операций на копирование

**Как исправить:**
```csharp
public bool[] Search(string text)
{
    bool[] found = new bool[patterns.Length];  // по количеству паттернов, а не узлов!
    // ...
    return found;  // без копирования
}
```

---

### 2.6. Неправильный размер массива в AhoCorasick.Search

**Проблема:**
```csharp
bool[] found = new bool[trie.Count];  // количество узлов в боре
```

Должно быть:
```csharp
bool[] found = new bool[patterns.Length];  // количество паттернов
```

**Почему это плохо:**
- Если в боре 1000 узлов, а паттернов 10, массив будет в 100 раз больше нужного
- Путаница при чтении кода

---

## 3. Проблемы низкого уровня (code style)

### 3.1. Несогласованное именование классов

- `SubstringCompare` (существительное)
- `PrefixFunction` (существительное)
- `ZFunction` (существительное)
- `CommonSubstring` (существительное)
- `MultipleSearch` (существительное)

Но в CT4:
- `SumSegmentTree` (существительное)
- `MinCountSegmentTree` (существительное)

**Рекомендация:** Использовать суффикс `Task` для классов задач:
- `SubstringCompareTask`
- `PrefixFunctionTask`
- `ZFunctionTask`

Это явно отделит задачи от алгоритмов.

---

### 3.2. Отсутствие проверок на null/empty

```csharp
public static void Solve()
{
    var fs = new FastScanner(Console.OpenStandardInput());
    string s = fs.NextString();  // а если пустая строка?
    // ...
}
```

**Рекомендация:** Добавить валидацию:
```csharp
if (string.IsNullOrEmpty(s))
    throw new ArgumentException("Input string cannot be empty");
```

---

### 3.3. Неиспользуемый параметр bufferSize

```csharp
public FastScanner(Stream stream, int bufferSize = 1 << 16)
{
    this.stream = stream;
    buffer = new byte[bufferSize];  // ok
}
```

Параметр никогда не меняется от значения по умолчанию. Если bufferSize не нужен — убрать. Если нужен — добавить тесты с разными размерами.

---

### 3.4. Жёсткое кодирование путей в Program.cs

```csharp
private static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
```

**Почему это плохо:**
- Зависимость от структуры каталогов bin/Debug/net8.0
- При изменении TargetFramework пути сломаются
- Невозможно запустить из другой рабочей директории

**Как исправить:**
```csharp
// Использовать AppDomain.CurrentDomain.BaseDirectory + относительный путь
// Или передавать корень через аргумент/переменную окружения
```

---

### 3.5. Отсутствие документации на публичные методы

```csharp
/// <summary>
/// A. Сравнения подстрок
/// ...
/// </summary>
internal static class SubstringCompare
{
    public static void Solve()  // нет XML-документации!
    {
```

**Рекомендация:** Добавить документацию:
```csharp
/// <summary>
/// Решает задачу A: сравнение подстрок с помощью хэширования.
/// Читает из stdin: строку s, число m, m запросов (a,b,c,d).
/// Выводит в stdout: m строк Yes/No.
/// </summary>
public static void Solve()
```

---

### 3.6. Неоптимальное использование StringBuilder

```csharp
var sb = new StringBuilder();
for (int i = 0; i < n; i++)
{
    sb.Append(pi[i]);
    if (i < n - 1) sb.Append(' ');
}
Console.WriteLine(sb.ToString());
```

**Проблема:** `ToString()` создаёт новую строку, затем `WriteLine` создаёт ещё одну копию при конвертации.

**Как исправить:**
```csharp
Console.WriteLine(sb);  // StringBuilder имеет перегрузку
```

---

### 3.7. Избыточная проверка в GetHash

```csharp
private static long GetHash(long[] h, long[] powers, int l, int r, long MOD)
{
    long result = (h[r + 1] - h[l] * powers[r - l + 1]) % MOD;
    if (result < 0) result += MOD;  // ok
    return result;
}
```

**Проблема:** Умножение `h[l] * powers[r - l + 1]` может переполнить `long` (максимум ~9×10^18, а произведение двух чисел ~10^9 даст ~10^18, что близко к пределу).

**Рекомендация:** Использовать `unchecked` или `BigInteger` для гарантии:
```csharp
long result = (h[r + 1] - unchecked(h[l] * powers[r - l + 1])) % MOD;
```

---

## 4. Рекомендации по улучшению архитектуры

### 4.1. Предлагаемая структура проекта

```
CT9/
├── CT9.csproj                    # Исправить RootNamespace и AssemblyName
├── Program.cs                    # Точка входа, тонкий слой маршрутизации
├── CT9.md                        # Описание контеста
│
├── Infrastructure/               # Общие утилиты
│   ├── FastScanner.cs            # Единый класс для ввода
│   ├── HashConstants.cs          # Константы хэширования
│   └── TextNormalizer.cs         # Нормализация вывода
│
├── Algorithms/                   # Чистые алгоритмы (без IO)
│   ├── Hashing/
│   │   ├── PolynomialHash.cs     # Префиксные хэши
│   │   └── DoubleHash.cs         # Двойное хэширование
│   ├── StringMatching/
│   │   ├── PrefixFunction.cs     # π-функция
│   │   ├── ZFunction.cs          # Z-функция
│   │   └── AhoCorasick.cs        # Ахо-Корасик
│   └── Period/
│       └── PeriodFinder.cs       # Поиск периода
│
├── Tasks/                        # Обработчики задач (IO + алгоритмы)
│   ├── A-SubstringCompare.cs
│   ├── B-PrefixFunction.cs
│   ├── C-ZFunction.cs
│   ├── D-SubstringSearch.cs
│   ├── E-PeriodSearch.cs
│   ├── F-CommonSubstring.cs
│   └── G-MultipleSearch.cs
│
├── Samples/                      # Тестовые данные
│   ├── A.in, A.out
│   └── ...
│
├── Explanations/                 # Объяснения решений
│   ├── A.md
│   └── ...
│
└── Tests/                        # Юнит-тесты (отсутствуют!)
    ├── PolynomialHashTests.cs
    ├── ZFunctionTests.cs
    └── ...
```

---

### 4.2. Пример рефакторинга задачи C

**Было:**
```csharp
// Tasks/C-ZFunction.cs
internal static class ZFunction
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        string s = fs.NextString();
        int[] z = ComputeZFunction(s);  // алгоритм внутри задачи
        // ...
    }
    
    private static int[] ComputeZFunction(string s) { /* 30 строк */ }
    private sealed class FastScanner { /* 60 строк */ }
}
```

**Стало:**
```csharp
// Algorithms/StringMatching/ZFunction.cs
namespace CT9.Algorithms.StringMatching;

/// <summary>
/// Вычисляет Z-функцию для строки.
/// Z[i] = длина наибольшего общего префикса s и s[i..n-1].
/// </summary>
internal static class ZFunction
{
    /// <summary>
    /// Вычисляет массив Z-значений за O(n).
    /// </summary>
    /// <param name="s">Входная строка</param>
    /// <returns>Массив z[0..n-1], где z[0] = 0</returns>
    public static int[] Compute(string s)
    {
        if (s == null) throw new ArgumentNullException(nameof(s));
        
        int n = s.Length;
        int[] z = new int[n];
        int l = 0, r = 0;
        
        for (int i = 1; i < n; i++)
        {
            if (i < r)
                z[i] = Math.Min(r - i, z[i - l]);
            
            while (i + z[i] < n && s[z[i]] == s[i + z[i]])
                z[i]++;
            
            if (i + z[i] > r)
            {
                l = i;
                r = i + z[i];
            }
        }
        
        return z;
    }
}

// Tasks/C-ZFunction.cs
namespace CT9.Tasks;

/// <summary>
/// Задача C: построение Z-функции.
/// </summary>
internal static class ZFunctionTask
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        string s = fs.NextString();
        
        int[] z = ZFunction.Compute(s);  // делегирование алгоритму
        
        var sb = new StringBuilder();
        for (int i = 1; i < z.Length; i++)  // z[0] не выводим
        {
            sb.Append(z[i]);
            if (i < z.Length - 1) sb.Append(' ');
        }
        Console.WriteLine(sb);
    }
}
```

**Выгоды:**
- Алгоритм тестируется изолированно
- Можно переиспользовать в других задачах
- Задача отвечает только за IO
- Понятная ответственность каждого класса

---

### 4.3. Добавить юнит-тесты

**Почему важно:** Сейчас корректность проверяется только на примерах из условия. Нет тестов на:
- Граничные случаи (пустая строка, 1 символ)
- Большие данные (10^6 символов)
- Коллизии хэшей
- Корректность Aho-Corasick на специфичных паттернах

**Пример теста:**
```csharp
// Tests/ZFunctionTests.cs
[TestClass]
public class ZFunctionTests
{
    [TestMethod]
    public void Compute_EmptyString_ReturnsEmptyArray()
    {
        int[] z = ZFunction.Compute("");
        Assert.AreEqual(0, z.Length);
    }
    
    [TestMethod]
    public void Compute_SingleChar_ReturnsZero()
    {
        int[] z = ZFunction.Compute("a");
        Assert.AreEqual(1, z.Length);
        Assert.AreEqual(0, z[0]);
    }
    
    [TestMethod]
    public void Compute_AaaAAA_ReturnsExpected()
    {
        int[] z = ZFunction.Compute("aaaAAA");
        CollectionAssert.AreEqual(new[] { 0, 2, 1, 0, 0, 0 }, z);
    }
}
```

---

### 4.4. Использовать Nullable Reference Types

**Проблема:** В проекте отключены nullable reference types:
```xml
<Nullable>disable</Nullable>
```

**Почему это плохо:** Компилятор не предупреждает о возможном `NullReferenceException`.

**Как исправить:**
```xml
<Nullable>enable</Nullable>
```

И исправить код:
```csharp
public static void Solve()
{
    string s = fs.NextString()!;  // явное указание, что null невозможен
    // или
    string? s = fs.NextString();
    if (s == null) return;
}
```

---

## 5. Сводная таблица проблем

| Категория | Проблема | Приоритет | Трудозатраты |
|-----------|----------|-----------|--------------|
| DRY | Дублирование FastScanner (7 копий) | Критично | 1 час |
| Конфигурация | Неверные RootNamespace/AssemblyName | Критично | 5 минут |
| Архитектура | Отсутствие слоёв (Infrastructure/Algorithms/Tasks) | Высокий | 4 часа |
| SRP | Program.cs делает всё | Высокий | 2 часа |
| Память | Бесмысленное копирование массивов в G | Средний | 15 минут |
| Память | Неправильный размер массива в AhoCorasick | Средний | 10 минут |
| Надёжность | Нет проверок на null/empty | Средний | 30 минут |
| Надёжность | Магические числа в хэшировании | Средний | 30 минут |
| Стиль | Несогласованное именование | Низкий | 30 минут |
| Стиль | Отсутствие XML-документации | Низкий | 1 час |
| Тесты | Полное отсутствие юнит-тестов | Высокий | 6 часов |

---

## 6. План рефакторинга (приоритизированный)

### Фаза 1: Быстрые победы (30 минут)
1. Исправить `CT9.csproj` (RootNamespace, AssemblyName)
2. Исправить размер массива в `AhoCorasick.Search`
3. Убрать копирование массива в `AhoCorasick.Search`

### Фаза 2: Устранение дупликации (1 час)
1. Создать `Infrastructure/FastScanner.cs`
2. Удалить 7 копий из задач
3. Добавить `using CT9.Infrastructure` в задачи

### Фаза 3: Разделение на слои (4 часа)
1. Создать папки `Algorithms/Hashing`, `Algorithms/StringMatching`
2. Вынести чистые алгоритмы в отдельные классы
3. Обновить задачи на использование алгоритмов

### Фаза 4: Улучшение Program.cs (2 часа)
1. Выделить `TaskRunner`, `InputResolver`, `OutputComparer`
2. Упростить `Main` до маршрутизации
3. Добавить обработку исключений

### Фаза 5: Тесты (6 часов)
1. Добавить тесты на алгоритмы
2. Добавить тесты на граничные случаи
3. Настроить CI (опционально)

---

## 7. Заключение

Проект **функционален**, но **архитектурно незрел**. Основные проблемы:
- Дупликация кода (7 копий FastScanner)
- Путаница с namespace/assembly
- Отсутствие разделения на слои
- Нет тестов

**Хорошая новость:** Все проблемы исправимы за 8-10 часов работы. После рефакторинга проект станет образцом для учебных контестов.

**Рекомендация:** Начать с Фазы 1 и 2 (быстрые победы), затем постепенно двигаться дальше.
