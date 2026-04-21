namespace CT12.Tasks;

using System;
using System.Collections.Generic;

internal static class A_MCMF
{
    const long INF = long.MaxValue / 2;
    
    static int n, m;
    static int[] to, next_edge, head;
    static long[] cap, cost;
    static int edge_cnt;
    
    static void Init(int nodes, int maxEdges)
    {
        head = new int[nodes + 1];
        Array.Fill(head, -1);
        to = new int[maxEdges * 2];
        next_edge = new int[maxEdges * 2];
        cap = new long[maxEdges * 2];
        cost = new long[maxEdges * 2];
        edge_cnt = 0;
    }
    
    static void AddEdge(int u, int v, long c, long w)
    {
        to[edge_cnt] = v; cap[edge_cnt] = c; cost[edge_cnt] = w;
        next_edge[edge_cnt] = head[u]; head[u] = edge_cnt++;
        
        to[edge_cnt] = u; cap[edge_cnt] = 0; cost[edge_cnt] = -w;
        next_edge[edge_cnt] = head[v]; head[v] = edge_cnt++;
    }
    
    static long totalFlow = 0, totalCost = 0;
    
    static void MCMFAlgorithm(int s, int t, int nodes)
    {
        while (true)
        {
            long[] dist = new long[nodes + 1];
            Array.Fill(dist, INF);
            dist[s] = 0;
            bool[] inQueue = new bool[nodes + 1];
            int[] prevv = new int[nodes + 1];
            int[] preve = new int[nodes + 1];
            Array.Fill(prevv, -1);
            
            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            inQueue[s] = true;
            
            while (q.Count > 0)
            {
                int u = q.Dequeue();
                inQueue[u] = false;
                
                for (int e = head[u]; e != -1; e = next_edge[e])
                {
                    int v = to[e];
                    if (cap[e] > 0 && dist[u] + cost[e] < dist[v])
                    {
                        dist[v] = dist[u] + cost[e];
                        prevv[v] = u;
                        preve[v] = e;
                        if (!inQueue[v])
                        {
                            inQueue[v] = true;
                            q.Enqueue(v);
                        }
                    }
                }
            }
            
            if (dist[t] == INF) break;
            
            long f = INF;
            for (int v = t; v != s; v = prevv[v])
                f = Math.Min(f, cap[preve[v]]);
            
            for (int v = t; v != s; v = prevv[v])
            {
                cap[preve[v]] -= f;
                cap[preve[v] ^ 1] += f;
            }
            
            totalFlow += f;
            totalCost += f * dist[t];
        }
    }
    
    public static void Solve()
    {
        string[] line = Console.ReadLine().Trim().Split();
        n = int.Parse(line[0]);
        m = int.Parse(line[1]);
        
        Init(n, m + 10);
        
        for (int i = 0; i < m; i++)
        {
            string[] parts = Console.ReadLine().Trim().Split();
            int u = int.Parse(parts[0]);
            int v = int.Parse(parts[1]);
            long c = long.Parse(parts[2]);
            long w = long.Parse(parts[3]);
            AddEdge(u, v, c, w);
        }
        
        MCMFAlgorithm(1, n, n);
        
        Console.WriteLine(totalCost);
    }
}