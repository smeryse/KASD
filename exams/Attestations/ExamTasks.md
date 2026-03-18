## Задачи на алгоритмы
### 1. Минимальный элемент за один проход
**Формулировка:** массив целых чисел длины n (могут быть отрицательные и повторы). Найти минимальный элемент, его количество и индекс самого левого вхождения за один проход. Обосновать нижнюю оценку `Ω(n)`.  
**Алгоритм:** храним `min`, `count`, `first`. При встрече меньшего значения — обновляем всё, при равном — увеличиваем счётчик. Любой алгоритм обязан прочитать каждый элемент, иначе можно пропустить минимум ⇒ `Ω(n)`.  
```csharp
(int min, int count, int first) FindMinInfo(int[] a)
{
    if (a.Length == 0)
        throw new ArgumentException("empty");

    int min = a[0];
    int count = 1;
    int first = 0;

    for (int i = 1; i < a.Length; i++)
    {
        if (a[i] < min)
        {
            min = a[i];
            count = 1;
            first = i;
        }
        else if (a[i] == min)
        {
            count++;
        }
    }

    return (min, count, first);
}
```

### 2. Два неубывающих массива, максимальное число пар суммы
**Вопрос:** даны два неубывающих массива `a` и `b` длины n. Найти значение `S`, для которого число пар `(i, j)` с `a[i] + b[j] = S` максимально (в лекции требование — линейно по числу элементов, поэтому используем сжатие повторов).  
**Алгоритм:** сжимаем каждый массив в пары `(значение, кратность)` (run-length encoding). Перебираем все комбинации блоков: `pairs = cntA * cntB`, суммируем их по `sum = valA + valB` в словарь. Количество блоков не превышает n, поэтому сложность `O(Ua·Ub)`, где `U` — число разных значений (в худшем случае `O(n^2)`, в лекции — линейно при малом числе блоков).  
```csharp
static List<(int val, int cnt)> Compress(int[] arr)
{
    var res = new List<(int val, int cnt)>();
    for (int i = 0; i < arr.Length;)
    {
        int v = arr[i];
        int j = i;
        while (j < arr.Length && arr[j] == v) j++;
        res.Add((v, j - i));
        i = j;
    }
    return res;
}

(int s, long pairs) MaxPairs(int[] a, int[] b)
{
    var ar = Compress(a);
    var br = Compress(b);

    var counts = new Dictionary<int, long>();
    foreach (var (va, ca) in ar)
    {
        foreach (var (vb, cb) in br)
        {
            int sum = va + vb;
            counts.TryGetValue(sum, out var cur);
            counts[sum] = cur + (long)ca * cb;
        }
    }

    int bestS = 0;
    long bestCount = -1;
    foreach (var kv in counts)
    {
        if (kv.Value > bestCount)
        {
            bestCount = kv.Value;
            bestS = kv.Key;
        }
    }
    return (bestS, bestCount);
}
```

### 3. Количество инверсий и сортировка за `O(n log n)`
**Формулировка:** посчитать число инверсий и выдать отсортированный массив за `O(n log n)` модификацией merge sort.  
**Алгоритм:** рекурсивный merge sort; при слиянии, если берём элемент из правой половины, добавляем в ответ оставшуюся длину левой.  
```csharp
(long inv, int[] arr) SortCount(int[] a)
{
    if (a.Length <= 1)
        return (0, a.ToArray());

    int mid = a.Length / 2;
    var left = a[..mid];
    var right = a[mid..];

    var (lInv, lArr) = SortCount(left);
    var (rInv, rArr) = SortCount(right);

    long invCount = lInv + rInv;
    int[] res = new int[a.Length];
    int i = 0, j = 0, k = 0;

    while (i < lArr.Length && j < rArr.Length)
    {
        if (lArr[i] <= rArr[j])
        {
            res[k++] = lArr[i++];
        }
        else
        {
            res[k++] = rArr[j++];
            invCount += lArr.Length - i;
        }
    }

    while (i < lArr.Length) res[k++] = lArr[i++];
    while (j < rArr.Length) res[k++] = rArr[j++];

    return (invCount, res);
}
```

### 4. Очередь с приоритетом на куче с уменьшением ключа
**Формулировка:** последовательность операций add, extract‑min, уменьшить ключ по индексу; каждая за `O(log n)`.  
**Алгоритм:** куча в массиве + словарь `pos[id]`, хранящий позицию элемента. `DecreaseKey` меняет значение и поднимает вверх.  
```csharp
class BinaryHeap
{
    private readonly List<(int val, int id)> heap = new();
    private readonly Dictionary<int, int> pos = new();

    private void Swap(int i, int j)
    {
        (heap[i], heap[j]) = (heap[j], heap[i]);
        pos[heap[i].id] = i;
        pos[heap[j].id] = j;
    }

    private void SiftUp(int i)
    {
        while (i > 0)
        {
            int p = (i - 1) / 2;
            if (heap[p].val <= heap[i].val) break;
            Swap(i, p);
            i = p;
        }
    }

    private void SiftDown(int i)
    {
        while (true)
        {
            int l = 2 * i + 1;
            int r = l + 1;
            int best = i;
            if (l < heap.Count && heap[l].val < heap[best].val) best = l;
            if (r < heap.Count && heap[r].val < heap[best].val) best = r;
            if (best == i) break;
            Swap(i, best);
            i = best;
        }
    }

    public void Insert(int id, int value)
    {
        heap.Add((value, id));
        pos[id] = heap.Count - 1;
        SiftUp(heap.Count - 1);
    }

    public (int value, int id) Extract()
    {
        var root = heap[0];
        Swap(0, heap.Count - 1);
        heap.RemoveAt(heap.Count - 1);
        pos.Remove(root.id);
        if (heap.Count > 0) SiftDown(0);
        return root;
    }

    public void DecreaseKey(int id, int newValue)
    {
        int i = pos[id];
        if (newValue > heap[i].val) throw new ArgumentException("key not decreased");
        heap[i] = (newValue, id);
        SiftUp(i);
    }
}
```

### 5. Рандомизированный быстрый выбор k‑й статистики
**Формулировка:** найти k‑ю статистику QuickSelect с рандомизацией; показать плохой фиксированный тест.  
**Алгоритм:** случайный pivot, partition, рекурсируемся в нужную часть. Фиксированный “первый элемент” ломается на отсортированном массиве (`O(n^2)`), рандомизация делает это маловероятным (ожидаемо `O(n)`).  
```csharp
int Partition(int[] a, int l, int r, int p)
{
    (a[p], a[r]) = (a[r], a[p]);
    int x = a[r];
    int store = l;
    for (int i = l; i < r; i++)
    {
        if (a[i] <= x)
        {
            (a[i], a[store]) = (a[store], a[i]);
            store++;
        }
    }
    (a[store], a[r]) = (a[r], a[store]);
    return store;
}

int QuickSelect(int[] a, int k, Random rng)
{
    int l = 0, r = a.Length - 1;
    while (true)
    {
        int p = rng.Next(l, r + 1);
        int pos = Partition(a, l, r, p);
        if (pos == k) return a[pos];
        if (k < pos) r = pos - 1; else l = pos + 1;
    }
}
```

