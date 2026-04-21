namespace CT12.Tasks;
using System;
using System.Collections.Generic;

internal static class TravelingSalesmen
{
    public static void Solve()
    {
        var line1 = Console.ReadLine()!.Trim().Split();
        int n = int.Parse(line1[0]);
        int m = int.Parse(line1[1]);

        long[] cityCost = new long[n];
        var line2 = Console.ReadLine()!.Trim().Split();
        for (int i = 0; i < n; i++)
            cityCost[i] = long.Parse(line2[i]);

        const long INF = long.MaxValue / 2;
        long[,] dist = new long[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                dist[i, j] = (i == j) ? 0 : INF;

        var edges = new List<(int u, int v, long cost)>();
        for (int i = 0; i < m; i++)
        {
            var parts = Console.ReadLine()!.Trim().Split();
            int u = int.Parse(parts[0]) - 1;
            int v = int.Parse(parts[1]) - 1;
            long cost = long.Parse(parts[2]);
            edges.Add((u, v, cost));
            dist[u, v] = cost;
        }

        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                if (dist[i, k] < INF)
                    for (int j = 0; j < n; j++)
                        if (dist[k, j] < INF)
                            dist[i, j] = Math.Min(dist[i, j], dist[i, k] + dist[k, j]);

        int source = 0;
        int sink = 2 * n + 1;
        int numNodes = 2 * n + 2;

        var graph = new List<(int to, long cap, long cost, int rev)>[numNodes];
        for (int i = 0; i < numNodes; i++)
            graph[i] = new List<(int, long, long, int)>();

        void AddEdge(int u, int v, long cap, long cost)
        {
            graph[u].Add((v, cap, cost, graph[v].Count));
            graph[v].Add((u, 0, -cost, graph[u].Count - 1));
        }

        for (int i = 0; i < n; i++)
            AddEdge(source, i + 1, 1, 0);

        for (int i = 0; i < n; i++)
            AddEdge(n + 1 + i, sink, 1, 0);

        for (int u = 0; u < n; u++)
            for (int v = 0; v < n; v++)
                if (u != v && dist[u, v] < INF)
                    AddEdge(u + 1, n + 1 + v, 1, dist[u, v]);

        for (int i = 0; i < n; i++)
            AddEdge(i + 1, n + 1 + i, 1, cityCost[i]);

        long totalCost = 0;

        while (true)
        {
            long[] spfaDist = new long[numNodes];
            Array.Fill(spfaDist, INF);
            spfaDist[source] = 0;

            int[] parentV = new int[numNodes];
            int[] parentE = new int[numNodes];
            Array.Fill(parentV, -1);

            bool[] inQueue = new bool[numNodes];
            var queue = new Queue<int>();
            queue.Enqueue(source);
            inQueue[source] = true;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                inQueue[u] = false;

                for (int i = 0; i < graph[u].Count; i++)
                {
                    var (vv, capv, costv, _) = graph[u][i];
                    if (capv > 0 && spfaDist[u] + costv < spfaDist[vv])
                    {
                        spfaDist[vv] = spfaDist[u] + costv;
                        parentV[vv] = u;
                        parentE[vv] = i;
                        if (!inQueue[vv])
                        {
                            queue.Enqueue(vv);
                            inQueue[vv] = true;
                        }
                    }
                }
            }

            if (spfaDist[sink] >= INF)
                break;

            int cur = sink;
            while (cur != source)
            {
                int u = parentV[cur];
                int idx = parentE[cur];

                var (fto, fcap, fcost, frev) = graph[u][idx];
                graph[u][idx] = (fto, fcap - 1, fcost, frev);

                var (rto, rcap, rcost, rrev) = graph[cur][frev];
                graph[cur][frev] = (rto, rcap + 1, rcost, rrev);

                cur = u;
            }

            totalCost += spfaDist[sink];
        }

        Console.WriteLine(totalCost);
    }
}
