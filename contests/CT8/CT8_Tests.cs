using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT8.Tasks;

/// <summary>
/// Генератор тестов и проверка решений для задач A-F (кратчайшие пути)
/// </summary>
internal class CT8_TestRunner
{
    private static readonly Random Rand = new Random(42);

    public static void RunAllTests()
    {
        int passed = 0;
        int failed = 0;

        Console.WriteLine("Запуск тестов для задач CT8 (кратчайшие пути)");
        Console.WriteLine(new string('=', 60));

        // Задача A - Флойд
        Console.WriteLine("\n--- Задача A: Флойд-Уоршелл ---");
        for (int i = 1; i <= 15; i++)
        {
            if (RunTestA(i, GenerateTestA(i)))
                passed++;
            else
                failed++;
        }

        // Задача B - Дейкстра
        Console.WriteLine("\n--- Задача B: Дейкстра ---");
        for (int i = 1; i <= 15; i++)
        {
            if (RunTestB(i, GenerateTestB(i)))
                passed++;
            else
                failed++;
        }

        // Задача C - Цикл отрицательного веса
        Console.WriteLine("\n--- Задача C: Цикл отрицательного веса ---");
        for (int i = 1; i <= 15; i++)
        {
            if (RunTestC(i, GenerateTestC(i)))
                passed++;
            else
                failed++;
        }

        // Задача D - Кратчайший путь длины K
        Console.WriteLine("\n--- Задача D: Кратчайший путь длины K ---");
        for (int i = 1; i <= 15; i++)
        {
            if (RunTestD(i, GenerateTestD(i)))
                passed++;
            else
                failed++;
        }

        // Задача E - Беллман-Форд с -∞
        Console.WriteLine("\n--- Задача E: Кратчайшие пути (Беллман-Форд) ---");
        for (int i = 1; i <= 15; i++)
        {
            if (RunTestE(i, GenerateTestE(i)))
                passed++;
            else
                failed++;
        }

        // Задача F - Кефир
        Console.WriteLine("\n--- Задача F: В поисках кефира ---");
        for (int i = 1; i <= 15; i++)
        {
            if (RunTestF(i, GenerateTestF(i)))
                passed++;
            else
                failed++;
        }

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine($"Результаты: {passed} пройдено, {failed} провалено из 90");

        if (failed == 0)
        {
            Console.WriteLine("✓ Все тесты пройдены!");
        }
        else
        {
            Console.WriteLine($"✗ Провалено тестов: {failed}");
            Environment.ExitCode = 1;
        }
    }

    #region Задача A - Флойд