### 6. Lower/Upper bound бинарным поиском с повторами
**Формулировка:** для множества запросов найти нижнюю и верхнюю границы значения x в отсортированном массиве.  
**Алгоритм:** стандартные бинарные поиски; при `a[m]` недостаёт — сдвиг левой границы, иначе правой.  
```csharp
int Lower(int[] a, int x)
{
    int l = 0, r = a.Length;
    while (l < r)
    {
        int m = (l + r) / 2;
        if (a[m] < x) l = m + 1; else r = m;
    }
    return l;
}

int Upper(int[] a, int x)
{
    int l = 0, r = a.Length;
    while (l < r)
    {
        int m = (l + r) / 2;
        if (a[m] <= x) l = m + 1; else r = m;
    }
    return l;
}
```

### 7. Бинарный поиск по ответу
**Формулировка:** есть монотонный `Check(x)` (“можно ли произвести x изделий”). Найти максимум x.  
**Алгоритм:** найти верхнюю границу (удвоением), затем бинарный поиск по ответу.  
```csharp
int MaxAnswer(int lo, int hi, Func<int, bool> ok)
{
    while (lo < hi)
    {
        int mid = (lo + hi + 1) / 2;
        if (ok(mid)) lo = mid; else hi = mid - 1;
    }
    return lo;
}
```

### 8. Очередь на двух стеках + амортизированный анализ
**Формулировка:** реализовать очередь на двух стеках, показать `O(1)` амортизированно.  
**Алгоритм:** вставки в `back`, удаления из `front`; при пустом `front` переливаем всё из `back`. Каждый элемент переносится не чаще одного раза ⇒ амортизированная константа.  
```csharp
class TwoStackQueue<T>
{
    private readonly Stack<T> front = new();
    private readonly Stack<T> back = new();

    public void Enqueue(T x) => back.Push(x);

    public T Dequeue()
    {
        if (front.Count == 0)
        {
            while (back.Count > 0) front.Push(back.Pop());
        }
        return front.Pop();
    }
}
```

### 9. DSU с эвристиками
**Формулировка:** реализовать `Union/Find` с рангом и сжатием путей, объяснить амортизированную близость к константе (`α(n)`).  
**Алгоритм:** ранг ограничивает высоту, сжатие путей подтягивает к корню; амортизированная сложность `O(α(n))`.  
```csharp
int Find(int v)
{
    if (parent[v] == v) return v;
    parent[v] = Find(parent[v]);
    return parent[v];
}

void Union(int a, int b)
{
    a = Find(a); b = Find(b);
    if (a == b) return;
    if (rank[a] < rank[b]) (a, b) = (b, a);
    parent[b] = a;
    if (rank[a] == rank[b]) rank[a]++;
}
```

### 10. Рюкзак 0/1: `O(n·W)`, память `O(W)`, восстановление
**Вопрос:** решить DP, оптимизировать память до одного массива, восстановить одно оптимальное решение без хранения полной таблицы.  
**Алгоритм:** классический одномерный `dp[weight]`. Для восстановления храним для каждого веса только предыдущее состояние (`prevWeight`) и предмет, которым улучшили значение (`fromItem`). Это добавляет `O(W)` памяти без матрицы `n·W`.  
```csharp
int Knapsack01(int[] w, int[] c, int W, List<int> picked)
{
    int n = w.Length;
    int[] dp = new int[W + 1];
    int[] prevWeight = Enumerable.Repeat(-1, W + 1).ToArray();
    int[] fromItem = Enumerable.Repeat(-1, W + 1).ToArray();

    for (int i = 0; i < n; i++)
    {
        for (int ww = W; ww >= w[i]; ww--)
        {
            int cand = dp[ww - w[i]] + c[i];
            if (cand > dp[ww])
            {
                dp[ww] = cand;
                prevWeight[ww] = ww - w[i];
                fromItem[ww] = i;
            }
        }
    }

    int bestWeight = 0;
    for (int ww = 1; ww <= W; ww++)
        if (dp[ww] > dp[bestWeight]) bestWeight = ww;

    for (int ww = bestWeight; ww != -1 && fromItem[ww] != -1;)
    {
        int item = fromItem[ww];
        picked.Add(item);
        ww = prevWeight[ww];
    }
    picked.Reverse();
    return dp[bestWeight];
}
```

### 11. Сегдерево: минимум и количество минимумов
**Формулировка:** дерево отрезков возвращает минимум и число его вхождений на отрезке; поддерживает точечное обновление.  
**Алгоритм:** узел хранит `(min, cnt)`. Комбинация: если min равны — складываем cnt, иначе берём меньший min и его cnt. Запрос/обновление — `O(log n)`.  
```csharp
struct Node { public int Min; public int Cnt; }

Node Combine(Node a, Node b)
{
    if (a.Min == b.Min) return new Node { Min = a.Min, Cnt = a.Cnt + b.Cnt };
    return a.Min < b.Min ? a : b;
}
```

### 12. Фенвик: суммы и поиск префикса
**Формулировка:** реализовать дерево Фенвика (сумма на префиксе, точечное обновление), пояснить `i & -i`; найти наибольший префикс с суммой ≤ x (неотрицательные элементы).  
**Алгоритм:** `Add/Sum` стандартные; поиск — двоичный подъём по степеням двойки, двигаемся, пока можем “съесть” блок.  
```csharp
int FindPrefix(long x, int n, long[] bit)
{
    int pos = 0;
    int bitLen = 1;
    while ((bitLen << 1) <= n) bitLen <<= 1;
    for (; bitLen > 0; bitLen >>= 1)
    {
        int next = pos + bitLen;
        if (next <= n && bit[next] <= x)
        {
            x -= bit[next];
            pos = next;
        }
    }
    return pos;
}
```

