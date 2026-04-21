using System;
using System.Collections.Generic;
using System.Text;

internal static class SeekingBrides
{
    static readonly long INF = long.MaxValue / 2;

    struct Edge
    {
        public int To, Rev, EdgeId;
        public long Cap, Cost;
    }

    static List<Edge>[] graph;

    static void AddEdge(int u, int v, long cap, long cost, int eid)
    {
        graph[u].Add(new Edge { To = v, Cap = cap,  Cost = cost,  EdgeId = eid, Rev = graph[v].Count });
        graph[v].Add(new Edge { To = u, Cap = 0,    Cost = -cost, EdgeId = eid, Rev = graph[u].Count - 1 });
    }

    public static void Solve()
    {
        var line1 = Console.ReadLine()!.Trim().Split();
        int n = int.Parse(line1[0]);
        int m = int.Parse(line1[1]);
        int k = int.Parse(line1[2]);

        graph = new List<Edge>[n + 1];
        for (int i = 0; i <= n; i++) graph[i] = new List<Edge>();

        for (int i = 0; i < m; i++)
        {
            var p = Console.ReadLine()!.Trim().Split();
            int u = int.Parse(p[0]);
            int v = int.Parse(p[1]);
            long w = long.Parse(p[2]);

            AddEdge(u, v, 1, w, i + 1);
            AddEdge(v, u, 1, w, i + 1);
        }

        int source = 1, sink = n;

        long[] pot = new long[n + 1];
        Array.Fill(pot, INF);
        pot[source] = 0;
        for (int iter = 0; iter < n - 1; iter++)
        {
            bool upd = false;
            for (int u = 1; u <= n; u++)
            {
                if (pot[u] == INF) continue;
                foreach (var e in graph[u])
                {
                    if (e.Cap > 0 && pot[u] != INF && pot[u] + e.Cost < pot[e.To])
                    {
                        pot[e.To] = pot[u] + e.Cost;
                        upd = true;
                    }
                }
            }
            if (!upd) break;
        }

        long totalCost = 0;
        int totalFlow = 0;
        var paths = new List<List<int>>();

        for (int iter = 0; iter < k; iter++)
        {
            // Dijkstra with Johnson potentials (reduced costs >= 0)
            long[] dist = new long[n + 1];
            Array.Fill(dist, INF);
            dist[source] = 0;
            int[] parentV = new int[n + 1];
            int[] parentE = new int[n + 1];
            Array.Fill(parentV, -1);

            var pq = new PriorityQueue<int, long>();
            pq.Enqueue(source, 0);

            while (pq.Count > 0)
            {
                pq.TryDequeue(out int u, out long d);
                if (d > dist[u]) continue;
                for (int i = 0; i < graph[u].Count; i++)
                {
                    var e = graph[u][i];
                    if (e.Cap <= 0) continue;
                    if (pot[u] == INF || pot[e.To] == INF) continue;
                    long rc = e.Cost + pot[u] - pot[e.To];
                    if (rc < 0) rc = 0;
                    long nd = dist[u] + rc;
                    if (nd < dist[e.To])
                    {
                        dist[e.To] = nd;
                        parentV[e.To] = u;
                        parentE[e.To] = i;
                        pq.Enqueue(e.To, nd);
                    }
                }
            }

            if (dist[sink] >= INF) break;

            // Real cost of this augmenting path = pot[sink] + dist[sink]
            // (because pot[v] = shortest real dist to v before this iteration's adjustments)
            // Actually: real_dist[sink] = dist[sink] + pot[sink] - pot[source]
            // pot[source] = 0 always (source dist=0, never updated away from 0)
            long realCost = dist[sink] + pot[sink]; // pot[source]=0

            // Update potentials
            for (int i = 1; i <= n; i++)
                if (dist[i] < INF) pot[i] += dist[i];

            // Trace and augment
            var path = new List<int>();
            int v = sink;
            while (v != source)
            {
                int u = parentV[v];
                int idx = parentE[v];
                var fe = graph[u][idx];
                path.Add(fe.EdgeId);

                // Update forward edge
                graph[u][idx] = new Edge {
                    To = fe.To, Cap = fe.Cap - 1,
                    Cost = fe.Cost, EdgeId = fe.EdgeId, Rev = fe.Rev
                };
                // Update reverse edge
                var re = graph[v][fe.Rev];
                graph[v][fe.Rev] = new Edge {
                    To = re.To, Cap = re.Cap + 1,
                    Cost = re.Cost, EdgeId = re.EdgeId, Rev = re.Rev
                };

                v = u;
            }
            path.Reverse();
            paths.Add(path);
            totalFlow++;
            totalCost += realCost;
        }

        if (totalFlow < k)
        {
            Console.WriteLine("-1");
            return;
        }

        var sb = new StringBuilder();
        double avgTime = (double)totalCost / k;
        sb.AppendLine(avgTime.ToString("F5", System.Globalization.CultureInfo.InvariantCulture));
        foreach (var path in paths)
            sb.AppendLine(path.Count + " " + string.Join(" ", path));
        Console.Write(sb);
    }

    static void Main() => Solve();
}

class Program
{
    static void Main() => SeekingBrides.Main();
}