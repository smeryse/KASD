using System;
using System.Collections.Generic;
using System.IO;

namespace CT8.Tasks;

internal class LostKefir
{
    // Быстрый ввод для обработки больших тестов
    private class FastReader
    {
        private TextReader reader;
        private Queue<string> tokens = new Queue<string>();

        public FastReader(TextReader reader)
        {
            this.reader = reader;
        }

        public string Next()
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

        public int NextInt() => int.Parse(Next());
        public long NextLong() => long.Parse(Next());
    }

    public static void Solve()
    {
        var fastReader = new FastReader(Console.In);
        
        string token = fastReader.Next();
        if (token == null) return;

        int n = int.Parse(token);
        int m = fastReader.NextInt();

        var adj = new List<(int to, int weight)>[n + 1];
        for (int i = 1; i <= n; i++)
            adj[i] = new List<(int, int)>();

        for (int i = 0; i < m; i++)
        {
            int u = fastReader.NextInt();
            int v = fastReader.NextInt();
            int w = fastReader.NextInt();
            adj[u].Add((v, w));
            adj[v].Add((u, w));
        }

        int a = fastReader.NextInt();
        int b = fastReader.NextInt();
        int c = fastReader.NextInt();

        const long INF = long.MaxValue / 4;

        // Запускаем Дейкстру из a, b, c
        long[] distA = Dijkstra(n, adj, a);
        long[] distB = Dijkstra(n, adj, b);
        long[] distC = Dijkstra(n, adj, c);

        // Проверяем все 6 перестановок посещения a, b, c
        long ans = INF;

        // a -> b -> c
        if (distA[b] != INF && distB[c] != INF)
            ans = Math.Min(ans, distA[b] + distB[c]);

        // a -> c -> b
        if (distA[c] != INF && distC[b] != INF)
            ans = Math.Min(ans, distA[c] + distC[b]);

        // b -> a -> c
        if (distB[a] != INF && distA[c] != INF)
            ans = Math.Min(ans, distB[a] + distA[c]);

        // b -> c -> a
        if (distB[c] != INF && distC[a] != INF)
            ans = Math.Min(ans, distB[c] + distC[a]);

        // c -> a -> b
        if (distC[a] != INF && distA[b] != INF)
            ans = Math.Min(ans, distC[a] + distA[b]);

        // c -> b -> a
        if (distC[b] != INF && distB[a] != INF)
            ans = Math.Min(ans, distC[b] + distB[a]);

        if (ans == INF)
            Console.WriteLine(-1);
        else
            Console.WriteLine(ans);
    }

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

        while (pq.Count > 0)
        {
            int u = pq.Dequeue();

            // Пропускаем уже обработанные вершины
            if (visited[u])
                continue;
            
            visited[u] = true;

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
}