### 13. LCA бинарными подъёмами
**Формулировка:** дерево с корнем, отвечать LCA за `O(log n)`, предобработка за `O(n log n)`, корректно, если одна вершина — предок другой.  
**Алгоритм:** DFS заполняет `up[v][k]` и `depth[v]`. Запрос: выравниваем глубины, затем поднимаем обоих от старших степеней к младшим, пока предки не совпадут; родитель совпавших — ответ (если после выравнивания вершины совпали — предок найден сразу).  
```csharp
int Lca(int a, int b)
{
    if (depth[a] < depth[b]) (a, b) = (b, a);
    int diff = depth[a] - depth[b];
    for (int k = LOG - 1; k >= 0; k--)
    {
        if (((diff >> k) & 1) == 1) a = up[a, k];
    }
    if (a == b) return a;
    for (int k = LOG - 1; k >= 0; k--)
    {
        if (up[a, k] != up[b, k])
        {
            a = up[a, k];
            b = up[b, k];
        }
    }
    return up[a, 0];
}
```

### 14. КСС и минимальные дуги до сильной связности
**Формулировка:** найти компоненты сильной связности, построить конденсацию, определить минимум дуг для сильной связности.  
**Алгоритм:** Косарайю/Таръян. Считаем числа компонент без входящих (sources) и без исходящих (sinks) в DAG конденсации. Если компонент одна — ответ 0, иначе `max(sources, sinks)`.  
```csharp
int MinEdgesToStronglyConnect(List<int>[] g)
{
    int n = g.Length;
    var order = new List<int>();
    bool[] used = new bool[n];
    void Dfs1(int v)
    {
        used[v] = true;
        foreach (var to in g[v]) if (!used[to]) Dfs1(to);
        order.Add(v);
    }
    for (int v = 0; v < n; v++) if (!used[v]) Dfs1(v);

    var gt = new List<int>[n];
    for (int i = 0; i < n; i++) gt[i] = new List<int>();
    for (int v = 0; v < n; v++) foreach (var to in g[v]) gt[to].Add(v);

    int[] comp = Enumerable.Repeat(-1, n).ToArray();
    void Dfs2(int v, int c)
    {
        comp[v] = c;
        foreach (var to in gt[v]) if (comp[to] == -1) Dfs2(to, c);
    }
    int comps = 0;
    for (int idx = order.Count - 1; idx >= 0; idx--)
        if (comp[order[idx]] == -1) Dfs2(order[idx], comps++);

    if (comps == 1) return 0;

    var inDeg = new int[comps];
    var outDeg = new int[comps];
    for (int v = 0; v < n; v++)
        foreach (var to in g[v])
            if (comp[v] != comp[to])
            {
                outDeg[comp[v]]++;
                inDeg[comp[to]]++;
            }

    int sources = inDeg.Count(x => x == 0);
    int sinks = outDeg.Count(x => x == 0);
    return Math.Max(sources, sinks);
}
```

### 15. Мосты и точки; рёберная двусвязность
**Формулировка:** найти все мосты и точки сочленения за линейное время; по мостам определить, сколько рёбер добавить для рёберной двусвязности, и предложить способ.  
**Алгоритм:** DFS с tin/low. Сжимаем граф по мостам в дерево; в нём считаем листья — нужно `ceil(leaves/2)` рёбер, соединяем листья попарно.  
```csharp
(List<(int, int)> bridges, int edgesToAdd) BridgesAndBiconnect(List<int>[] g)
{
    int n = g.Length;
    int timer = 0;
    var tin = Enumerable.Repeat(-1, n).ToArray();
    var low = new int[n];
    var bridgesList = new List<(int, int)>();

    void Dfs(int v, int p)
    {
        tin[v] = low[v] = timer++;
        foreach (var to in g[v])
        {
            if (to == p) continue;
            if (tin[to] != -1)
            {
                low[v] = Math.Min(low[v], tin[to]);
            }
            else
            {
                Dfs(to, v);
                low[v] = Math.Min(low[v], low[to]);
                if (low[to] > tin[v]) bridgesList.Add((v, to));
            }
        }
    }
    for (int v = 0; v < n; v++) if (tin[v] == -1) Dfs(v, -1);

    // сжимаем по мостам
    int compId = 0;
    var comp = Enumerable.Repeat(-1, n).ToArray();
    var bridgeSet = new HashSet<(int, int)>(bridgesList.Select(b => b.Item1 < b.Item2 ? b : (b.Item2, b.Item1)));
    void Paint(int v, int id)
    {
        comp[v] = id;
        foreach (var to in g[v])
        {
            var e = v < to ? (v, to) : (to, v);
            if (bridgeSet.Contains(e)) continue;
            if (comp[to] == -1) Paint(to, id);
        }
    }
    for (int v = 0; v < n; v++) if (comp[v] == -1) Paint(v, compId++);

    if (compId == 1) return (bridgesList, 0);

    var deg = new int[compId];
    foreach (var (u, v) in bridgesList)
    {
        int cu = comp[u], cv = comp[v];
        deg[cu]++; deg[cv]++;
    }
    int leaves = deg.Count(x => x == 1);
    int need = (leaves + 1) / 2;
    return (bridgesList, need);
}
```

### 16. Левенштейн + восстановление
**Формулировка:** две строки s и t, посчитать расстояние Левенштейна, вывести одну оптимальную последовательность операций.  
**Алгоритм:** DP таблица `dp[i][j]` с переходами вставка/удаление/замена; восстановление — идём от (n,m) к (0,0), выбирая ход, давший минимум (инсерты, делиты, замены/совпадения).  
```csharp
(int dist, List<string> ops) LevenshteinWithPath(string s, string t)
{
    int n = s.Length, m = t.Length;
    int[,] dp = new int[n + 1, m + 1];
    for (int i = 0; i <= n; i++) dp[i, 0] = i;
    for (int j = 0; j <= m; j++) dp[0, j] = j;

    for (int i = 1; i <= n; i++)
        for (int j = 1; j <= m; j++)
        {
            int cost = s[i - 1] == t[j - 1] ? 0 : 1;
            dp[i, j] = Math.Min(
                Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                dp[i - 1, j - 1] + cost);
        }

    var path = new List<string>();
    int x = n, y = m;
    while (x > 0 || y > 0)
    {
        if (x > 0 && dp[x, y] == dp[x - 1, y] + 1)
        {
            path.Add($"Del '{s[x - 1]}' at {x - 1}");
            x--;
        }
        else if (y > 0 && dp[x, y] == dp[x, y - 1] + 1)
        {
            path.Add($"Ins '{t[y - 1]}' at {x}");
            y--;
        }
        else
        {
            if (s[x - 1] != t[y - 1])
                path.Add($"Sub '{s[x - 1]}' -> '{t[y - 1]}' at {x - 1}");
            x--; y--;
        }
    }
    path.Reverse();
    return (dp[n, m], path);
}
```

