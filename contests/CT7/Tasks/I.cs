using System;
using System.Collections.Generic;
using System.IO;

namespace CT7.Tasks;

internal static class MaxFlowFordFulkerson
{
    private sealed class Edge
    {
        public int To;
        public int Rev;
        public long Cap;

        public Edge(int to, int rev, long cap)
        {
            To = to;
            Rev = rev;
            Cap = cap;
        }
    }

    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        int m = fs.NextInt();
        if (n == 0) return;

        var graph = new List<Edge>[n + 1];
        for (int i = 1; i <= n; i++) graph[i] = new List<Edge>();

        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            long cap = fs.NextLong();
            AddEdge(graph, u, v, cap);
        }

        int source = fs.NextInt();
        int sink = fs.NextInt();
        long flow = 0;
        var parentVertex = new int[n + 1];
        var parentEdge = new int[n + 1];

        while (Bfs(graph, source, sink, parentVertex, parentEdge))
        {
            long add = long.MaxValue;
            for (int v = sink; v != source; v = parentVertex[v])
            {
                var e = graph[parentVertex[v]][parentEdge[v]];
                add = Math.Min(add, e.Cap);
            }

            for (int v = sink; v != source; v = parentVertex[v])
            {
                var e = graph[parentVertex[v]][parentEdge[v]];
                e.Cap -= add;
                graph[e.To][e.Rev].Cap += add;
            }

            flow += add;
        }

        Console.WriteLine(flow);
    }

    private static void AddEdge(List<Edge>[] graph, int from, int to, long cap)
    {
        var forward = new Edge(to, graph[to].Count, cap);
        var backward = new Edge(from, graph[from].Count, 0);
        graph[from].Add(forward);
        graph[to].Add(backward);
    }

    private static bool Bfs(List<Edge>[] graph, int source, int sink, int[] parentVertex, int[] parentEdge)
    {
        Array.Fill(parentVertex, -1);
        var queue = new Queue<int>();
        parentVertex[source] = source;
        queue.Enqueue(source);

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            for (int i = 0; i < graph[u].Count; i++)
            {
                var e = graph[u][i];
                if (e.Cap <= 0 || parentVertex[e.To] != -1) continue;
                parentVertex[e.To] = u;
                parentEdge[e.To] = i;
                if (e.To == sink) return true;
                queue.Enqueue(e.To);
            }
        }

        return false;
    }

    private sealed class FastScanner
    {
        private readonly TextReader reader;
        private readonly char[] buffer = new char[1 << 16];
        private int len;
        private int ptr;

        public FastScanner(TextReader reader)
        {
            this.reader = reader;
        }

        private int Read()
        {
            if (ptr >= len)
            {
                len = reader.Read(buffer, 0, buffer.Length);
                ptr = 0;
                if (len == 0) return 0;
            }

            return buffer[ptr++];
        }

        public int NextInt()
        {
            int c = Read();
            while (c <= ' ')
            {
                if (c == 0) return 0;
                c = Read();
            }

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            int result = 0;
            while (c > ' ')
            {
                result = result * 10 + c - '0';
                c = Read();
            }

            return result * sign;
        }

        public long NextLong()
        {
            int c = Read();
            while (c <= ' ')
            {
                if (c == 0) return 0;
                c = Read();
            }

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            long result = 0;
            while (c > ' ')
            {
                result = result * 10 + c - '0';
                c = Read();
            }

            return sign == 1 ? result : -result;
        }
    }
}
