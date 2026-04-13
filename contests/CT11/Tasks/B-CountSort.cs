using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

internal static class TaskB
{
    private class Edge
    {
        public int To;
        public int Capacity;
        public int Flow;
        public int ReverseIndex;
    }

    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        int m = fs.NextInt();

        var graph = new List<Edge>[n + 1];
        for (int i = 1; i <= n; i++)
            graph[i] = new List<Edge>();

        var edges = new (int u, int v, int cap, int idx)[m];

        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            int c = fs.NextInt();

            edges[i] = (u, v, c, i + 1);

            graph[u].Add(new Edge { To = v, Capacity = c, Flow = 0, ReverseIndex = graph[v].Count });
            graph[v].Add(new Edge { To = u, Capacity = c, Flow = 0, ReverseIndex = graph[u].Count - 1 });
        }

        int source = 1;
        int sink = n;

        int maxFlow = 0;
        while (true)
        {
            var parent = new int[n + 1];
            var parentEdge = new int[n + 1];
            Array.Fill(parent, -1);
            parent[source] = source;

            var queue = new Queue<int>();
            queue.Enqueue(source);

            while (queue.Count > 0 && parent[sink] == -1)
            {
                int v = queue.Dequeue();
                for (int i = 0; i < graph[v].Count; i++)
                {
                    var edge = graph[v][i];
                    if (parent[edge.To] == -1 && edge.Capacity - edge.Flow > 0)
                    {
                        parent[edge.To] = v;
                        parentEdge[edge.To] = i;
                        queue.Enqueue(edge.To);
                    }
                }
            }

            if (parent[sink] == -1) break;

            int pushed = int.MaxValue;
            int cur = sink;
            while (cur != source)
            {
                int p = parent[cur];
                int idx = parentEdge[cur];
                pushed = Math.Min(pushed, graph[p][idx].Capacity - graph[p][idx].Flow);
                cur = p;
            }

            cur = sink;
            while (cur != source)
            {
                int p = parent[cur];
                int idx = parentEdge[cur];
                graph[p][idx].Flow += pushed;
                int revIdx = graph[p][idx].ReverseIndex;
                graph[cur][revIdx].Flow -= pushed;
                cur = p;
            }

            maxFlow += pushed;
        }

        var reachable = new bool[n + 1];
        reachable[source] = true;
        var q = new Queue<int>();
        q.Enqueue(source);

        while (q.Count > 0)
        {
            int v = q.Dequeue();
            foreach (var edge in graph[v])
            {
                if (!reachable[edge.To] && edge.Capacity - edge.Flow > 0)
                {
                    reachable[edge.To] = true;
                    q.Enqueue(edge.To);
                }
            }
        }

        var cutEdges = new List<int>();
        int cutCapacity = 0;

        for (int i = 0; i < m; i++)
        {
            var (u, v, cap, idx) = edges[i];
            bool uReach = reachable[u];
            bool vReach = reachable[v];

            if (uReach != vReach)
            {
                cutEdges.Add(idx);
                cutCapacity += cap;
            }
        }

        cutEdges.Sort();

        var sb = new StringBuilder();
        sb.AppendLine($"{cutEdges.Count} {cutCapacity}");
        sb.AppendLine(string.Join(" ", cutEdges));
        Console.Write(sb.ToString());
    }

    internal sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int pos;
        private int len;

        public FastScanner(Stream stream)
        {
            this.stream = stream;
            buffer = new byte[1 << 16];
            pos = 0;
            len = 0;
        }

        private byte ReadByte()
        {
            if (pos >= len)
            {
                pos = 0;
                len = stream.Read(buffer, 0, buffer.Length);
                if (len == 0) return 0;
            }
            return buffer[pos++];
        }

        public int NextInt()
        {
            int c = ReadByte();
            while (c <= 32) c = ReadByte();
            int sign = 1;
            if (c == '-') { sign = -1; c = ReadByte(); }
            int res = 0;
            while (c > 32)
            {
                res = res * 10 + (c - '0');
                c = ReadByte();
            }
            return res * sign;
        }
    }
}