### 17. Левенштейн с двумя строками памяти
**Формулировка:** посчитать то же расстояние, но хранить только две строки; объяснить, почему путь не восстановить.  
**Алгоритм:** используем предыдущую/текущую строки; значения вне этих строк теряются, поэтому без дополнительного логирования восстановить путь нельзя (только расстояние остаётся верным).  
```csharp
int LevenshteinBandTwoRows(string s, string t)
{
    int n = s.Length, m = t.Length;
    int[] prev = new int[m + 1];
    int[] cur = new int[m + 1];
    for (int j = 0; j <= m; j++) prev[j] = j;

    for (int i = 1; i <= n; i++)
    {
        cur[0] = i;
        for (int j = 1; j <= m; j++)
        {
            int cost = s[i - 1] == t[j - 1] ? 0 : 1;
            cur[j] = Math.Min(
                Math.Min(prev[j] + 1, cur[j - 1] + 1),
                prev[j - 1] + cost);
        }
        (prev, cur) = (cur, prev);
    }
    return prev[m];
}
// Для восстановления пути нужно хранить матрицу или хотя бы дополнительные ссылки;
// двух строк памяти недостаточно — прежние решения стерты.
```

### 18. Левенштейн с ограничением d
**Формулировка:** проверить наличие строки t на расстоянии ≤ d при ограничениях на позиции; использовать d для ускорения.  
**Алгоритм:** считаем DP только в полосе `|i-j|<=d`; если в строке все значения > d, можно остановиться. Это даёт `O(d·n)`.  
```csharp
int LevenshteinLimited(string s, string t, int d)
{
    int n = s.Length, m = t.Length;
    const int INF = 1_000_000;
    int[] prev = Enumerable.Repeat(INF, m + 1).ToArray();
    int[] cur = Enumerable.Repeat(INF, m + 1).ToArray();
    for (int j = 0; j <= Math.Min(m, d); j++) prev[j] = j;

    for (int i = 1; i <= n; i++)
    {
        Array.Fill(cur, INF);
        int from = Math.Max(1, i - d);
        int to = Math.Min(m, i + d);
        cur[from - 1] = INF;
        if (from == 1) cur[0] = i;
        for (int j = from; j <= to; j++)
        {
            int cost = s[i - 1] == t[j - 1] ? 0 : 1;
            cur[j] = Math.Min(
                Math.Min(prev[j] + 1, cur[j - 1] + 1),
                prev[j - 1] + cost);
        }
        (prev, cur) = (cur, prev);
    }
    return prev[m];
}
```

### 19. Кузнечик `O(n·k)` с восстановлением
**Формулировка:** кузнечик прыгает 1..k клеток вперёд, каждая клетка имеет цену; найти минимальную стоимость пути до n и восстановить маршрут.  
**Алгоритм:** `dp[i]=min_{j=1..k}(dp[i-j]+cost[i])`, храним `prev[i]` — откуда пришли, для восстановления.  
```csharp
(int cost, List<int> path) FrogNaive(int[] price, int k)
{
    int n = price.Length;
    int[] dp = Enumerable.Repeat(int.MaxValue / 2, n).ToArray();
    int[] prev = Enumerable.Repeat(-1, n).ToArray();
    dp[0] = price[0];
    for (int i = 1; i < n; i++)
    {
        for (int j = 1; j <= k && i - j >= 0; j++)
        {
            int cand = dp[i - j] + price[i];
            if (cand < dp[i])
            {
                dp[i] = cand;
                prev[i] = i - j;
            }
        }
    }
    var route = new List<int>();
    for (int v = n - 1; v != -1; v = prev[v]) route.Add(v);
    route.Reverse();
    return (dp[n - 1], route);
}
```

### 20. Кузнечик быстрее `O(n·k)`
**Формулировка:** та же задача, но ускорить за счёт структуры данных.  
**Алгоритм:** монотонная очередь минимумов окна длины k (`O(n)`) или приоритетная очередь (`O(n log k)`) с удалением устаревших элементов, текущий минимум + cost — новое dp.  
```csharp
int FrogWithDeque(int[] price, int k)
{
    int n = price.Length;
    int[] dp = new int[n];
    dp[0] = price[0];
    var dq = new LinkedList<int>(); // индексы с возрастающими dp
    dq.AddLast(0);

    for (int i = 1; i < n; i++)
    {
        while (dq.Count > 0 && dq.First!.Value < i - k) dq.RemoveFirst();
        dp[i] = dp[dq.First!.Value] + price[i];
        while (dq.Count > 0 && dp[dq.Last!.Value] >= dp[i]) dq.RemoveLast();
        dq.AddLast(i);
    }
    return dp[n - 1];
}
```

## Задачи на структуры данных
Код ниже приведён для каждой структуры.

### 21. MyArrayList
```csharp
public class MyArrayList<T>
{
    private T[] data;
    public int Count { get; private set; }

    public MyArrayList(int capacity = 4)
    {
        data = new T[capacity];
    }

    private void Ensure(int need)
    {
        if (need <= data.Length) return;
        int newCap = Math.Max(need, data.Length * 2);
        Array.Resize(ref data, newCap);
    }

    public T this[int index]
    {
        get => data[index];
        set => data[index] = value;
    }

    public void Add(T item)
    {
        Ensure(Count + 1);
        data[Count++] = item;
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > Count) throw new ArgumentOutOfRangeException(nameof(index));
        Ensure(Count + 1);
        Array.Copy(data, index, data, index + 1, Count - index);
        data[index] = item;
        Count++;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
        Array.Copy(data, index + 1, data, index, Count - index - 1);
        Count--;
    }

    public int IndexOf(T item)
    {
        var cmp = EqualityComparer<T>.Default;
        for (int i = 0; i < Count; i++)
            if (cmp.Equals(data[i], item)) return i;
        return -1;
    }

    public override bool Equals(object? obj) =>
        obj is MyArrayList<T> other &&
        Count == other.Count &&
        data.Take(Count).SequenceEqual(other.data.Take(other.Count));

    public override int GetHashCode() =>
        HashCode.Combine(Count, data.Take(Count).Aggregate(0, HashCode.Combine));

    public override string ToString() => $"[{string.Join(", ", data.Take(Count))}]";
}
```

