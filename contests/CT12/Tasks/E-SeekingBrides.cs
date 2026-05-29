using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class Solution
{
    const long INF = long.MaxValue / 2;

    struct Edge
    {
        public int to, cap;
        public long cost;
        public int orig;
    }

    static List<Edge> edges;
    static List<int>[] g;
    static int n_nodes;

    static void AddArc(int from, int to, int cap, long cost, int origIdx)
    {
        g[from].Add(edges.Count);
        edges.Add(new Edge { to = to, cap = cap, cost = cost, orig = origIdx });
        g[to].Add(edges.Count);
        edges.Add(new Edge { to = from, cap = 0, cost = -cost, orig = 0 });
    }

    
    static (long flow, long cost) MinCostFlow(int s, int t, int need)
    {
        long totalFlow = 0, totalCost = 0;
        long[] dist = new long[n_nodes];
        bool[] inq = new bool[n_nodes];
        int[] prev = new int[n_nodes];

        while (totalFlow < need)
        {
            for (int i = 0; i < n_nodes; i++) { dist[i] = INF; inq[i] = false; prev[i] = -1; }
            dist[s] = 0;

            
            var dq = new LinkedList<int>();
            dq.AddFirst(s);
            inq[s] = true;

            while (dq.Count > 0)
            {
                int v = dq.First.Value; dq.RemoveFirst(); inq[v] = false;
                foreach (int id in g[v])
                {
                    var e = edges[id];
                    if (e.cap > 0 && dist[v] < INF && dist[v] + e.cost < dist[e.to])
                    {
                        dist[e.to] = dist[v] + e.cost;
                        prev[e.to] = id;
                        if (!inq[e.to])
                        {
                            inq[e.to] = true;
                            
                            if (dq.Count > 0 && dist[e.to] < dist[dq.First.Value])
                                dq.AddFirst(e.to);
                            else
                                dq.AddLast(e.to);
                        }
                    }
                }
            }

            if (dist[t] == INF) break;

            int pushed = need - (int)totalFlow;
            for (int cur = t; cur != s;) { int id = prev[cur]; pushed = Math.Min(pushed, edges[id].cap); cur = edges[id ^ 1].to; }
            for (int cur = t; cur != s;)
            {
                int id = prev[cur];
                var e = edges[id]; e.cap -= pushed; edges[id] = e;
                var er = edges[id ^ 1]; er.cap += pushed; edges[id ^ 1] = er;
                cur = edges[id ^ 1].to;
            }
            totalFlow += pushed;
            totalCost += (long)pushed * dist[t];
        }
        return (totalFlow, totalCost);
    }

    static int[] remFlow;

    static bool DFSPath(int v, int sink, List<int> path, bool[] visited)
    {
        if (v == sink) return true;
        visited[v] = true;
        foreach (int id in g[v])
        {
            var e = edges[id];
            if (e.orig != 0 && remFlow[id] > 0 && !visited[e.to])
            {
                remFlow[id]--;
                path.Add(Math.Abs(e.orig));
                if (DFSPath(e.to, sink, path, visited))
                    return true;
                path.RemoveAt(path.Count - 1);
                remFlow[id]++;
            }
        }
        visited[v] = false;
        return false;
    }

    static void Run()
    {
        var line = Console.ReadLine().Trim().Split();
        int N = int.Parse(line[0]);
        int M = int.Parse(line[1]);
        int K = int.Parse(line[2]);

        n_nodes = N;
        edges = new List<Edge>(M * 4 + 4);
        g = new List<int>[N];
        for (int i = 0; i < N; i++) g[i] = new List<int>();

        int[] eu = new int[M]; int[] ev = new int[M]; long[] ew = new long[M];
        for (int i = 0; i < M; i++)
        {
            var p = Console.ReadLine().Trim().Split();
            eu[i] = int.Parse(p[0]) - 1;
            ev[i] = int.Parse(p[1]) - 1;
            ew[i] = long.Parse(p[2]);
        }

        
        
        
        
        
        for (int i = 0; i < M; i++)
        {
            AddArc(eu[i], ev[i], 1, ew[i],  (i + 1));
            AddArc(ev[i], eu[i], 1, ew[i], -(i + 1));
        }

        int source = 0, sink = N - 1;
        var (flow, cost) = MinCostFlow(source, sink, K);

        if (flow < K) { Console.WriteLine(-1); return; }

        double avg = (double)cost / K;
        var sb = new StringBuilder();
        sb.AppendLine(avg.ToString("F5"));

        remFlow = new int[edges.Count];
        for (int i = 0; i < edges.Count; i++)
            if (edges[i].orig != 0)
                remFlow[i] = 1 - edges[i].cap;

        for (int p = 0; p < K; p++)
        {
            var path = new List<int>();
            DFSPath(source, sink, path, new bool[N]);
            sb.Append(path.Count);
            foreach (int eo in path) sb.Append(' ').Append(eo);
            sb.AppendLine();
        }

        Console.Write(sb.ToString().TrimEnd());
        Console.WriteLine();
    }

    static void Main()
    {
        new Thread(Run, 64 * 1024 * 1024).Start();
    }
}