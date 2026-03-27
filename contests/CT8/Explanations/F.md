# F. В поисках утраченного кефира

## Введение

Задача на нахождение **минимального пути, проходящего через три заданные вершины** (a, b, c) в неориентированном взвешенном графе. Путь может начинаться в любой вершине и посещать a, b, c в любом порядке.

Решение использует **три запуска алгоритма Дейкстры** (из каждой из вершин a, b, c) и перебор всех перестановок порядка посещения. Для оптимизации применяется **быстрый ввод** и **массив visited**.

## Идея

Ключевое наблюдение: минимальный путь, проходящий через a, b, c, имеет один из шести видов:
- a → b → c
- a → c → b
- b → a → c
- b → c → a
- c → a → b
- c → b → a

Длина пути, например, a → b → c равна `dist(a, b) + dist(b, c)`.

Для вычисления расстояний между всеми парами (a, b, c) достаточно запустить Дейкстру из каждой из трёх вершин.

## Разбор кода

### Быстрый ввод

```csharp
private static class FastReader
{
    private static TextReader reader = Console.In;
    private static Queue<string> tokens = new Queue<string>();

    public static string Next()
    {
        while (tokens.Count == 0)
        {
            string line = reader.ReadLine();
            if (line == null) return null;
            foreach (var token in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                tokens.Enqueue(token);
        }
        return tokens.Dequeue();
    }

    public static int NextInt() => int.Parse(Next());
}
```

`FastReader` читает весь входной поток построчно и разбивает на токены. Это значительно быстрее, чем многократные вызовы `Console.ReadLine()` и `Split()` для каждого ребра.

### Инициализация

```csharp
int n = FastReader.NextInt();
int m = FastReader.NextInt();

var adj = new List<(int to, int weight)>[n + 1];
for (int i = 1; i <= n; i++)
    adj[i] = new List<(int, int)>();

for (int i = 0; i < m; i++)
{
    int u = FastReader.NextInt();
    int v = FastReader.NextInt();
    int w = FastReader.NextInt();
    adj[u].Add((v, w));
    adj[v].Add((u, w));  // Неориентированный граф
}
```

Читаем рёбра через `FastReader` и строим список смежности.

```csharp
int a = FastReader.NextInt();
int b = FastReader.NextInt();
int c = FastReader.NextInt();
```

Читаем три целевые вершины.

### Три запуска Дейкстры

```csharp
long[] distA = Dijkstra(n, adj, a);
long[] distB = Dijkstra(n, adj, b);
long[] distC = Dijkstra(n, adj, c);
```

Вычисляем расстояния от каждой из трёх вершин до всех остальных.

### Функция Дейкстры с оптимизациями

```csharp
private static long[] Dijkstra(int n, List<(int, int)>[] adj, int start)
{
    const long INF = long.MaxValue / 4;
    long[] dist = new long[n + 1];
    bool[] visited = new bool[n + 1];
    
    for (int i = 1; i <= n; i++)
        dist[i] = INF;
    
    dist[start] = 0;

    var pq = new PriorityQueue<int, long>();
    pq.Enqueue(start, 0);
```

- `dist` — расстояния от стартовой вершины
- `visited` — флаг обработки вершины (оптимизация)

```csharp
    while (pq.Count > 0)
    {
        int u = pq.Dequeue();

        // Пропускаем уже обработанные вершины
        if (visited[u])
            continue;
        
        visited[u] = true;
```

**Ключевая оптимизация**: если вершина уже была извлечена из очереди, её расстояние окончательно и повторно обрабатывать её не нужно. Это предотвращает экспоненциальный рост времени на графах с большим количеством рёбер.

```csharp
        foreach (var (v, w) in adj[u])
        {
            if (!visited[v] && dist[u] + w < dist[v])
            {
                dist[v] = dist[u] + w;
                pq.Enqueue(v, dist[v]);
            }
        }
    }

    return dist;
}
```

Релаксация рёбер только для непосещённых вершин.

### Перебор перестановок

```csharp
long ans = INF;

// a -> b -> c
if (distA[b] != INF && distB[c] != INF)
    ans = Math.Min(ans, distA[b] + distB[c]);

// a -> c -> b
if (distA[c] != INF && distC[b] != INF)
    ans = Math.Min(ans, distA[c] + distC[b]);

// ... остальные 4 перестановки
```

Проверяем все 6 перестановок. Для каждой проверяем достижимость промежуточных вершин и обновляем минимальное расстояние.

### Вывод результата

```csharp
if (ans == INF)
    Console.WriteLine(-1);
else
    Console.WriteLine(ans);
```

Если ни один путь не существует, выводим `-1`.

## Оптимизации

1. **FastReader**: буферизированный ввод снижает накладные расходы на парсинг
2. **Массив visited**: предотвращает повторную обработку вершин, критично для графов с большим количеством рёбер
3. **Ранняя проверка visited**: перед релаксацией проверяем, не обработана ли вершина
4. **Три запуска Дейкстры**: O(m log n) каждый, итого O(m log n)
5. **Константное число перестановок**: 6 проверок за O(1)
6. **Временная сложность**: O(m log n) — три запуска Дейкстры + O(m) на ввод
7. **Пространственная сложность**: O(n + m) — три массива расстояний + список смежности

## Резюме

Задача сводится к вычислению расстояний между тремя вершинами и перебору всех порядков их посещения. Три запуска Дейкстры дают все необходимые расстояния, а 6 перестановок покрывают все возможные маршруты. Оптимизации (FastReader и visited) критичны для прохождения жёстких ограничений по времени на больших тестах.