### 22. MyLinkedList
```csharp
public class MyLinkedList<T>
{
    private class Node { public T Val; public Node? Prev, Next; public Node(T v){ Val = v; } }
    private Node? head, tail;
    public int Count { get; private set; }

    public void AddLast(T v)
    {
        var node = new Node(v) { Prev = tail };
        if (tail != null) tail.Next = node; else head = node;
        tail = node;
        Count++;
    }

    public void AddFirst(T v)
    {
        var node = new Node(v) { Next = head };
        if (head != null) head.Prev = node; else tail = node;
        head = node;
        Count++;
    }

    public bool Remove(T v)
    {
        var cmp = EqualityComparer<T>.Default;
        for (var cur = head; cur != null; cur = cur.Next)
        {
            if (!cmp.Equals(cur.Val, v)) continue;
            if (cur.Prev != null) cur.Prev.Next = cur.Next; else head = cur.Next;
            if (cur.Next != null) cur.Next.Prev = cur.Prev; else tail = cur.Prev;
            Count--;
            return true;
        }
        return false;
    }

    private Node GetNode(int index)
    {
        if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
        if (index < Count / 2)
        {
            var cur = head!;
            for (int i = 0; i < index; i++) cur = cur.Next!;
            return cur;
        }
        else
        {
            var cur = tail!;
            for (int i = Count - 1; i > index; i--) cur = cur.Prev!;
            return cur;
        }
    }

    public T this[int index]
    {
        get => GetNode(index).Val;
        set => GetNode(index).Val = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not MyLinkedList<T> other || other.Count != Count) return false;
        var a = head; var b = other.head;
        var cmp = EqualityComparer<T>.Default;
        while (a != null)
        {
            if (!cmp.Equals(a.Val, b!.Val)) return false;
            a = a.Next; b = b.Next;
        }
        return true;
    }

    public override int GetHashCode() => HashCode.Combine(Count, ToString());

    public override string ToString() => $"[{string.Join(" <-> ", Enumerate())}]";

    private IEnumerable<T> Enumerate()
    {
        for (var cur = head; cur != null; cur = cur.Next) yield return cur.Val;
    }
}
```

### 23. MyStack
```csharp
public class MyStack<T>
{
    private readonly List<T> data = new();
    public int Count => data.Count;

    public void Push(T item) => data.Add(item);

    public T Pop()
    {
        if (Count == 0) throw new InvalidOperationException("empty");
        var v = data[^1];
        data.RemoveAt(Count - 1);
        return v;
    }

    public T Peek()
    {
        if (Count == 0) throw new InvalidOperationException("empty");
        return data[^1];
    }

    public override bool Equals(object? obj) =>
        obj is MyStack<T> other && data.SequenceEqual(other.data);

    public override int GetHashCode() => data.Aggregate(Count, HashCode.Combine);

    public static bool operator ==(MyStack<T> a, MyStack<T> b) => a.Equals(b);
    public static bool operator !=(MyStack<T> a, MyStack<T> b) => !a.Equals(b);

    public override string ToString() => $"bottom [{string.Join(", ", data)}] <- top";
}
```

### 24. MyQueue (кольцевой буфер)
```csharp
public class MyQueue<T>
{
    private T[] data;
    private int head, tail;
    public int Count { get; private set; }

    public MyQueue(int capacity = 4)
    {
        data = new T[capacity];
    }

    private void Ensure()
    {
        if (Count < data.Length) return;
        T[] nd = new T[data.Length * 2];
        for (int i = 0; i < Count; i++) nd[i] = this[i];
        data = nd; head = 0; tail = Count;
    }

    public void Enqueue(T item)
    {
        Ensure();
        data[tail] = item;
        tail = (tail + 1) % data.Length;
        Count++;
    }

    public T Dequeue()
    {
        if (Count == 0) throw new InvalidOperationException("empty");
        var v = data[head];
        head = (head + 1) % data.Length;
        Count--;
        return v;
    }

    public T Peek()
    {
        if (Count == 0) throw new InvalidOperationException("empty");
        return data[head];
    }

    public T this[int index] => data[(head + index) % data.Length];

    public override bool Equals(object? obj) =>
        obj is MyQueue<T> other && Count == other.Count &&
        Enumerable.Range(0, Count).All(i => EqualityComparer<T>.Default.Equals(this[i], other[i]));

    public override int GetHashCode() => data.Aggregate(Count, HashCode.Combine);

    public override string ToString() => $"head [{string.Join(", ", Enumerable.Range(0, Count).Select(i => this[i]))}] tail";
}
```

### 25. MyArrayDeque
```csharp
public class MyArrayDeque<T>
{
    private T[] data;
    private int head, tail;
    public int Count { get; private set; }

    public MyArrayDeque(int capacity = 4)
    {
        data = new T[capacity];
    }

    private int Mask(int i) => (i + data.Length) % data.Length;

    private void Ensure()
    {
        if (Count < data.Length) return;
        T[] nd = new T[data.Length * 2];
        for (int i = 0; i < Count; i++) nd[i] = this[i];
        data = nd; head = 0; tail = Count;
    }

    public void AddFirst(T item)
    {
        Ensure();
        head = Mask(head - 1);
        data[head] = item;
        Count++;
    }

    public void AddLast(T item)
    {
        Ensure();
        data[tail] = item;
        tail = Mask(tail + 1);
        Count++;
    }

    public T RemoveFirst()
    {
        if (Count == 0) throw new InvalidOperationException("empty");
        var v = data[head];
        head = Mask(head + 1);
        Count--;
        return v;
    }

    public T RemoveLast()
    {
        if (Count == 0) throw new InvalidOperationException("empty");
        tail = Mask(tail - 1);
        var v = data[tail];
        Count--;
        return v;
    }

    public T this[int index] => data[Mask(head + index)];

    public override bool Equals(object? obj) =>
        obj is MyArrayDeque<T> other && Count == other.Count &&
        Enumerable.Range(0, Count).All(i => EqualityComparer<T>.Default.Equals(this[i], other[i]));

    public override int GetHashCode() => data.Aggregate(Count, HashCode.Combine);

    public override string ToString() => $"[{string.Join(", ", Enumerable.Range(0, Count).Select(i => this[i]))}]";
}
```

