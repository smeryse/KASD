// Номер в списке - 12
// Решены задачи 3, 12, 15
namespace Task20.Graphs
{
    public class Graph
    {
        public int VerticesCount { get; private set; }
        private List<List<int>> adjList;
        private List<List<int>> reverseAdjList;

        public Graph(int n)
        {
            VerticesCount = n;
            adjList = new List<List<int>>(n);
            reverseAdjList = new List<List<int>>(n);
            for (int i = 0; i < n; i++)
            {
                adjList.Add(new List<int>());
                reverseAdjList.Add(new List<int>());
            }
        }

        public void AddEdge(int from, int to)
        {
            adjList[from].Add(to);
            reverseAdjList[to].Add(from);
        }

        public List<int> GetNeighbors(int v) => adjList[v];
        public List<int> GetReverseNeighbors(int v) => reverseAdjList[v];

        // Транзитивное замыкание через DFS
        public bool[,] TransitiveClosureDFS()
        {
            int n = VerticesCount;
            bool[,] reach = new bool[n, n];
            for (int i = 0; i < n; i++)
            {
                bool[] visited = new bool[n];
                DFSReach(i, i, visited, reach);
            }
            return reach;
        }

        private void DFSReach(int start, int v, bool[] visited, bool[,] reach)
        {
            visited[v] = true;
            reach[start, v] = true;
            foreach (int w in adjList[v])
            {
                if (!visited[w])
                    DFSReach(start, w, visited, reach);
            }
        }

        // Алгоритм Мальгранжа
        public List<List<int>> MalgrangeSCC()
        {
            int n = VerticesCount;
            bool[] visited = new bool[n];
            Stack<int> finishStack = new Stack<int>();

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                    MalgrangeDFS(i, visited, finishStack);
            }

            bool[] visited2 = new bool[n];
            List<List<int>> sccs = new List<List<int>>();

            while (finishStack.Count > 0)
            {
                int v = finishStack.Pop();
                if (!visited2[v])
                {
                    List<int> component = new List<int>();
                    MalgrangeReverseDFS(v, visited2, component);
                    sccs.Add(component);
                }
            }

            return sccs;
        }

        private void MalgrangeDFS(int v, bool[] visited, Stack<int> stack)
        {
            visited[v] = true;
            foreach (int w in adjList[v])
            {
                if (!visited[w])
                    MalgrangeDFS(w, visited, stack);
            }
            stack.Push(v);
        }

