namespace CT12.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;

internal static class AssignmentProblem
{
    
    public static void Solve()
    {
        int n = int.Parse(Console.ReadLine()!.Trim());
        long[,] cost = new long[n, n];
        
        for (int i = 0; i < n; i++)
        {
            var parts = Console.ReadLine()!.Trim().Split();
            for (int j = 0; j < n; j++)
            {
                cost[i, j] = long.Parse(parts[j]);
            }
        }
        
        
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
        
        
        for (int j = 0; j < n; j++)
            AddEdge(n + 1 + j, sink, 1, 0);
        
        
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                AddEdge(i + 1, n + 1 + j, 1, cost[i, j]);
            }
        }
        
        
        long totalCost = 0;
        int flow = 0;
        
        while (true)
        {
            long[] dist = new long[numNodes];
            Array.Fill(dist, long.MaxValue / 2);
            dist[source] = 0;
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
                    var (vv, capv, costv, revv) = graph[u][i];
                    if (capv > 0 && dist[u] + costv < dist[vv])
                    {
                        dist[vv] = dist[u] + costv;
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
            
            if (dist[sink] >= long.MaxValue / 2)
                break;
            
            int push = 1;
            int v = sink;
            while (v != source)
            {
                int u = parentV[v];
                int idx = parentE[v];
                graph[u][idx] = (graph[u][idx].to, graph[u][idx].cap - push, graph[u][idx].cost, graph[u][idx].rev);
                int revIdx = graph[v][graph[u][idx].rev].rev;
                graph[v][revIdx] = (graph[v][revIdx].to, graph[v][revIdx].cap + push, graph[v][revIdx].cost, graph[v][revIdx].rev);
                v = u;
            }
            
            flow += push;
            totalCost += push * dist[sink];
        }
        
        
        var result = new List<(int row, int col)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < graph[i + 1].Count; j++)
            {
                var (vv, capv, costv, revv) = graph[i + 1][j];
                if (vv > n && vv <= 2 * n && capv == 0)
                {
                    result.Add((i + 1, vv - n));
                    break;
                }
            }
        }
        
        Console.WriteLine(totalCost);
        foreach (var item in result)
        {
            Console.WriteLine($"{item.row} {item.col}");
        }
    }
}