### 26. BinaryHeap
```csharp
public class BinaryHeap<T>
{
    private readonly List<T> heap = new();
    private readonly Comparison<T> cmp;
    public BinaryHeap(Comparison<T> cmp) { this.cmp = cmp; }
    public int Count => heap.Count;
    public T this[int i] => heap[i];

    private void SiftUp(int i)
    {
        while (i > 0)
        {
            int p = (i - 1) / 2;
            if (cmp(heap[p], heap[i]) <= 0) break;
            (heap[p], heap[i]) = (heap[i], heap[p]);
            i = p;
        }
    }

    private void SiftDown(int i)
    {
        while (true)
        {
            int l = 2 * i + 1, r = l + 1, best = i;
            if (l < Count && cmp(heap[l], heap[best]) < 0) best = l;
            if (r < Count && cmp(heap[r], heap[best]) < 0) best = r;
            if (best == i) break;
            (heap[i], heap[best]) = (heap[best], heap[i]);
            i = best;
        }
    }

    public void Insert(T v)
    {
        heap.Add(v);
        SiftUp(Count - 1);
    }

    public T Peek() => heap[0];

    public T Extract()
    {
        var res = heap[0];
        heap[0] = heap[^1];
        heap.RemoveAt(Count - 1);
        if (heap.Count > 0) SiftDown(0);
        return res;
    }

    public void ChangeKey(int i, T v)
    {
        var old = heap[i];
        heap[i] = v;
        if (cmp(v, old) < 0) SiftUp(i); else SiftDown(i);
    }

    public static BinaryHeap<T> operator +(BinaryHeap<T> a, BinaryHeap<T> b)
    {
        var res = new BinaryHeap<T>(a.cmp);
        res.heap.AddRange(a.heap);
        res.heap.AddRange(b.heap);
        for (int i = res.Count / 2; i >= 0; i--) res.SiftDown(i);
        return res;
    }

    public override bool Equals(object? obj) =>
        obj is BinaryHeap<T> other && heap.SequenceEqual(other.heap, EqualityComparer<T>.Default);

    public override int GetHashCode() => heap.Aggregate(Count, HashCode.Combine);

    public override string ToString() => $"[{string.Join(", ", heap)}]";
}
```

### 27. HashTable (цепочки)
```csharp
public class HashTable
{
    private List<(string key, int value)>[] buckets;
    private int count;

    public HashTable(int capacity = 16)
    {
        buckets = new List<(string, int)>[capacity];
    }

    private int Index(string key) => (key.GetHashCode() & int.MaxValue) % buckets.Length;

    private void Ensure()
    {
        if ((double)count / buckets.Length < 0.75) return;
        var old = buckets;
        buckets = new List<(string, int)>[old.Length * 2];
        count = 0;
        foreach (var list in old)
            if (list != null)
                foreach (var p in list)
                    AddOrUpdate(p.key, p.value);
    }

    public void AddOrUpdate(string key, int value)
    {
        Ensure();
        int idx = Index(key);
        buckets[idx] ??= new List<(string, int)>();
        for (int i = 0; i < buckets[idx]!.Count; i++)
        {
            if (buckets[idx][i].key == key)
            {
                buckets[idx][i] = (key, value);
                return;
            }
        }
        buckets[idx]!.Add((key, value));
        count++;
    }

    public bool TryGetValue(string key, out int value)
    {
        int idx = Index(key);
        if (buckets[idx] != null)
        {
            foreach (var p in buckets[idx])
                if (p.key == key) { value = p.value; return true; }
        }
        value = default;
        return false;
    }

    public bool Remove(string key)
    {
        int idx = Index(key);
        if (buckets[idx] == null) return false;
        for (int i = 0; i < buckets[idx]!.Count; i++)
        {
            if (buckets[idx]![i].key == key)
            {
                buckets[idx]!.RemoveAt(i);
                count--;
                return true;
            }
        }
        return false;
    }

    public int this[string key]
    {
        get => TryGetValue(key, out var v) ? v : throw new KeyNotFoundException();
        set => AddOrUpdate(key, value);
    }

    public IEnumerable<string> Keys =>
        buckets.Where(b => b != null).SelectMany(b => b!.Select(p => p.key));

    public override bool Equals(object? obj)
    {
        if (obj is not HashTable other) return false;
        var keys = Keys.OrderBy(k => k).ToList();
        var otherKeys = other.Keys.OrderBy(k => k).ToList();
        if (!keys.SequenceEqual(otherKeys)) return false;
        return keys.All(k => this[k] == other[k]);
    }

    public override int GetHashCode() =>
        Keys.Aggregate(count, (acc, k) => HashCode.Combine(acc, k.GetHashCode(), this[k]));

    public override string ToString() => string.Join("; ", Keys.Select(k => $"{k}={this[k]}"));
}
```

### 28. Rational
```csharp
public readonly struct Rational : IComparable<Rational>
{
    public int Num { get; }
    public int Den { get; }

    public Rational(int num, int den)
    {
        if (den == 0) throw new DivideByZeroException();
        int sign = den < 0 ? -1 : 1;
        int g = Gcd(Math.Abs(num), Math.Abs(den));
        Num = sign * num / g;
        Den = sign * den / g;
    }

    private static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);

    public int this[int index] => index switch
    {
        0 => Num,
        1 => Den,
        _ => throw new IndexOutOfRangeException()
    };

    public static Rational operator +(Rational a, Rational b) =>
        new Rational(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);

    public static Rational operator -(Rational a, Rational b) =>
        new Rational(a.Num * b.Den - b.Num * a.Den, a.Den * b.Den);

    public static Rational operator *(Rational a, Rational b) =>
        new Rational(a.Num * b.Num, a.Den * b.Den);

    public int CompareTo(Rational other) => (Num * other.Den).CompareTo(other.Num * Den);

    public override bool Equals(object? obj) =>
        obj is Rational r && Num == r.Num && Den == r.Den;

    public override int GetHashCode() => HashCode.Combine(Num, Den);

    public override string ToString() => $"{Num}/{Den}";
}
```

### 29. Matrix
```csharp
public class Matrix
{
    private readonly int[,] data;
    public int Rows { get; }
    public int Cols { get; }

    public Matrix(int rows, int cols)
    {
        Rows = rows; Cols = cols;
        data = new int[rows, cols];
    }

    public int this[int r, int c]
    {
        get => data[r, c];
        set => data[r, c] = value;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        if (a.Cols != b.Rows) throw new ArgumentException("size mismatch");
        var res = new Matrix(a.Rows, b.Cols);
        for (int i = 0; i < a.Rows; i++)
            for (int k = 0; k < a.Cols; k++)
                for (int j = 0; j < b.Cols; j++)
                    res[i, j] += a[i, k] * b[k, j];
        return res;
    }

    public Matrix Transpose()
    {
        var t = new Matrix(Cols, Rows);
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Cols; j++)
                t[j, i] = data[i, j];
        return t;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Matrix m || m.Rows != Rows || m.Cols != Cols) return false;
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Cols; j++)
                if (data[i, j] != m.data[i, j]) return false;
        return true;
    }

    public override int GetHashCode() => HashCode.Combine(Rows, Cols);

    public override string ToString()
    {
        var lines = new List<string>();
        for (int i = 0; i < Rows; i++)
        {
            var row = new int[Cols];
            for (int j = 0; j < Cols; j++) row[j] = data[i, j];
            lines.Add(string.Join(" ", row));
        }
        return string.Join(Environment.NewLine, lines);
    }
}
```