        private void MalgrangeReverseDFS(int v, bool[] visited, List<int> component)
        {
            visited[v] = true;
            component.Add(v);
            foreach (int w in reverseAdjList[v])
            {
                if (!visited[w])
                    MalgrangeReverseDFS(w, visited, component);
            }
        }
    }

    // Максимальный поток — алгоритм проталкивания предпотока
    public class PushRelabelMaxFlow
    {
        private int n;
        private int[,] capacity; // пропускная способность
        private int[,] flow; // поток
        private int[] height; // высота
        private int[] excess; // избыток
        private bool[] inQueue;

        public PushRelabelMaxFlow(int n)
        {
            this.n = n;
            capacity = new int[n, n];
            flow = new int[n, n];
            height = new int[n];
            excess = new int[n];
            inQueue = new bool[n];
        }

        public void AddEdge(int u, int v, int cap)
        {
            capacity[u, v] = cap;
        }

        public int ComputeMaxFlow(int source, int sink)
        {
            height[source] = n;
            excess[source] = int.MaxValue;

            // Инициализация — проталкиваем из источника
            for (int v = 0; v < n; v++)
            {
                if (capacity[source, v] > 0)
                {
                    flow[source, v] = capacity[source, v];
                    flow[v, source] = -flow[source, v];
                    excess[v] = capacity[source, v];
                    excess[source] -= capacity[source, v];
                    if (v != source && v != sink)
                        Enqueue(v);
                }
            }

            // Обработка вершин с избытком
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < n; i++)
                if (inQueue[i]) queue.Enqueue(i);

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                inQueue[u] = false;

                if (u == source || u == sink) continue;
                if (excess[u] <= 0) continue;

                Discharge(u, source, sink, queue);
            }

            return excess[sink];
        }

        private void Discharge(int u, int source, int sink, Queue<int> queue)
        {
            while (excess[u] > 0)
            {
                bool pushed = false;
                for (int v = 0; v < n; v++)
                {
                    int residual = capacity[u, v] - flow[u, v];
                    if (residual > 0 && height[u] == height[v] + 1)
                    {
                        Push(u, v);
                        pushed = true;
                        if (v != source && v != sink && !inQueue[v])
                        {
                            queue.Enqueue(v);
                            inQueue[v] = true;
                        }
                        if (excess[u] == 0) break;
                    }
                }
                if (!pushed)
                {
                    Relabel(u);
                }
            }
        }

        private void Push(int u, int v)
        {
            int residual = capacity[u, v] - flow[u, v];
            int pushAmount = Math.Min(excess[u], residual);
            flow[u, v] += pushAmount;
            flow[v, u] -= pushAmount;
            excess[u] -= pushAmount;
            excess[v] += pushAmount;
        }

        private void Relabel(int u)
        {
            int minHeight = int.MaxValue;
            for (int v = 0; v < n; v++)
            {
                int residual = capacity[u, v] - flow[u, v];
                if (residual > 0)
                    minHeight = Math.Min(minHeight, height[v]);
            }
            if (minHeight != int.MaxValue)
                height[u] = minHeight + 1;
        }

        private void Enqueue(int v)
        {
            inQueue[v] = true;
        }
    }

    // Алгоритм Брона-Кербоша — максимальная клика
    public class BronKerbosch
    {
        private int n;
        private bool[,] adjMatrix;
        public List<List<int>> MaximalCliques { get; private set; } = new List<List<int>>();
        public List<int> MaximumClique { get; private set; } = new List<int>();

        public BronKerbosch(int n)
        {
            this.n = n;
            adjMatrix = new bool[n, n];
        }

        public void AddEdge(int u, int v)
        {
            adjMatrix[u, v] = true;
            adjMatrix[v, u] = true;
        }

        public void FindAllMaximalCliques()
        {
            MaximalCliques.Clear();
            MaximumClique.Clear();
            var R = new HashSet<int>();
            var P = new HashSet<int>();
            var X = new HashSet<int>();
            for (int i = 0; i < n; i++) P.Add(i);
            BronKerboschRecursive(R, P, X);
        }

        private void BronKerboschRecursive(HashSet<int> R, HashSet<int> P, HashSet<int> X)
        {
            if (P.Count == 0 && X.Count == 0)
            {
                var clique = R.ToList();
                MaximalCliques.Add(clique);
                if (clique.Count > MaximumClique.Count)
                    MaximumClique = clique;
                return;
            }

            int pivot = -1;
            int maxNeighbors = -1;
            var unionPX = new HashSet<int>(P);
            unionPX.UnionWith(X);
            foreach (int v in unionPX)
            {
                int neighbors = NeighborsInSet(v, P).Count;
                if (neighbors > maxNeighbors)
                {
                    maxNeighbors = neighbors;
                    pivot = v;
                }
            }

            var candidates = new HashSet<int>(P);
            if (pivot >= 0)
            {
                var pivotNeighbors = NeighborsInSet(pivot, P);
                candidates.ExceptWith(pivotNeighbors);
            }

            foreach (int v in candidates.ToList())
            {
                var newR = new HashSet<int>(R) { v };
                var neighbors = GetNeighborsSet(v);
                var newP = new HashSet<int>(P);
                newP.IntersectWith(neighbors);
                var newX = new HashSet<int>(X);
                newX.IntersectWith(neighbors);
                BronKerboschRecursive(newR, newP, newX);
                P.Remove(v);
                X.Add(v);
            }
        }

        private HashSet<int> GetNeighborsSet(int v)
        {
            var neighbors = new HashSet<int>();
            for (int i = 0; i < n; i++)
                if (adjMatrix[v, i]) neighbors.Add(i);
            return neighbors;
        }

        private HashSet<int> NeighborsInSet(int v, HashSet<int> set)
        {
            var neighbors = new HashSet<int>();
            for (int i = 0; i < n; i++)
                if (adjMatrix[v, i] && set.Contains(i)) neighbors.Add(i);
            return neighbors;
        }

        public List<int> FindMaximumClique()
        {
            FindAllMaximalCliques();
            return MaximumClique;
        }
    }
}
