# Задача 31*: Крисс-кросс — генератор головоломки

## Что такое крисс-кросс?

Это головоломка, похожая на кроссворд, но без определений. Тебе дают:
1. Пустую сетку
2. Список слов

Нужно вписать все слова в сетку так, чтобы они пересекались (как в кроссворде) — в местах пересечения буквы должны совпадать. Все слова должны быть использованы ровно один раз.

Пример:
```
  ow
hello
  odr
  w l
    d
```

Здесь `hello` (горизонтально) пересекается с `world` (вертикально) по букве 'l'. Оба слова использованы, всё связно.

---

## Как это решается: backtracking (перебор с возвратом)

Это классическая задача, которая решается **рекурсивным перебором**:

```
1. Берём первое слово, кладём его по центру сетки горизонтально
2. Для каждого оставшегося слова:
   3. Пробуем пересечь его с каждым уже размещённым словом
      4. Пробуем каждую возможную букву пересечения
         5. Если слово можно разместить — размещаем
            6. Переходим к следующему слову (шаг 2)
         7. Если не получается — откатываем (удаляем слово)
8. Если все слова размещены — УРА, нашли решение!
9. Если ни одно слово не подходит — решения нет
```

Это называется **backtracking** — рекурсивно пробуем варианты, и если зашли в тупик, откатываемся назад и пробуем другой вариант.

---

## Структура программы

### WordPlacement — запись о размещённом слове

```csharp
public class WordPlacement
{
    public string Word { get; set; }      // само слово
    public int Row { get; set; }          // строка начала
    public int Col { get; set; }          // колонка начала
    public bool Horizontal { get; set; }  // true = горизонтально, false = вертикально
    public int EndRow => Horizontal ? Row : Row + Word.Length - 1;
    public int EndCol => Horizontal ? Col + Word.Length - 1 : Col;
}
```

Хранит, где и как лежит слово. `EndRow`/`EndCol` — вычисляемые координаты конца слова.

### CrissCrossSolver — сам решатель

Поля:
- `_words` — список всех слов
- `_grid[GridSize, GridSize]` — сетка (char[,]), где `'\0'` = пустая клетка
- `_placements` — список размещённых слов
- `_solved` — нашли ли решение

---

## Пошаговый разбор алгоритма

### Шаг 1: Начальное размещение

```csharp
// Берём самое длинное слово (оно даёт больше возможностей для пересечений)
var first = ordered[0];

// Кладём его горизонтально в центр сетки
int center = GridSize / 2;
for (int c = 0; c < first.Length; c++)
    _grid[center, center + c] = first[c];

_placements.Add(new WordPlacement
{
    Word = first, Row = center, Col = center, Horizontal = true
});
```

Центр сетки — чтобы было место для роста во все стороны.

### Шаг 2: Backtrack — основная рекурсия

```csharp
private bool Backtrack(List<string> words, bool[] used, int placedCount)
{
    // Базовый случай: все слова размещены → решение найдено
    if (placedCount == words.Count) return true;

    // Пробуем каждое ещё не использованное слово
    for (int i = 0; i < words.Count; i++)
    {
        if (used[i]) continue;  // слово уже использовано

        // Пробуем пересечь его с каждым уже размещённым словом
        for (int p = 0; p < _placements.Count; p++)
        {
            var existing = _placements[p];

            // Пробуем каждую букву нового слова
            for (int intersectIdx = 0; intersectIdx < word.Length; intersectIdx++)
            {
                // Пробуем каждую букву существующего слова
                for (int existIdx = 0; existIdx < existing.Word.Length; existIdx++)
                {
                    // Буквы должны совпадать для пересечения
                    if (existing.Word[existIdx] != word[intersectIdx])
                        continue;

                    // Вычисляем, где окажется новое слово
                    int row, col;
                    bool horizontal;

                    if (existing.Horizontal)
                    {
                        // Существующее слово — горизонтальное
                        // Новое слово будет вертикальным
                        row = existing.Row - intersectIdx;
                        col = existing.Col + existIdx;
                        horizontal = false;
                    }
                    else
                    {
                        // Существующее слово — вертикальное
                        // Новое слово будет горизонтальным
                        row = existing.Row + existIdx;
                        col = existing.Col - intersectIdx;
                        horizontal = true;
                    }

                    // Проверяем, можно ли разместить
                    if (CanPlace(word, row, col, horizontal))
                    {
                        PlaceWord(word, row, col, horizontal);
                        used[i] = true;

                        // Рекурсивно продолжаем
                        if (Backtrack(words, used, placedCount + 1))
                            return true;

                        // Не получилось — откатываем
                        RemoveWord(word, row, col, horizontal);
                        used[i] = false;
                    }
                }
            }
        }
    }

    return false;  // ничего не подошло
}
```

**Визуализация отката (backtrack):**

```
Попробовали hello → ok
  Попробовали world → ok
    Попробовали low → не встаёт :(
      Пробуем old → не встаёт :(
      Откатываем low
    Попробовали old → не встаёт :(
    Откатываем world
  Попробовали world в другом месте → ok
    Попробовали low → ok! Всё встало.
    УРА!
```

