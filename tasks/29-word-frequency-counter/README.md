# Задача 29: Иерархия интерфейсов

## Что это и зачем

Представь, что у тебя есть куча разных структур данных: `MyArrayList`, `MyVector`, `MyLinkedList`, `MyPriorityQueue`, `MyArrayDeque`, `MyHashSet`, `MyTreeSet`, `MyHashMap`, `MyTreeMap`. Каждая делает что-то своё, но у них есть общие черты:

- Все умеют `Add()`, `Clear()`, `Contains()`, `Size()` и т.д.
- Со списками (`MyArrayList`, `MyVector`, `MyLinkedList`) можно работать по индексу — `Get(0)`, `Set(1, val)`.
- Очереди (`MyPriorityQueue`) умеют `Poll()`, `Peek()`.
- Деки (`MyArrayDeque`) умеют `AddFirst()`, `AddLast()`.
- Множества (`MyHashSet`, `MyTreeSet`) умеют `First()`, `Last()`, `SubSet()`.
- Карты (`MyHashMap`, `MyTreeMap`) работают с парами ключ-значение.

Если описать эти общие черты в виде **интерфейсов**, то:
1. Код становится понятнее — сразу видно, что умеет объект.
2. Можно писать функции, которые работают с любым списком, а не только с `MyArrayList`.
3. Можно сделать UML-диаграмму и увидеть всю архитектуру.

---

## Иерархия целиком

```
IMyCollection<T>                         (базовый: add, clear, contains, size, toArray...)
├── IMyList<T>                           (+ add(index), get, set, indexOf, subList...)
├── IMyQueue<T>                          (+ element, offer, peek, poll)
├── IMyDeque<T>                          (+ addFirst, addLast, push, pop...)
└── IMySet<T>                            (+ first, last, subSet, headSet, tailSet)
     └── IMySortedSet<T>                 (уточняет: first, last, subSet, headSet, tailSet)
          └── IMyNavigableSet<T>         (+ lower, floor, ceiling, higher, pollFirst...)

IMyEntry<K,V>                            (пара ключ-значение)

IMyMap<K,V>                              (put, get, remove, containsKey, keySet...)
└── IMySortedMap<K,V>                    (+ firstKey, lastKey, headMap, subMap, tailMap)
     └── IMyNavigableMap<K,V>            (+ lowerKey, floorKey, ceilingKey, higherKey...)
```

---

## IMYCollection<T> — корень всего

Самый базовый интерфейс. Его реализуют все коллекции (кроме карт).

```csharp
public interface IMyCollection<T>
{
    void Add(T e);
    void AddAll(T[] a);
    void Clear();
    bool Contains(object o);
    bool ContainsAll(T[] a);
    bool IsEmpty();
    bool Remove(object o);
    void RemoveAll(T[] a);
    void RetainAll(T[] a);
    int Size();
    object[] ToArray();
    T[] ToArray(T[] a);
}
```

Каждый метод — это **контракт**: "если ты реализуешь этот интерфейс, ты ОБЯЗАН уметь делать всё это".

---

## IMyList<T> — для списков с индексами

Добавляет методы работы по индексу:

```csharp
void Add(int index, T e);    // вставка в позицию
void AddAll(int index, T[] a);
T Get(int index);            // доступ по индексу
int IndexOf(object o);       // поиск индекса
int LastIndexOf(object o);   // поиск последнего вхождения
T RemoveAt(int index);       // удаление по индексу
T Set(int index, T e);       // замена
IMyList<T> SubList(int from, int to);  // подсписок
```

Реализуют: `MyArrayList`, `MyVector`, `MyLinkedList`.

---

## IMyQueue<T> — для очередей

Добавляет методы работы по принципу FIFO (первый пришёл — первый ушёл):

```csharp
T Element();      // посмотреть первый элемент (ошибка если пусто)
bool Offer(T obj); // попробовать добавить
T Peek();         // посмотреть первый элемент (null если пусто)
T Poll();         // достать первый элемент (null если пусто)
```

Реализует: `MyPriorityQueue`.

Разница между `Element()` и `Peek()`, `Offer()` и `Add()`:
- `Element()` / `Add()` — кидают исключение, если что-то пошло не так
- `Peek()` / `Offer()` — возвращают null/false, без исключений

---

## IMyDeque<T> — для двунаправленных очередей

Можно работать и с головой, и с хвостом:

```csharp
void AddFirst(T obj);    // вставить в начало
void AddLast(T obj);     // вставить в конец
T GetFirst();            // получить первый
T GetLast();             // получить последний
T RemoveFirst();         // удалить первый
T RemoveLast();          // удалить последний
void Push(T obj);        // положить на стек (в начало)
T Pop();                 // снять со стека (с начала)
T PollFirst();           // достать первый (null если пусто)
T PollLast();            // достать последний (null если пусто)
T PeekFirst();           // посмотреть первый (null если пусто)
T PeekLast();            // посмотреть последний (null если пусто)
bool OfferFirst(T obj);  // попробовать вставить в начало
bool OfferLast(T obj);   // попробовать вставить в конец
bool RemoveFirstOccurrence(object obj);  // удалить первое вхождение
bool RemoveLastOccurrence(object obj);   // удалить последнее вхождение
```

Реализует: `MyArrayDeque` (и `IMyList`, и `IMyDeque`).

