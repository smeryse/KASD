using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

internal static class TaskC
{
    private class Edge
    {
        public int To;
        public int Capacity;
        public int Flow;
        public int ReverseIndex;
    }

    private static List<Edge>[] graph;
    private static int[] parent;
    private static int[] parentEdge;

    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        int m = fs.NextInt();
        int s = fs.NextInt();
        int t = fs.NextInt();

        graph = new List<Edge>[n + 1];
        for (int i = 1; i <= n; i++)
            graph[i] = new List<Edge>();

        for (int i = 0; i < m; i++)
        {
            int u = fs.NextInt();
            int v = fs.NextInt();
            graph[u].Add(new Edge { To = v, Capacity = 1, Flow = 0, ReverseIndex = graph[v].Count });
            graph[v].Add(new Edge { To = u, Capacity = 0, Flow = 0, ReverseIndex = graph[u].Count - 1 });
        }

        int maxFlow = 0;
        while (Bfs(n, s, t))
        {
            int cur = t;
            while (cur != s)
            {
                int p = parent[cur];
                int idx = parentEdge[cur];
                graph[p][idx].Flow += 1;
                int revIdx = graph[p][idx].ReverseIndex;
                graph[cur][revIdx].Flow -= 1;
                cur = p;
            }
            maxFlow++;
        }

        if (maxFlow < 2)
        {
            Console.WriteLine("NO");
            return;
        }

        Console.WriteLine("YES");

        var path1 = FindPath(n, s, t);
        var path2 = FindPath(n, s, t);

        Console.WriteLine(string.Join(" ", path1));
        Console.WriteLine(string.Join(" ", path2));
    }

    private static bool Bfs(int n, int s, int t)
    {
        parent = new int[n + 1];
        parentEdge = new int[n + 1];
        Array.Fill(parent, -1);
        parent[s] = s;

        var queue = new Queue<int>();
        queue.Enqueue(s);

        while (queue.Count > 0 && parent[t] == -1)
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

        return parent[t] != -1;
    }

    private static List<int> FindPath(int n, int s, int t)
    {
        var path = new List<int>();
        var visited = new bool[n + 1];
        var pathList = new List<int>();

        if (DfsPath(s, t, pathList, visited))
        {
            return pathList;
        }
        return new List<int> { s, t };
    }

    private static bool DfsPath(int v, int t, List<int> path, bool[] visited)
    {
        path.Add(v);
        visited[v] = true;

        if (v == t) return true;

        for (int i = 0; i < graph[v].Count; i++)
        {
            var edge = graph[v][i];
            if (!visited[edge.To] && edge.Flow == 1)
            {
                edge.Flow = 0;
                if (DfsPath(edge.To, t, path, visited))
                    return true;
                edge.Flow = 1;
            }
        }

        path.RemoveAt(path.Count - 1);
        return false;
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