### Шаг 3: CanPlace — проверка возможности размещения

```csharp
private bool CanPlace(string word, int startRow, int startCol, bool horizontal)
{
    // 1. Проверка границ сетки
    if (startRow < 0 || startCol < 0 || endRow >= GridSize || endCol >= GridSize)
        return false;

    bool hasIntersection = false;

    // 2. Проверка каждой клетки слова
    for (int i = 0; i < word.Length; i++)
    {
        int r = horizontal ? startRow : startRow + i;
        int c = horizontal ? startCol + i : startCol;

        char existing = _grid[r, c];  // что уже есть в этой клетке

        if (existing != Empty) // клетка занята
        {
            if (existing == word[i])
                hasIntersection = true;  // совпало — это пересечение!
            else
                return false;  // не совпало — конфликт букв
        }
        // если клетка пуста — всё ок, просто ставим букву
    }

    return hasIntersection; // нужно хотя бы одно пересечение
}
```

Упрощённая версия — проверяет только:
1. Не вылезаем за границы сетки
2. В пустые клетки можно ставить буквы
3. В занятых клетках буквы должны совпадать (пересечение)
4. Должно быть хотя бы одно пересечение (иначе слово висит в воздухе)

### Шаг 4: PlaceWord — размещение слова

```csharp
private void PlaceWord(string word, int startRow, int startCol, bool horizontal)
{
    // Ставим буквы в сетку
    for (int i = 0; i < word.Length; i++)
    {
        int r = horizontal ? startRow : startRow + i;
        int c = horizontal ? startCol + i : startCol;
        _grid[r, c] = word[i];
    }

    // Запоминаем размещение
    _placements.Add(new WordPlacement { Word = word, Row = startRow, Col = startCol, Horizontal = horizontal });
}
```

### Шаг 5: RemoveWord — откат размещения

```csharp
private void RemoveWord(string word, int startRow, int startCol, bool horizontal)
{
    for (int i = 0; i < word.Length; i++)
    {
        int r = horizontal ? startRow : startRow + i;
        int c = horizontal ? startCol + i : startCol;

        // Проверяем, не пересекается ли эта клетка с другими словами
        bool shared = false;
        foreach (var pl in _placements)
        {
            if (pl.Word == word) continue;  // пропускаем себя
            // Проверяем, занимает ли другое слово эту клетку
            if (pl.Horizontal && pl.Row == r && c >= pl.Col && c <= pl.EndCol)
            { shared = true; break; }
            if (!pl.Horizontal && pl.Col == c && r >= pl.Row && r <= pl.EndRow)
            { shared = true; break; }
        }

        if (!shared)
            _grid[r, c] = Empty;  // клетка не общая — очищаем
    }

    // Удаляем запись о слове
    _placements.RemoveAll(p => p.Word == word && p.Row == startRow && p.Col == startCol && p.Horizontal == horizontal);
}
```

**Важно:** при откате нельзя просто затереть все клетки — некоторые буквы могут быть частью пересечения с другими словами. Код проверяет, не делит ли кто-то ещё эту клетку.

### Шаг 6: PrintGrid — вывод результата

```csharp
public void PrintGrid()
{
    if (!_solved) { Console.WriteLine("No solution found."); return; }

    // Находим bounding box
    int minR = GridSize, maxR = 0, minC = GridSize, maxC = 0;
    for (int r = 0; r < GridSize; r++)
        for (int c = 0; c < GridSize; c++)
            if (_grid[r, c] != Empty)
            {
                if (r < minR) minR = r;
                if (r > maxR) maxR = r;
                if (c < minC) minC = c;
                if (c > maxC) maxC = c;
            }

    // Выводим только занятую область
    for (int r = minR; r <= maxR; r++)
    {
        for (int c = minC; c <= maxC; c++)
            Console.Write(_grid[r, c] == Empty ? ' ' : _grid[r, c]);
        Console.WriteLine();
    }
}
```

Находит минимальный прямоугольник, который охватывает все буквы, и выводит только его.

---

## Почему слова группируются по длине?

```csharp
var grouped = _words
    .GroupBy(w => w.Length)
    .OrderByDescending(g => g.Key)
    .Select(g => g.OrderBy(w => w).ToList())
    .ToList();
```

Сначала самые длинные слова — у них больше букв, а значит больше возможных пересечений. Это эвристика: длинные слова разместить труднее, поэтому их ставим первыми.

Внутри каждой группы — по алфавиту, для детерминированности (одинаковые результаты при одинаковых входных данных).

---

## Сложность алгоритма

В худшем случае — **O(n! * m²)**, где n — количество слов, m — длина слова. Это факториальная сложность, потому что мы перебираем все порядки размещения слов.

Для 10+ слов может быть медленно. Эвристики (длинные слова первыми, группировка по длине) помогают, но не гарантируют быстроты.

---

## Если решения нет

```csharp
Console.WriteLine("No solution exists for the given word list.");
```

Причины:
1. Слова не имеют общих букв — нечем пересекаться
2. Слова имеют буквы, но расположение не позволяет всем пересечься связно
3. Просто не повезло — алгоритм не находит все возможные расположения