**Важно:** Deque можно использовать и как очередь (`AddLast` / `PollFirst`), и как стек (`Push` / `Pop`), и как двустороннюю очередь.

---

## IMySet<T> / IMySortedSet<T> / IMyNavigableSet<T> — для множеств

Множество — это коллекция **уникальных** элементов.

```csharp
// IMySet<T> — базовое множество
T First();
T Last();
IMySet<T> SubSet(T from, T to);
IMySet<T> HeadSet(T to);
IMySet<T> TailSet(T from);

// IMySortedSet<T> — то же самое (уточняет типы возврата)

// IMyNavigableSet<T> — навигация
T Lower(T e);      // наибольший элемент < e
T Floor(T e);      // наибольший элемент <= e
T Ceiling(T e);    // наименьший элемент >= e
T Higher(T e);     // наименьший элемент > e
T PollFirst();     // удалить и вернуть первый
T PollLast();      // удалить и вернуть последний
```

Реализуют: `MyHashSet` (IMySet), `MyTreeSet` (IMyNavigableSet).

---

## IMyMap<K,V> / IMySortedMap<K,V> / IMyNavigableMap<K,V> — для карт

Карта хранит пары **ключ → значение**.

```csharp
// IMyMap<K,V>
void Clear();
bool ContainsKey(object key);
bool ContainsValue(object value);
IMySet<IMyEntry<K,V>> EntrySet();
V Get(object key);
bool IsEmpty();
IMySet<K> KeySet();
void Put(K key, V value);
void PutAll(IMyMap<K,V> m);
V Remove(object key);
int Size();
IMyCollection<V> Values();

// IMySortedMap<K,V> — сортированная карта
K FirstKey();
K LastKey();
IMySortedMap<K,V> HeadMap(K end);
IMySortedMap<K,V> SubMap(K start, K end);
IMySortedMap<K,V> TailMap(K start);

// IMyNavigableMap<K,V> — навигация
K LowerKey(K key);
K FloorKey(K key);
K CeilingKey(K key);
K HigherKey(K key);
```

Реализуют: `MyHashMap` (IMyMap), `MyTreeMap` (IMyNavigableMap).

---

## Почему интерфейсы, а не абстрактные классы?

В C# у класса может быть только один родитель, но **много интерфейсов**.

С абстрактным классом:
```csharp
public abstract class MyCollection<T> { ... }
public class MyArrayList<T> : MyCollection<T> { }  // ✅ ок
public class MyArrayDeque<T> : MyCollection<T>, ?   // ❌ а как добавить Deque?
```

С интерфейсами:
```csharp
public class MyArrayList<T> : IMyList<T> { }        // список
public class MyArrayDeque<T> : IMyList<T>, IMyDeque<T> { }  // и список, и дека!
```

`MyArrayDeque` одновременно **список** (есть индексы) и **двунаправленная очередь** (есть addFirst/addLast). Без интерфейсов такое не сделать.

---

## MyLinkedList — что это и как работает

Это двунаправленный связный список. Каждый элемент (узел) хранит:
- `data` — сами данные
- `prev` — ссылка на предыдущий узел (null у первого)
- `next` — ссылка на следующий узел (null у последнего)

```
null ← [a] ⇄ [b] ⇄ [c] → null
        ↑                ↑
       head            tail
```

**Преимущества:**
- Вставка и удаление в середине — O(1) (если уже есть ссылка на узел)
- Не надо выделять большой массив заранее

**Недостатки:**
- Доступ по индексу — O(n) (нужно пройти от начала)
- Каждый элемент занимает больше памяти (две ссылки)

Метод `GetNode(int index)` оптимизирован — идёт от головы, если индекс < size/2, и от хвоста, если индекс >= size/2. Это даёт O(n/2) вместо O(n).

```csharp
private Node GetNode(int index)
{
    if (index < size / 2) // ближе к началу — идём от head
    {
        current = head;
        for (int i = 0; i < index; i++) current = current.next;
    }
    else // ближе к концу — идём от tail
    {
        current = tail;
        for (int i = size - 1; i > index; i--) current = current.prev;
    }
    return current;
}
```

---

## UML-диаграмма

Должна быть нарисована отдельно (в PDF или на картинке). Показывает:

```
<<interface>>              <<interface>>
IMyCollection<T>           IMyMap<K,V>
    ↑ extends                   ↑ extends
    |                          ...
IMyList<T>              IMySortedMap<K,V>
    ↑ implements               ↑ extends
    |                          |
MyArrayList<T> ---┐      IMyNavigableMap<K,V>
MyVector<T> ------┤           ↑ implements
MyLinkedList<T> --┤           |
                  |      MyTreeMap<K,V>
IMyQueue<T> ------┤
    ↑ implements   |
MyPriorityQueue<T> |

IMyDeque<T> -------┤
    ↑ implements   |
MyArrayDeque<T> ---┤ (также IMyList)

IMySet<T> ---------┤
    ↑ extends      |
IMySortedSet<T> ---┤
    ↑ extends      |
IMyNavigableSet<T> |
    ↑ implements   |
MyTreeSet<T> ------┤
MyHashSet<T> ------┤ (IMySet)
```

Каждая стрелка показывает: "этот класс реализует этот интерфейс" или "этот интерфейс расширяет другой".
