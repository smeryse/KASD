# Задача 28: Итераторы

## Что это вообще такое?

Итератор — это штука, которая позволяет пройтись по всем элементам коллекции по одному, не залезая в её внутренности. Представь, что у тебя есть коробка с шариками, а итератор — это рука, которая достаёт шарики по одному: "Дай следующий. Дай следующий. Дай следующий".

В C# и Java есть встроенные итераторы (`foreach`). Здесь мы пишем свои — чтобы понять, как они работают изнутри.

---

## Два вида итераторов

### 1. `IMyIterator<T>` — простой итератор
Умеет только три вещи:

```csharp
public interface IMyIterator<T>
{
    bool HasNext();   // есть ли ещё элемент?
    T Next();         // дай следующий
    void Remove();    // удали текущий элемент из коллекции
}
```

**Как работает:**
- `cursor` — указатель на текущую позицию (начинается с 0)
- `lastRet` — индекс последнего возвращённого элемента (чтобы знать, что удалять)
- `HasNext()` — true, если cursor < размера коллекции
- `Next()` — возвращает элемент по cursor и сдвигает cursor++
- `Remove()` — удаляет элемент, который вернули последним вызовом Next()

### 2. `IMyListIterator<T>` — продвинутый итератор (для списков)
Умеет ходить и вперёд, и назад, и вставлять, и заменять:

```csharp
public interface IMyListIterator<T> : IMyIterator<T>
{
    bool HasPrevious();    // есть ли предыдущий элемент?
    T Previous();          // дай предыдущий
    int NextIndex();       // какой индекс у следующего?
    int PreviousIndex();   // какой индекс у предыдущего?
    void Set(T element);   // замени текущий элемент
    void Add(T element);   // вставь новый элемент перед текущей позицией
}
```

**Зачем?** Обычный итератор — как плёнка: только вперёд. ListIterator — как книжка: можно и туда, и обратно листать, и закладки вставлять.

---

## Для каких коллекций сделаны итераторы

### `IMyIterator` (простой) для:
| Коллекция | Класс итератора | Как работает |
|-----------|----------------|--------------|
| `MyPriorityQueue` | `MyPriorityQueueIterator<T>` | Снимает снепшот (копию) элементов через `ToArray()`, ходит по нему. При `Remove()` дёргает `queue.Remove()`. |
| `MyHashSet` | `MyHashSetIterator<T>` | То же самое: снепшот + удаление через `set.Remove()`. |
| `MyTreeSet` | `MyTreeSetIterator<T>` | То же самое: снепшот + удаление через `treeSet.Remove()`. |

### `IMyListIterator` (продвинутый) для:
| Коллекция | Класс итератора | Как работает |
|-----------|----------------|--------------|
| `MyArrayList` | `MyArrayListIterator<T>` | Ходит по индексам. `Set()` вызывает `list.Set(index, elem)`. `Add()` вставляет со сдвигом. |
| `MyVector` | `MyVectorIterator<T>` | Аналогично `MyArrayListIterator`, но для `MyVector`. |
| `MyLinkedList` | `MyLinkedListIterator<T>` | Аналогично, через `Get(index)`, `Set(index, elem)`, `Add(index, elem)`. |

---

## Снепшот — что это и зачем?

```csharp
private readonly List<T> snapshot;

public MyHashSetIterator(MyHashSet<T> set)
{
    this.snapshot = new List<T>();
    foreach (var obj in set.ToArray())
        if (obj is T t) snapshot.Add(t);
    this.cursor = 0;
}
```

**Проблема:** пока мы итерируемся, коллекция может измениться (добавят/удалят элементы). Если итератор работает напрямую с коллекцией, а она меняется — всё сломается.

**Решение:** в конструкторе делаем копию всех элементов в список (снепшот). Итератор ходит по этой копии. При `Remove()` — удаляем и из коллекции, и из снепшота, чтобы они были согласованы.

**Минус:** если коллекция огромная, снепшот жрёт память. Но для учебных целей ок.

---

## ListIterator — как работает ходьба назад

```csharp
public class MyArrayListIterator<T> : IMyListIterator<T>
{
    private readonly MyArrayList<T> list;
    private int cursor;    // текущая позиция
    private int lastRet;   // индекс последнего возвращённого элемента

    public T Next()
    {
        lastRet = cursor++;
        return list.Get(lastRet);  // вернули текущий, сдвинулись вправо
    }

    public T Previous()
    {
        lastRet = --cursor;
        return list.Get(lastRet);  // сдвинулись влево, вернули
    }
}
```

**Визуально:**

```
Список: [a, b, c, d]
            ^
          cursor=0

Next() → a, cursor=1
Next() → b, cursor=2
Previous() → b, cursor=1   // вернулись назад!
Previous() → a, cursor=0
```

`Set()` заменяет элемент, который вернули последним `Next()` или `Previous()`.
`Add()` вставляет новый элемент **перед** текущей позицией курсора.

---

## Иерархия ошибок

```csharp
MyCollectionException          // базовое исключение для всех коллекций
├── MyNoSuchElementException   // когда элемента нет, а ты просишь
├── MyIllegalStateException    // когда Remove() без вызова Next()
└── MyUnsupportedOperationException  // когда операция не поддерживается
```

Зачем иерархия? Можно ловить все ошибки коллекций одним catch:

```csharp
try { it.Remove(); }
catch (MyCollectionException ex) { /* любая ошибка итератора */ }
```

Или каждую отдельно:
```csharp
catch (MyNoSuchElementException e) { /* нет элемента */ }
catch (MyIllegalStateException e) { /* Remove без Next */ }
```

Это **полиморфизм исключений** — базовый класс отлавливает все дочерние.

---

## Как этим пользоваться

```csharp
// Создаём коллекцию
var list = new MyArrayList<string>();
list.Add("a"); list.Add("b"); list.Add("c");

// Создаём итератор
var it = new MyArrayListIterator<string>(list);

// Итерируемся вперёд
while (it.HasNext())
{
    string s = it.Next();
    Console.Write(s + " ");  // a b c
}

// Итерируемся назад
while (it.HasPrevious())
{
    string s = it.Previous();
    Console.Write(s + " ");  // c b a
}

// Удаление с удалением
it = new MyArrayListIterator<string>(list);
while (it.HasNext())
{
    string s = it.Next();
    if (s == "b") it.Remove();  // удаляем "b" из списка
}

// Вставка
it = new MyArrayListIterator<string>(list, 1); // начать с позиции 1
it.Add("X");  // вставляет "X" перед позицией 1
```