### 30. Polynomial
```csharp
public class Polynomial
{
    private readonly List<int> coeffs = new();

    private void Ensure(int deg)
    {
        while (coeffs.Count <= deg) coeffs.Add(0);
    }

    public int this[int deg]
    {
        get => deg < coeffs.Count ? coeffs[deg] : 0;
        set { Ensure(deg); coeffs[deg] = value; }
    }

    public static Polynomial operator +(Polynomial a, Polynomial b)
    {
        var res = new Polynomial();
        int n = Math.Max(a.coeffs.Count, b.coeffs.Count);
        for (int i = 0; i < n; i++) res[i] = a[i] + b[i];
        res.Trim();
        return res;
    }

    public static Polynomial operator *(Polynomial a, Polynomial b)
    {
        var res = new Polynomial();
        for (int i = 0; i < a.coeffs.Count; i++)
            for (int j = 0; j < b.coeffs.Count; j++)
                res[i + j] = res[i + j] + a[i] * b[j];
        res.Trim();
        return res;
    }

    public int Evaluate(int x)
    {
        long res = 0;
        for (int i = coeffs.Count - 1; i >= 0; i--)
            res = res * x + coeffs[i];
        return (int)res;
    }

    private void Trim()
    {
        for (int i = coeffs.Count - 1; i >= 0; i--)
        {
            if (coeffs[i] != 0) break;
            coeffs.RemoveAt(i);
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Polynomial p) return false;
        int n = Math.Max(coeffs.Count, p.coeffs.Count);
        for (int i = 0; i < n; i++) if (this[i] != p[i]) return false;
        return true;
    }

    public override int GetHashCode() => coeffs.Aggregate(coeffs.Count, HashCode.Combine);

    public override string ToString()
    {
        var parts = new List<string>();
        for (int i = coeffs.Count - 1; i >= 0; i--)
        {
            int c = coeffs[i];
            if (c == 0) continue;
            string term = i switch
            {
                0 => $"{c}",
                1 => $"{c}x",
                _ => $"{c}x^{i}"
            };
            parts.Add(term);
        }
        return parts.Count == 0 ? "0" : string.Join(" + ", parts);
    }
}
```

## Задачи на ООП
Ниже примерные реализации (нумерация продолжает список): создайте объекты и вызовите методы для демонстрации. Код развёрнут, без однострочных методов.

### 31. Animal
```csharp
public interface IMovable
{
    void Move();
}

public interface INoisy
{
    void MakeSound();
}

public abstract class Animal : IMovable, INoisy
{
    public string Name { get; }
    public int Age { get; }
    public double Weight { get; }

    protected Animal(string name, int age, double weight)
    {
        Name = name;
        Age = age;
        Weight = weight;
    }

    public abstract void Move();
    public abstract void MakeSound();
}

public class Cat : Animal
{
    public Cat(string name, int age, double weight) : base(name, age, weight) { }

    public override void Move()
    {
        Console.WriteLine("Cat prowls");
    }

    public override void MakeSound()
    {
        Console.WriteLine("Meow");
    }
}

public class Dog : Animal
{
    public Dog(string name, int age, double weight) : base(name, age, weight) { }

    public override void Move()
    {
        Console.WriteLine("Dog runs");
    }

    public override void MakeSound()
    {
        Console.WriteLine("Woof");
    }
}

public class Bird : Animal
{
    public Bird(string name, int age, double weight) : base(name, age, weight) { }

    public override void Move()
    {
        Console.WriteLine("Bird flies");
    }

    public override void MakeSound()
    {
        Console.WriteLine("Tweet");
    }
}
```

### 32. Instrument
```csharp
public interface IInstrument
{
    string Name { get; }
    string Type { get; }
    void Play();
    void Tune();
}

public class Guitar : IInstrument
{
    public string Name => "Guitar";
    public string Type => "String";

    public void Play()
    {
        Console.WriteLine("Strum");
    }

    public void Tune()
    {
        Console.WriteLine("Tune strings");
    }
}

public class Piano : IInstrument
{
    public string Name => "Piano";
    public string Type => "Keyboard";

    public void Play()
    {
        Console.WriteLine("Keys");
    }

    public void Tune()
    {
        Console.WriteLine("Tune hammers");
    }
}

public class Violin : IInstrument
{
    public string Name => "Violin";
    public string Type => "String";

    public void Play()
    {
        Console.WriteLine("Bow");
    }

    public void Tune()
    {
        Console.WriteLine("Peg tune");
    }
}
```

### 33. Plant
```csharp
public interface IPlant
{
    string Title { get; }
    double Height { get; }
    string Kind { get; }
    void Water();
    void Fertilize();
}

public class Tree : IPlant
{
    public string Title { get; }
    public double Height { get; }
    public string Kind => "Tree";

    public Tree(string title, double height)
    {
        Title = title;
        Height = height;
    }

    public void Water()
    {
        Console.WriteLine("Tree watered");
    }

    public void Fertilize()
    {
        Console.WriteLine("Tree fertilized");
    }
}

public class Flower : IPlant
{
    public string Title { get; }
    public double Height { get; }
    public string Kind => "Flower";

    public Flower(string title, double height)
    {
        Title = title;
        Height = height;
    }

    public void Water()
    {
        Console.WriteLine("Flower watered");
    }

    public void Fertilize()
    {
        Console.WriteLine("Flower fertilized");
    }
}

public class Shrub : IPlant
{
    public string Title { get; }
    public double Height { get; }
    public string Kind => "Shrub";

    public Shrub(string title, double height)
    {
        Title = title;
        Height = height;
    }

    public void Water()
    {
        Console.WriteLine("Shrub watered");
    }

    public void Fertilize()
    {
        Console.WriteLine("Shrub fertilized");
    }
}
```

### 34. Clothing
```csharp
public abstract class Clothing
{
    public string Size { get; }
    public string Color { get; }
    public string Material { get; }

    protected Clothing(string size, string color, string material)
    {
        Size = size;
        Color = color;
        Material = material;
    }

    public abstract void Wash();
    public abstract void Iron();
}

public class Shirt : Clothing
{
    public Shirt(string size, string color, string material) : base(size, color, material) { }

    public override void Wash()
    {
        Console.WriteLine("Wash shirt");
    }

    public override void Iron()
    {
        Console.WriteLine("Iron shirt");
    }
}

public class Pants : Clothing
{
    public Pants(string size, string color, string material) : base(size, color, material) { }

    public override void Wash()
    {
        Console.WriteLine("Wash pants");
    }

    public override void Iron()
    {
        Console.WriteLine("Iron pants");
    }
}

public class Dress : Clothing
{
    public Dress(string size, string color, string material) : base(size, color, material) { }

    public override void Wash()
    {
        Console.WriteLine("Delicate wash dress");
    }

    public override void Iron()
    {
        Console.WriteLine("Steam dress");
    }
}
```

