using System;
using System.Collections.Generic;

namespace CT7.Tasks;

internal static class ShortestPathBFS
{
    public static void Solve()
    {
        string first = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(first)) return;
        var nm = first.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int n = int.Parse(nm[0]);
        int m = int.Parse(nm[1]);

        var names = new string[n];
        var id = new Dictionary<string, int>();
        for (int i = 0; i < n; i++)
        {
            names[i] = Console.ReadLine()!.Trim();
            id[names[i]] = i;
        }

        var graph = new List<int>[2 * n];
        var rev = new List<int>[2 * n];
        for (int i = 0; i < 2 * n; i++)
        {
            graph[i] = new List<int>();
            rev[i] = new List<int>();
        }

        for (int i = 0; i < m; i++)
        {
            var parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int a = Literal(parts[0], id, n);
            int b = Literal(parts[2], id, n);
            Add(graph, rev, a, b);
            Add(graph, rev, Neg(b, n), Neg(a, n));
        }

        var order = new List<int>();
        var used = new bool[2 * n];
        for (int i = 0; i < 2 * n; i++)
            if (!used[i]) Dfs1(i, graph, used, order);

        var comp = new int[2 * n];
        Array.Fill(comp, -1);
        int compId = 0;
        for (int i = order.Count - 1; i >= 0; i--)
            if (comp[order[i]] == -1) Dfs2(order[i], rev, comp, compId++);

        var invited = new List<string>();
        for (int i = 0; i < n; i++)
        {
            if (comp[i] == comp[i + n])
            {
                Console.WriteLine("-1");
                return;
            }
            if (comp[i] > comp[i + n]) invited.Add(names[i]);
        }

        Console.WriteLine(invited.Count);
        foreach (string name in invited) Console.WriteLine(name);
    }

    private static int Literal(string token, Dictionary<string, int> id, int n)
    {
        int v = id[token[1..]];
        return token[0] == '+' ? v : v + n;
    }

    private static int Neg(int v, int n) => v < n ? v + n : v - n;

    private static void Add(List<int>[] graph, List<int>[] rev, int from, int to)
    {
        graph[from].Add(to);
        rev[to].Add(from);
    }

    private static void Dfs1(int start, List<int>[] graph, bool[] used, List<int> order)
    {
        var stack = new Stack<(int node, int index)>();
        used[start] = true;
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
            if (!used[to])
            {
                used[to] = true;
                stack.Push((to, 0));
            }
        }
    }

    private static void Dfs2(int start, List<int>[] rev, int[] comp, int compId)
    {
        var stack = new Stack<int>();
        comp[start] = compId;
        stack.Push(start);
        while (stack.Count > 0)
        {
            int node = stack.Pop();
            foreach (int to in rev[node])
            {
                if (comp[to] != -1) continue;
                comp[to] = compId;
                stack.Push(to);
            }
        }
    }
}
