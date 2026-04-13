using System;
using System.Collections.Generic;

class Program
{
    static int m, k, n;
    static int[] matchGreen, matchYellow;
    static bool[,] forbidden;
    static bool[] visited;
    static Queue<string> tokens = new Queue<string>();

    static int NextInt()
    {
        while (tokens.Count == 0)
        {
            string line = Console.ReadLine();
            if (line == null) break;
            foreach (var p in line.Trim().Split(new char[]{' ','\t'}, StringSplitOptions.RemoveEmptyEntries))
                tokens.Enqueue(p);
        }
        return int.Parse(tokens.Dequeue());
    }

    static bool DfsGreen(int g)
    {
        for (int y = 0; y < k; y++)
        {
            if (!forbidden[g, y] && !visited[y])
            {
                visited[y] = true;
                if (matchYellow[y] == -1 || DfsGreen(matchYellow[y]))
                {
                    matchGreen[g] = y;
                    matchYellow[y] = g;
                    return true;
                }
            }
        }
        return false;
    }

    static void Main()
    {
        m = NextInt(); k = NextInt(); n = NextInt();

        forbidden = new bool[m, k];
        int t = NextInt();
        for (int i = 0; i < t; i++)
        {
            int g = NextInt() - 1;
            int y = NextInt() - m - 1;
            forbidden[g, y] = true;
        }

        int q = NextInt();
        var lonelyGreen  = new HashSet<int>();
        var lonelyYellow = new HashSet<int>();
        for (int i = 0; i < q; i++)
        {
            int x = NextInt();
            if (x <= m) lonelyGreen.Add(x - 1);
            else        lonelyYellow.Add(x - m - 1);
        }

        matchGreen  = new int[m];
        matchYellow = new int[k];
        for (int i = 0; i < m; i++) matchGreen[i]  = -1;
        for (int j = 0; j < k; j++) matchYellow[j] = -1;

        // Обязательные зелёные — первыми (приоритет в паросочетании)
        foreach (int g in lonelyGreen) { visited = new bool[k]; DfsGreen(g); }
        // Остальные зелёные
        for (int g = 0; g < m; g++)
            if (!lonelyGreen.Contains(g)) { visited = new bool[k]; DfsGreen(g); }

        // Проверяем покрытие обязательных
        bool ok = true;
        foreach (int g in lonelyGreen)
            if (matchGreen[g] == -1) { ok = false; break; }
        if (ok)
            foreach (int y in lonelyYellow)
                if (matchYellow[y] == -1) { ok = false; break; }

        int matched = 0;
        for (int i = 0; i < m; i++) if (matchGreen[i] != -1) matched++;

        if (!ok || matched < n) { Console.WriteLine("NO"); return; }

        // Обязательные зелёные = lonelyGreen + партнёры lonelyYellow
        var mandatoryGreen = new HashSet<int>(lonelyGreen);
        foreach (int y in lonelyYellow)
            mandatoryGreen.Add(matchYellow[y]);

        var result = new List<(int g, int y)>();
        foreach (int g in mandatoryGreen)
            result.Add((g, matchGreen[g]));

        if (result.Count > n) { Console.WriteLine("NO"); return; }

        for (int g = 0; g < m && result.Count < n; g++)
            if (!mandatoryGreen.Contains(g) && matchGreen[g] != -1)
                result.Add((g, matchGreen[g]));

        if (result.Count < n) { Console.WriteLine("NO"); return; }

        Console.WriteLine("YES");
        foreach (var (g, y) in result)
            Console.WriteLine((g + 1) + " " + (y + m + 1));
    }
}