    private static bool RunTestA(int testNum, (int n, int[,] matrix, string expected) testData)
    {
        var (n, matrix, expected) = testData;
        try
        {
            string input = FormatInputA(n, matrix);
            string actual = RunSolutionA(input);

            if (Normalize(actual) == Normalize(expected))
            {
                Console.WriteLine($"Тест A{testNum,2}: OK (n={n})");
                return true;
            }
            else
            {
                Console.WriteLine($"Тест A{testNum,2}: FAIL");
                Console.WriteLine($"  Ожидалось: {Truncate(expected, 60)}");
                Console.WriteLine($"  Получено:  {Truncate(actual, 60)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест A{testNum,2}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static string FormatInputA(int n, int[,] matrix)
    {
        var sb = new StringBuilder();
        sb.AppendLine(n.ToString());
        for (int i = 0; i < n; i++)
        {
            var row = new int[n];
            for (int j = 0; j < n; j++)
                row[j] = matrix[i, j];
            sb.AppendLine(string.Join(" ", row));
        }
        return sb.ToString();
    }

    private static string RunSolutionA(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            Floyd.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static (int n, int[,] matrix, string expected) GenerateTestA(int testNum)
    {
        switch (testNum)
        {
            case 1: // Пример из условия
                return (4, new int[,] {
                    {0, 5, 9, 100}, {100, 0, 2, 8}, {100, 100, 0, 7}, {4, 100, 100, 0}
                }, "0 5 7 13\n12 0 2 8\n11 16 0 7\n4 9 11 0");

            case 2: // Одна вершина
                return (1, new int[,] { {0} }, "0");

            case 3: // Две вершины
                return (2, new int[,] { {0, 1}, {2, 0} }, "0 1\n2 0");

            case 4: // Три вершины, полный граф
                return (3, new int[,] {
                    {0, 1, 5}, {2, 0, 3}, {100, 100, 0}
                }, "0 1 4\n2 0 3\n100 100 0");

            case 5: // Все вершины достижимы
                return (3, new int[,] {
                    {0, 1, 1}, {1, 0, 1}, {1, 1, 0}
                }, "0 1 1\n1 0 1\n1 1 0");

            case 6: // Цепочка
                return (4, new int[,] {
                    {0, 1, 100, 100}, {100, 0, 1, 100}, {100, 100, 0, 1}, {100, 100, 100, 0}
                }, "0 1 2 3\n100 0 1 2\n100 100 0 1\n100 100 100 0");

            case 7: // Отрицательные веса (но без отрицательных циклов)
                return (3, new int[,] {
                    {0, -1, 100}, {100, 0, -2}, {100, 100, 0}
                }, "0 -1 -3\n98 0 -2\n100 99 0");

            case 8: // Большой граф
                int n8 = 5;
                var m8 = new int[n8, n8];
                for (int i = 0; i < n8; i++)
                    for (int j = 0; j < n8; j++)
                        m8[i, j] = (i == j) ? 0 : 10;
                return (n8, m8, string.Join("\n", Enumerable.Range(0, n8).Select(i => string.Join(" ", Enumerable.Range(0, n8).Select(j => (i == j) ? 0 : 10)))));

            case 9: // Граф с нулевыми рёбрами
                return (3, new int[,] {
                    {0, 0, 0}, {0, 0, 0}, {0, 0, 0}
                }, "0 0 0\n0 0 0\n0 0 0");

            case 10: // Несвязный граф
                return (4, new int[,] {
                    {0, 1, 100, 100}, {100, 0, 100, 100}, {100, 100, 0, 1}, {100, 100, 100, 0}
                }, "0 1 100 100\n100 0 100 100\n100 100 0 1\n100 100 100 0");

            case 11: // Случайный малый граф
                return GenerateRandomTestA(5, 10);

            case 12: // Случайный малый граф
                return GenerateRandomTestA(6, 15);

            case 13: // Случайный малый граф
                return GenerateRandomTestA(7, 20);

            case 14: // Случайный малый граф
                return GenerateRandomTestA(8, 25);

            case 15: // Случайный малый граф
                return GenerateRandomTestA(10, 30);

            default:
                return GenerateTestA(1);
        }
    }

    private static (int n, int[,] matrix, string expected) GenerateRandomTestA(int n, int density)
    {
        const int INF = 100000;
        var matrix = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                matrix[i, j] = (i == j) ? 0 : (Rand.Next(1, 20) > density / 3 ? INF : Rand.Next(-10, 20));

        // Флойд для вычисления ожидаемого ответа
        var dist = (int[,])matrix.Clone();
        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (dist[i, k] != INF && dist[k, j] != INF)
                        dist[i, j] = Math.Min(dist[i, j], dist[i, k] + dist[k, j]);

        var lines = new string[n];
        for (int i = 0; i < n; i++)
        {
            var row = new int[n];
            for (int j = 0; j < n; j++)
                row[j] = dist[i, j];
            lines[i] = string.Join(" ", row);
        }

        return (n, matrix, string.Join("\n", lines));
    }

    #endregion

    #region Задача B - Дейкстра

    private static bool RunTestB(int testNum, (int n, int m, List<(int u, int v, int w)> edges, string expected) testData)
    {
        var (n, m, edges, expected) = testData;
        try
        {
            string input = FormatInputB(n, m, edges);
            string actual = RunSolutionB(input);

            if (Normalize(actual) == Normalize(expected))
            {
                Console.WriteLine($"Тест B{testNum,2}: OK (n={n}, m={m})");
                return true;
            }
            else
            {
                Console.WriteLine($"Тест B{testNum,2}: FAIL");
                Console.WriteLine($"  Ожидалось: {Truncate(expected, 60)}");
                Console.WriteLine($"  Получено:  {Truncate(actual, 60)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест B{testNum,2}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static string FormatInputB(int n, int m, List<(int, int, int)> edges)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{n} {m}");
        foreach (var (u, v, w) in edges)
            sb.AppendLine($"{u} {v} {w}");
        return sb.ToString();
    }

    private static string RunSolutionB(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            ShortestPath2.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static (int n, int m, List<(int, int, int)>, string expected) GenerateTestB(int testNum)
    {
        switch (testNum)
        {
            case 1: // Пример из условия
                return (4, 5, new List<(int, int, int)> { (1, 2, 1), (1, 3, 5), (2, 4, 8), (3, 4, 1), (2, 3, 3) }, "0 1 4 5");

            case 2: // Две вершины
                return (2, 1, new List<(int, int, int)> { (1, 2, 10) }, "0 10");

            case 3: // Цепочка
                return (4, 3, new List<(int, int, int)> { (1, 2, 1), (2, 3, 2), (3, 4, 3) }, "0 1 3 6");

            case 4: // Звезда
                return (5, 4, new List<(int, int, int)> { (1, 2, 1), (1, 3, 2), (1, 4, 3), (1, 5, 4) }, "0 1 2 3 4");

            case 5: // Цикл
                return (4, 4, new List<(int, int, int)> { (1, 2, 1), (2, 3, 2), (3, 4, 3), (4, 1, 4) }, "0 1 3 4");

            case 6: // Полный граф K4
                return (4, 6, new List<(int, int, int)> { (1, 2, 1), (1, 3, 2), (1, 4, 3), (2, 3, 1), (2, 4, 2), (3, 4, 1) }, "0 1 2 3");

            case 7: // С нулевыми весами
                return (3, 2, new List<(int, int, int)> { (1, 2, 0), (2, 3, 0) }, "0 0 0");

            case 8: // Большой вес
                return (3, 2, new List<(int, int, int)> { (1, 2, 10000), (2, 3, 10000) }, "0 10000 20000");

            case 9: // Несколько путей
                return (4, 5, new List<(int, int, int)> { (1, 2, 10), (1, 3, 5), (2, 4, 1), (3, 2, 1), (3, 4, 10) }, "0 6 5 7");

            case 10: // Сложный граф
                return (5, 7, new List<(int, int, int)> { (1, 2, 1), (1, 3, 4), (2, 3, 2), (2, 4, 5), (3, 4, 1), (3, 5, 3), (4, 5, 1) }, "0 1 3 4 5");

            case 11: // Случайный граф
                return GenerateRandomTestB(10, 20);

            case 12: // Случайный граф
                return GenerateRandomTestB(15, 30);

            case 13: // Случайный граф
                return GenerateRandomTestB(20, 40);

            case 14: // Случайный граф
                return GenerateRandomTestB(25, 50);

            case 15: // Случайный граф
                return GenerateRandomTestB(30, 60);

            default:
                return GenerateTestB(1);
        }
    }

    private static (int n, int m, List<(int, int, int)>, string expected) GenerateRandomTestB(int n, int targetEdges)
    {
        var edges = new HashSet<(int, int)>();
        var edgeList = new List<(int, int, int)>();

        while (edgeList.Count < targetEdges && edges.Count < n * (n - 1) / 2)
        {
            int u = Rand.Next(1, n + 1);
            int v = Rand.Next(1, n + 1);
            if (u != v)
            {
                var edge = u < v ? (u, v) : (v, u);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                    edgeList.Add((edge.Item1, edge.Item2, Rand.Next(1, 100)));
                }
            }
        }

        // Дейкстра для ожидаемого ответа
        const long INF = long.MaxValue / 4;
        var adj = new List<(int, int)>[n + 1];
        for (int i = 1; i <= n; i++) adj[i] = new List<(int, int)>();
        foreach (var (u, v, w) in edgeList)
        {
            adj[u].Add((v, w));
            adj[v].Add((u, w));
        }

        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++) dist[i] = INF;
        dist[1] = 0;

        var pq = new PriorityQueue<int, long>();
        pq.Enqueue(1, 0);

        while (pq.Count > 0)
        {
            int u = pq.Dequeue();
            foreach (var (v, w) in adj[u])
            {
                if (dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    pq.Enqueue(v, dist[v]);
                }
            }
        }

        var result = new long[n];
        for (int i = 1; i <= n; i++) result[i - 1] = dist[i];

        return (n, edgeList.Count, edgeList, string.Join(" ", result));
    }

    #endregion

    #region Общие методы

    private static string Normalize(string s) => s.Replace("\r\n", "\n").TrimEnd();
    private static string Truncate(string s, int len) => s.Length <= len ? s : s.Substring(0, len) + "...";

    #endregion

    #region Задача C

    private static bool RunTestC(int testNum, (int n, int[,] matrix, bool hasCycle, string expected) testData)
    {
        var (n, matrix, hasCycle, expected) = testData;
        try
        {
            string input = FormatInputC(n, matrix);
            string actual = RunSolutionC(input);

            if (Normalize(actual) == Normalize(expected))
            {
                Console.WriteLine($"Тест C{testNum,2}: OK (n={n})");
                return true;
            }
            else
            {
                // Проверяем только наличие/отсутствие цикла
                bool actualHasCycle = actual.Contains("YES");
                if (actualHasCycle == hasCycle)
                {
                    Console.WriteLine($"Тест C{testNum,2}: OK (n={n}, цикл={(hasCycle ? "да" : "нет")})");
                    return true;
                }

                Console.WriteLine($"Тест C{testNum,2}: FAIL");
                Console.WriteLine($"  Ожидалось: {Truncate(expected, 60)}");
                Console.WriteLine($"  Получено:  {Truncate(actual, 60)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест C{testNum,2}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static string FormatInputC(int n, int[,] matrix)
    {
        var sb = new StringBuilder();
        sb.AppendLine(n.ToString());
        for (int i = 0; i < n; i++)
        {
            var row = new int[n];
            for (int j = 0; j < n; j++)
                row[j] = matrix[i, j];
            sb.AppendLine(string.Join(" ", row));
        }
        return sb.ToString();
    }

    private static string RunSolutionC(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            NegativeCycle.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static (int n, int[,] matrix, bool hasCycle, string expected) GenerateTestC(int testNum)
    {
        const int INF = 100000;

        switch (testNum)
        {
            case 1: // Пример из условия
                return (2, new int[,] { {0, -1}, {-1, 0} }, true, "YES\n2\n2 1");

            case 2: // Без цикла
                return (3, new int[,] { {0, 1, INF}, {INF, 0, 2}, {INF, INF, 0} }, false, "NO");

            case 3: // С циклом
                return (3, new int[,] { {0, 1, INF}, {INF, 0, -5}, {2, INF, 0} }, true, "YES");

            case 4: // Одна вершина
                return (1, new int[,] { {0} }, false, "NO");

            case 5: // Отрицательное ребро без цикла
                return (2, new int[,] { {0, -5}, {INF, 0} }, false, "NO");

            case 6: // Нулевой цикл
                return (2, new int[,] { {0, 0}, {0, 0} }, false, "NO");

            case 7: // Большой отрицательный цикл
                return (3, new int[,] { {0, -10, INF}, {INF, 0, -10}, {-10, INF, 0} }, true, "YES");

            case 8: // Положительный цикл
                return (3, new int[,] { {0, 1, INF}, {INF, 0, 1}, {1, INF, 0} }, false, "NO");

            case 9: // Сложный граф без цикла
                return (4, new int[,] {
                    {0, 1, INF, INF}, {INF, 0, 2, INF}, {INF, INF, 0, 3}, {INF, INF, INF, 0}
                }, false, "NO");

            case 10: // Сложный граф с циклом
                return (4, new int[,] {
                    {0, 1, INF, INF}, {INF, 0, 2, INF}, {INF, INF, 0, -10}, {1, INF, INF, 0}
                }, true, "YES");

            case 11: // Случайный граф
                return GenerateRandomTestC(5, 0.3);

            case 12: // Случайный граф
                return GenerateRandomTestC(6, 0.4);

            case 13: // Случайный граф
                return GenerateRandomTestC(7, 0.5);

            case 14: // Случайный граф
                return GenerateRandomTestC(8, 0.3);

            case 15: // Случайный граф
                return GenerateRandomTestC(10, 0.4);

            default:
                return GenerateTestC(1);
        }
    }

    private static (int n, int[,] matrix, bool hasCycle, string expected) GenerateRandomTestC(int n, double negProb)
    {
        const int INF = 100000;
        var matrix = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                    matrix[i, j] = 0;
                else if (Rand.NextDouble() < 0.3)
                    matrix[i, j] = INF;
                else if (Rand.NextDouble() < negProb)
                    matrix[i, j] = -Rand.Next(1, 20);
                else
                    matrix[i, j] = Rand.Next(1, 20);
            }

        // Проверяем наличие отрицательного цикла через Флойд
        var dist = (int[,])matrix.Clone();
        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (dist[i, k] != INF && dist[k, j] != INF)
                        dist[i, j] = Math.Min(dist[i, j], dist[i, k] + dist[k, j]);

        bool hasCycle = false;
        for (int i = 0; i < n; i++)
            if (dist[i, i] < 0)
            {
                hasCycle = true;
                break;
            }

        return (n, matrix, hasCycle, hasCycle ? "YES" : "NO");
    }

    #endregion

    #region Задача D

    private static bool RunTestD(int testNum, (int n, int m, int k, int s, List<(int u, int v, int w)> edges, string expected) testData)
    {
        var (n, m, k, s, edges, expected) = testData;
        try
        {
            string input = FormatInputD(n, m, k, s, edges);
            string actual = RunSolutionD(input);

            if (Normalize(actual) == Normalize(expected))
            {
                Console.WriteLine($"Тест D{testNum,2}: OK (n={n}, k={k})");
                return true;
            }
            else
            {
                Console.WriteLine($"Тест D{testNum,2}: FAIL");
                Console.WriteLine($"  Ожидалось: {Truncate(expected, 60)}");
                Console.WriteLine($"  Получено:  {Truncate(actual, 60)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест D{testNum,2}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static string FormatInputD(int n, int m, int k, int s, List<(int, int, int)> edges)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{n} {m} {k} {s}");
        foreach (var (u, v, w) in edges)
            sb.AppendLine($"{u} {v} {w}");
        return sb.ToString();
    }

    private static string RunSolutionD(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            ShortestPathK.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static (int n, int m, int k, int s, List<(int, int, int)>, string expected) GenerateTestD(int testNum)
    {
        switch (testNum)
        {
            case 1: // Пример 1
                return (3, 3, 1, 1, new List<(int, int, int)> { (1, 2, 100), (2, 3, 300), (1, 3, 2) }, "-1\n100\n2");

            case 2: // Пример 2
                return (3, 3, 2, 1, new List<(int, int, int)> { (1, 2, 100), (2, 3, 300), (1, 3, 2) }, "-1\n-1\n400");

            case 3: // K=0
                return (3, 2, 0, 1, new List<(int, int, int)> { (1, 2, 10), (2, 3, 20) }, "0\n-1\n-1");

            case 4: // Цепочка K=2
                return (4, 3, 2, 1, new List<(int, int, int)> { (1, 2, 1), (2, 3, 2), (3, 4, 3) }, "-1\n-1\n3\n-1");

            case 5: // K=3 цепочка
                return (4, 3, 3, 1, new List<(int, int, int)> { (1, 2, 1), (2, 3, 2), (3, 4, 3) }, "-1\n-1\n-1\n6");

            case 6: // Несколько путей
                return (3, 4, 2, 1, new List<(int, int, int)> { (1, 2, 1), (1, 3, 10), (2, 3, 1), (3, 2, 1) }, "-1\n11\n2");

            case 7: // Отрицательные веса
                return (3, 3, 2, 1, new List<(int, int, int)> { (1, 2, 5), (2, 3, -3), (1, 3, 10) }, "-1\n-1\n2");

            case 8: // K больше числа вершин
                return (3, 3, 5, 1, new List<(int, int, int)> { (1, 2, 1), (2, 3, 1), (3, 1, 1) }, "-1\n-1\n5");

            case 9: // Полный граф
                return (4, 6, 2, 1, new List<(int, int, int)> { (1, 2, 1), (1, 3, 2), (1, 4, 3), (2, 3, 1), (2, 4, 2), (3, 4, 1) }, "-1\n-1\n2\n3");

            case 10: // Несвязный
                return (4, 2, 2, 1, new List<(int, int, int)> { (1, 2, 1), (3, 4, 1) }, "-1\n-1\n-1\n-1");

            case 11: // Случайный
                return GenerateRandomTestD(5, 8, 2);

            case 12: // Случайный
                return GenerateRandomTestD(6, 10, 3);

            case 13: // Случайный
                return GenerateRandomTestD(7, 12, 4);

            case 14: // Случайный
                return GenerateRandomTestD(8, 15, 2);

            case 15: // Случайный
                return GenerateRandomTestD(10, 20, 3);

            default:
                return GenerateTestD(1);
        }
    }

    private static (int n, int m, int k, int s, List<(int, int, int)>, string expected) GenerateRandomTestD(int n, int targetEdges, int k)
    {
        var edges = new HashSet<(int, int)>();
        var edgeList = new List<(int, int, int)>();

        while (edgeList.Count < targetEdges && edges.Count < n * n)
        {
            int u = Rand.Next(1, n + 1);
            int v = Rand.Next(1, n + 1);
            if (u != v)
            {
                var edge = (u, v);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                    edgeList.Add((u, v, Rand.Next(-10, 20)));
                }
            }
        }

        // Динамика для ожидаемого ответа
        const long INF = long.MaxValue / 4;
        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++) dist[i] = INF;
        dist[1] = 0;

        for (int step = 0; step < k; step++)
        {
            long[] newDist = new long[n + 1];
            for (int i = 1; i <= n; i++) newDist[i] = INF;

            foreach (var (u, v, w) in edgeList)
            {
                if (dist[u] != INF)
                    newDist[v] = Math.Min(newDist[v], dist[u] + w);
            }
            dist = newDist;
        }

        var result = new string[n];
        for (int i = 1; i <= n; i++)
            result[i - 1] = (dist[i] == INF) ? "-1" : dist[i].ToString();

        return (n, edgeList.Count, k, 1, edgeList, string.Join("\n", result));
    }

    #endregion

    #region Задача E

    private static bool RunTestE(int testNum, (int n, int m, int s, List<(int u, int v, long w)> edges, string expected) testData)
    {
        var (n, m, s, edges, expected) = testData;
        try
        {
            string input = FormatInputE(n, m, s, edges);
            string actual = RunSolutionE(input);

            if (Normalize(actual) == Normalize(expected))
            {
                Console.WriteLine($"Тест E{testNum,2}: OK (n={n}, m={m})");
                return true;
            }
            else
            {
                Console.WriteLine($"Тест E{testNum,2}: FAIL");
                Console.WriteLine($"  Ожидалось: {Truncate(expected, 60)}");
                Console.WriteLine($"  Получено:  {Truncate(actual, 60)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест E{testNum,2}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static string FormatInputE(int n, int m, int s, List<(int, int, long)> edges)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{n} {m} {s}");
        foreach (var (u, v, w) in edges)
            sb.AppendLine($"{u} {v} {w}");
        return sb.ToString();
    }

    private static string RunSolutionE(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            ShortestPaths.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static (int n, int m, int s, List<(int, int, long)>, string expected) GenerateTestE(int testNum)
    {
        switch (testNum)
        {
            case 1: // Пример из условия
                return (6, 7, 1, new List<(int, int, long)> { (1, 2, 10), (2, 3, 5), (1, 3, 100), (3, 5, 7), (5, 4, 10), (4, 3, -18), (6, 1, -1) }, "0\n10\n-\n-\n-\n*");

            case 2: // Простой граф
                return (3, 2, 1, new List<(int, int, long)> { (1, 2, 5), (2, 3, 3) }, "0\n5\n8");

            case 3: // Несвязный
                return (4, 2, 1, new List<(int, int, long)> { (1, 2, 1), (3, 4, 1) }, "0\n1\n*\n*");

            case 4: // Отрицательное ребро без цикла
                return (3, 3, 1, new List<(int, int, long)> { (1, 2, 5), (2, 3, -3), (1, 3, 10) }, "0\n5\n2");

            case 5: // Отрицательный цикл
                return (3, 3, 1, new List<(int, int, long)> { (1, 2, 1), (2, 3, -5), (3, 2, 1) }, "0\n-\n-");

            case 6: // Петля
                return (2, 2, 1, new List<(int, int, long)> { (1, 1, 0), (1, 2, 5) }, "0\n5");

            case 7: // Отрицательная петля
                return (2, 2, 1, new List<(int, int, long)> { (1, 1, -1), (1, 2, 5) }, "-\n-");

            case 8: // Кратные рёбра
                return (2, 3, 1, new List<(int, int, long)> { (1, 2, 10), (1, 2, 5), (1, 2, 8) }, "0\n5");

            case 9: // Большой вес
                return (3, 2, 1, new List<(int, int, long)> { (1, 2, 1000000000), (2, 3, 1000000000) }, "0\n1000000000\n2000000000");

            case 10: // Сложный граф
                return (5, 6, 1, new List<(int, int, long)> { (1, 2, 1), (2, 3, 2), (3, 4, 3), (4, 5, 4), (1, 5, 20), (2, 5, 10) }, "0\n1\n3\n6\n10");

            case 11: // Случайный
                return GenerateRandomTestE(5, 8);

            case 12: // Случайный
                return GenerateRandomTestE(6, 10);

            case 13: // Случайный
                return GenerateRandomTestE(7, 12);

            case 14: // Случайный
                return GenerateRandomTestE(8, 15);

            case 15: // Случайный
                return GenerateRandomTestE(10, 20);

            default:
                return GenerateTestE(1);
        }
    }

    private static (int n, int m, int s, List<(int, int, long)>, string expected) GenerateRandomTestE(int n, int targetEdges)
    {
        var edges = new List<(int, int, long)>();
        for (int i = 0; i < targetEdges; i++)
        {
            int u = Rand.Next(1, n + 1);
            int v = Rand.Next(1, n + 1);
            long w = Rand.Next(-10, 20);
            edges.Add((u, v, w));
        }

        // Bellman-Ford для ожидаемого ответа (упрощённо)
        const long INF = long.MaxValue / 4;
        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++) dist[i] = INF;
        dist[1] = 0;

        for (int iter = 0; iter < n - 1; iter++)
        {
            foreach (var (u, v, w) in edges)
            {
                if (dist[u] != INF && dist[u] + w < dist[v])
                    dist[v] = dist[u] + w;
            }
        }

        bool[] negReach = new bool[n + 1];
        for (int iter = 0; iter < n; iter++)
        {
            foreach (var (u, v, w) in edges)
            {
                if (dist[u] != INF && dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    negReach[v] = true;
                }
                if (negReach[u])
                    negReach[v] = true;
            }
        }

        var result = new string[n];
        for (int i = 1; i <= n; i++)
        {
            if (dist[i] == INF)
                result[i - 1] = "*";
            else if (negReach[i])
                result[i - 1] = "-";
            else
                result[i - 1] = dist[i].ToString();
        }

        return (n, edges.Count, 1, edges, string.Join("\n", result));
    }

    #endregion

    #region Задача F

    private static bool RunTestF(int testNum, (int n, int m, List<(int u, int v, int w)> edges, int a, int b, int c, long expected) testData)
    {
        var (n, m, edges, a, b, c, expected) = testData;
        try
        {
            string input = FormatInputF(n, m, edges, a, b, c);
            string actual = RunSolutionF(input);

            if (Normalize(actual) == Normalize(expected.ToString()))
            {
                Console.WriteLine($"Тест F{testNum,2}: OK (n={n})");
                return true;
            }
            else
            {
                Console.WriteLine($"Тест F{testNum,2}: FAIL");
                Console.WriteLine($"  Ожидалось: {expected}");
                Console.WriteLine($"  Получено:  {Truncate(actual, 60)}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест F{testNum,2}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static string FormatInputF(int n, int m, List<(int, int, int)> edges, int a, int b, int c)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{n} {m}");
        foreach (var (u, v, w) in edges)
            sb.AppendLine($"{u} {v} {w}");
        sb.AppendLine($"{a} {b} {c}");
        return sb.ToString();
    }

    private static string RunSolutionF(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            LostKefir.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static (int n, int m, List<(int, int, int)>, int a, int b, int c, long expected) GenerateTestF(int testNum)
    {
        switch (testNum)
        {
            case 1: // Пример 1
                return (4, 4, new List<(int, int, int)> { (1, 2, 3), (2, 3, 1), (3, 4, 7), (4, 2, 10) }, 1, 4, 3, 11);

            case 2: // Пример 2 (несвязный)
                return (4, 2, new List<(int, int, int)> { (1, 2, 10), (2, 3, 5) }, 1, 2, 4, -1);

            case 3: // Прямой путь
                return (3, 2, new List<(int, int, int)> { (1, 2, 1), (2, 3, 1) }, 1, 2, 3, 2);

            case 4: // Звезда
                return (5, 4, new List<(int, int, int)> { (1, 2, 1), (1, 3, 2), (1, 4, 3), (1, 5, 4) }, 2, 3, 4, 7);

            case 5: // Треугольник
                return (3, 3, new List<(int, int, int)> { (1, 2, 1), (2, 3, 2), (1, 3, 5) }, 1, 2, 3, 3);

            case 6: // Полный граф K4
                return (4, 6, new List<(int, int, int)> { (1, 2, 1), (1, 3, 2), (1, 4, 3), (2, 3, 1), (2, 4, 2), (3, 4, 1) }, 1, 2, 4, 3);

            case 7: // Большое расстояние
                return (5, 4, new List<(int, int, int)> { (1, 2, 100), (2, 3, 100), (3, 4, 100), (4, 5, 100) }, 1, 3, 5, 400);

            case 8: // Несвязный граф
                return (6, 3, new List<(int, int, int)> { (1, 2, 1), (3, 4, 1), (5, 6, 1) }, 1, 3, 5, -1);

            case 9: // a,b,c рядом
                return (4, 4, new List<(int, int, int)> { (1, 2, 1), (2, 3, 1), (3, 4, 1), (4, 1, 1) }, 1, 2, 3, 2);

            case 10: // Сложный граф
                return (6, 7, new List<(int, int, int)> { (1, 2, 2), (2, 3, 3), (3, 4, 1), (4, 5, 2), (5, 6, 1), (1, 6, 10), (2, 5, 5) }, 1, 3, 6, 9);

            case 11: // Случайный
                return GenerateRandomTestF(8, 12);

            case 12: // Случайный
                return GenerateRandomTestF(10, 15);

            case 13: // Случайный
                return GenerateRandomTestF(12, 18);

            case 14: // Случайный
                return GenerateRandomTestF(15, 20);

            case 15: // Случайный
                return GenerateRandomTestF(20, 30);

            default:
                return GenerateTestF(1);
        }
    }

    private static (int n, int m, List<(int, int, int)>, int a, int b, int c, long expected) GenerateRandomTestF(int n, int targetEdges)
    {
        var edges = new HashSet<(int, int)>();
        var edgeList = new List<(int, int, int)>();

        while (edgeList.Count < targetEdges && edges.Count < n * (n - 1) / 2)
        {
            int u = Rand.Next(1, n + 1);
            int v = Rand.Next(1, n + 1);
            if (u != v)
            {
                var edge = u < v ? (u, v) : (v, u);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                    edgeList.Add((edge.Item1, edge.Item2, Rand.Next(1, 50)));
                }
            }
        }

        int a = Rand.Next(1, n + 1);
        int b = Rand.Next(1, n + 1);
        while (b == a) b = Rand.Next(1, n + 1);
        int c = Rand.Next(1, n + 1);
        while (c == a || c == b) c = Rand.Next(1, n + 1);

        // 3 Дейкстры
        const long INF = long.MaxValue / 4;
        var adj = new List<(int, int)>[n + 1];
        for (int i = 1; i <= n; i++) adj[i] = new List<(int, int)>();
        foreach (var (u, v, w) in edgeList)
        {
            adj[u].Add((v, w));
            adj[v].Add((u, w));
        }

        long[] distA = Dijkstra(n, adj, a);
        long[] distB = Dijkstra(n, adj, b);
        long[] distC = Dijkstra(n, adj, c);

        long ans = INF;
        if (distA[b] != INF && distB[c] != INF) ans = Math.Min(ans, distA[b] + distB[c]);
        if (distA[c] != INF && distC[b] != INF) ans = Math.Min(ans, distA[c] + distC[b]);
        if (distB[a] != INF && distA[c] != INF) ans = Math.Min(ans, distB[a] + distA[c]);
        if (distB[c] != INF && distC[a] != INF) ans = Math.Min(ans, distB[c] + distC[a]);
        if (distC[a] != INF && distA[b] != INF) ans = Math.Min(ans, distC[a] + distA[b]);
        if (distC[b] != INF && distB[a] != INF) ans = Math.Min(ans, distC[b] + distB[a]);

        return (n, edgeList.Count, edgeList, a, b, c, ans == INF ? -1 : ans);
    }

    private static long[] Dijkstra(int n, List<(int, int)>[] adj, int start)
    {
        const long INF = long.MaxValue / 4;
        long[] dist = new long[n + 1];
        for (int i = 1; i <= n; i++) dist[i] = INF;
        dist[start] = 0;

        var pq = new PriorityQueue<int, long>();
        pq.Enqueue(start, 0);

        while (pq.Count > 0)
        {
            int u = pq.Dequeue();
            foreach (var (v, w) in adj[u])
            {
                if (dist[u] + w < dist[v])
                {
                    dist[v] = dist[u] + w;
                    pq.Enqueue(v, dist[v]);
                }
            }
        }
        return dist;
    }

    #endregion
}