### 35. Device
```csharp
public interface IDevice
{
    string Brand { get; }
    string Model { get; }
    int Power { get; }
    void TurnOn();
    void TurnOff();
}

public class Smartphone : IDevice
{
    public string Brand { get; }
    public string Model { get; }
    public int Power { get; }

    public Smartphone(string brand, string model, int power)
    {
        Brand = brand;
        Model = model;
        Power = power;
    }

    public void TurnOn()
    {
        Console.WriteLine("Phone on");
    }

    public void TurnOff()
    {
        Console.WriteLine("Phone off");
    }
}

public class Laptop : IDevice
{
    public string Brand { get; }
    public string Model { get; }
    public int Power { get; }

    public Laptop(string brand, string model, int power)
    {
        Brand = brand;
        Model = model;
        Power = power;
    }

    public void TurnOn()
    {
        Console.WriteLine("Laptop on");
    }

    public void TurnOff()
    {
        Console.WriteLine("Laptop off");
    }
}

public class TV : IDevice
{
    public string Brand { get; }
    public string Model { get; }
    public int Power { get; }

    public TV(string brand, string model, int power)
    {
        Brand = brand;
        Model = model;
        Power = power;
    }

    public void TurnOn()
    {
        Console.WriteLine("TV on");
    }

    public void TurnOff()
    {
        Console.WriteLine("TV off");
    }
}
```

### 36. Solid
```csharp
public interface ISolid
{
    double Volume();
    double Area();
}

public class Cube : ISolid
{
    public double A { get; }

    public Cube(double a)
    {
        A = a;
    }

    public double Volume()
    {
        return A * A * A;
    }

    public double Area()
    {
        return 6 * A * A;
    }
}

public class Sphere : ISolid
{
    public double R { get; }

    public Sphere(double r)
    {
        R = r;
    }

    public double Volume()
    {
        return 4.0 / 3 * Math.PI * Math.Pow(R, 3);
    }

    public double Area()
    {
        return 4 * Math.PI * R * R;
    }
}

public class Cylinder : ISolid
{
    public double R { get; }
    public double H { get; }

    public Cylinder(double r, double h)
    {
        R = r;
        H = h;
    }

    public double Volume()
    {
        return Math.PI * R * R * H;
    }

    public double Area()
    {
        return 2 * Math.PI * R * (R + H);
    }
}
```

### 37. Account
```csharp
public interface IAccount
{
    string Number { get; }
    decimal Balance { get; }
    void Deposit(decimal sum);
    void Withdraw(decimal sum);
}

public abstract class Account : IAccount
{
    public string Number { get; }
    public decimal Balance { get; protected set; }

    protected Account(string number, decimal balance = 0)
    {
        Number = number;
        Balance = balance;
    }

    public virtual void Deposit(decimal sum)
    {
        Balance += sum;
    }

    public virtual void Withdraw(decimal sum)
    {
        Balance -= sum;
    }
}

public class SavingAccount : Account
{
    public SavingAccount(string number, decimal balance = 0) : base(number, balance) { }
}

public class CheckingAccount : Account
{
    public CheckingAccount(string number, decimal balance = 0) : base(number, balance) { }
}

public class CreditAccount : Account
{
    public decimal Limit { get; }

    public CreditAccount(string number, decimal limit) : base(number)
    {
        Limit = limit;
    }

    public override void Withdraw(decimal sum)
    {
        if (Balance - sum < -Limit)
            throw new InvalidOperationException("limit exceeded");
        base.Withdraw(sum);
    }
}
```

### 38. Book
```csharp
public interface IBook
{
    string Title { get; }
    string Author { get; }
    int Pages { get; }
    void Read();
    void Search(string query);
}

public class Novel : IBook
{
    public string Title { get; }
    public string Author { get; }
    public int Pages { get; }

    public Novel(string title, string author, int pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
    }

    public void Read()
    {
        Console.WriteLine("Reading novel");
    }

    public void Search(string query)
    {
        Console.WriteLine($"Find '{query}' in novel");
    }
}

public class Textbook : IBook
{
    public string Title { get; }
    public string Author { get; }
    public int Pages { get; }

    public Textbook(string title, string author, int pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
    }

    public void Read()
    {
        Console.WriteLine("Study chapter");
    }

    public void Search(string query)
    {
        Console.WriteLine($"Index search '{query}'");
    }
}

public class Encyclopedia : IBook
{
    public string Title { get; }
    public string Author { get; }
    public int Pages { get; }

    public Encyclopedia(string title, string author, int pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
    }

    public void Read()
    {
        Console.WriteLine("Browse entry");
    }

    public void Search(string query)
    {
        Console.WriteLine($"Lookup '{query}'");
    }
}
```

### 39. Game
```csharp
public interface IGame
{
    string Name { get; }
    int Players { get; }
    void Start();
    void Finish();
}

public class Football : IGame
{
    public string Name => "Football";
    public int Players => 22;

    public void Start()
    {
        Console.WriteLine("Kick off");
    }

    public void Finish()
    {
        Console.WriteLine("Final whistle");
    }
}

public class Basketball : IGame
{
    public string Name => "Basketball";
    public int Players => 10;

    public void Start()
    {
        Console.WriteLine("Tip off");
    }

    public void Finish()
    {
        Console.WriteLine("Buzzer");
    }
}

public class Tennis : IGame
{
    public string Name => "Tennis";
    public int Players => 2;

    public void Start()
    {
        Console.WriteLine("Serve");
    }

    public void Finish()
    {
        Console.WriteLine("Game set match");
    }
}
```

### 40. MusicGenre
```csharp
public interface IMusicGenre
{
    string Title { get; }
    string Instruments { get; }
    void Play();
    void Describe();
}

public class Rock : IMusicGenre
{
    public string Title => "Rock";
    public string Instruments => "Guitar, drums";

    public void Play()
    {
        Console.WriteLine("Riff");
    }

    public void Describe()
    {
        Console.WriteLine("Energy and drive");
    }
}

public class Jazz : IMusicGenre
{
    public string Title => "Jazz";
    public string Instruments => "Sax, piano";

    public void Play()
    {
        Console.WriteLine("Improvisation");
    }

    public void Describe()
    {
        Console.WriteLine("Swing and blue notes");
    }
}

public class Classical : IMusicGenre
{
    public string Title => "Classical";
    public string Instruments => "Strings, winds";

    public void Play()
    {
        Console.WriteLine("Orchestra");
    }

    public void Describe()
    {
        Console.WriteLine("Structured forms");
    }
}
```
