using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT7.Tasks;

internal static class TwoSAT
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.In);
        int n = fs.NextInt();
        int m = fs.NextInt();
        if (n == 0) return;

        int total = 2 * n;
        var graph = new List<int>[total];
        var reverse = new List<int>[total];
        for (int i = 0; i < total; i++)
        {
            graph[i] = new List<int>();
            reverse[i] = new List<int>();
        }

        var clauses = new (int A, int B)[m];
        for (int i = 0; i < m; i++)
        {
            int a = fs.NextInt();
            int b = fs.NextInt();
            clauses[i] = (a, b);
            AddImplication(graph, reverse, Neg(a, n), Id(b, n));
            AddImplication(graph, reverse, Neg(b, n), Id(a, n));
        }

        var order = new List<int>(total);
        var visited = new bool[total];
        for (int i = 0; i < total; i++)
            if (!visited[i]) Dfs1(i, graph, visited, order);

        var comp = new int[total];
        Array.Fill(comp, -1);
        int compId = 0;
        for (int i = order.Count - 1; i >= 0; i--)
        {
            int v = order[i];
            if (comp[v] == -1) Dfs2(v, reverse, comp, compId++);
        }

        for (int i = 1; i <= n; i++)
        {
            if (comp[Id(i, n)] == comp[Id(-i, n)])
            {
                Console.WriteLine("No solution");
                return;
            }
        }

        var answer = new int[n + 1];
        Array.Fill(answer, -1);
        for (int i = 1; i <= n; i++)
        {
            answer[i] = 0;
            if (!IsSatisfiable(n, clauses, answer))
                answer[i] = 1;
        }

        var sb = new StringBuilder();
        for (int i = 1; i <= n; i++)
        {
            if (i > 1) sb.Append(' ');
            sb.Append(answer[i]);
        }
        Console.WriteLine(sb);
    }

    private static int Id(int literal, int n)
    {
        int v = Math.Abs(literal) - 1;
        return literal > 0 ? v : v + n;
    }

    private static int Neg(int literal, int n) => Id(-literal, n);

    private static bool IsSatisfiable(int n, (int A, int B)[] clauses, int[] fixedValues)
    {
        int total = 2 * n;
        var graph = new List<int>[total];
        var reverse = new List<int>[total];
        for (int i = 0; i < total; i++)
        {
            graph[i] = new List<int>();
            reverse[i] = new List<int>();
        }

        foreach (var (a, b) in clauses)
        {
            AddImplication(graph, reverse, Neg(a, n), Id(b, n));
            AddImplication(graph, reverse, Neg(b, n), Id(a, n));
        }

        for (int i = 1; i <= n; i++)
        {
            if (fixedValues[i] == -1) continue;
            int literal = fixedValues[i] == 1 ? i : -i;
            AddImplication(graph, reverse, Neg(literal, n), Id(literal, n));
        }

        var order = new List<int>(total);
        var visited = new bool[total];
        for (int i = 0; i < total; i++)
            if (!visited[i]) Dfs1(i, graph, visited, order);

        var comp = new int[total];
        Array.Fill(comp, -1);
        int compId = 0;
        for (int i = order.Count - 1; i >= 0; i--)
        {
            int v = order[i];
            if (comp[v] == -1) Dfs2(v, reverse, comp, compId++);
        }

        for (int i = 1; i <= n; i++)
            if (comp[Id(i, n)] == comp[Id(-i, n)])
                return false;
        return true;
    }

    private static void AddImplication(List<int>[] graph, List<int>[] reverse, int from, int to)
    {
        graph[from].Add(to);
        reverse[to].Add(from);
    }

    private static void Dfs1(int start, List<int>[] graph, bool[] visited, List<int> order)
    {
        var stack = new Stack<(int node, int index)>();
        visited[start] = true;
        stack.Push((start, 0));
        while (stack.Count > 0)
        {
            var (node, index) = stack.Pop();
            if (index == graph[node].Count)
            {
                order.Add(node);
                continue;
            }
            stack.Push((node, index + 1));
            int to = graph[node][index];
            if (!visited[to])
            {
                visited[to] = true;
                stack.Push((to, 0));
            }
        }
    }

    private static void Dfs2(int start, List<int>[] reverse, int[] comp, int compId)
    {
        var stack = new Stack<int>();
        comp[start] = compId;
        stack.Push(start);
        while (stack.Count > 0)
        {
            int node = stack.Pop();
            foreach (int to in reverse[node])
            {
                if (comp[to] != -1) continue;
                comp[to] = compId;
                stack.Push(to);
            }
        }
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
    }
}
