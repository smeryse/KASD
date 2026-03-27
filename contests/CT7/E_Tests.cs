using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT7.Tasks;

/// <summary>
/// Генератор тестов и проверка решения для задачи E (Компоненты вершинной двусвязности)
/// </summary>
internal class E_TestRunner
{
    private static readonly Random Rand = new Random(42);

    public static void RunAllTests()
    {
        int passed = 0;
        int failed = 0;

        Console.WriteLine("Запуск тестов для задачи E (Vertex Biconnected Components)");
        Console.WriteLine(new string('=', 60));

        // Тесты 1-10: Базовые случаи
        for (int i = 1; i <= 10; i++)
        {
            if (RunTest(i, GenerateBasicTest(i)))
                passed++;
            else
                failed++;
        }

        // Тесты 11-15: Кратные рёбра
        for (int i = 11; i <= 15; i++)
        {
            if (RunTest(i, GenerateMultiEdgeTest(i)))
                passed++;
            else
                failed++;
        }

        // Тесты 16-35: Случайные графы малой плотности
        for (int i = 16; i <= 35; i++)
        {
            if (RunTest(i, GenerateRandomTest(10 + (i % 20), 15 + (i % 30))))
                passed++;
            else
                failed++;
        }

        // Тесты 36-55: Случайные графы средней плотности
        for (int i = 36; i <= 55; i++)
        {
            if (RunTest(i, GenerateRandomTest(50 + (i % 50), 100 + (i % 100))))
                passed++;
            else
                failed++;
        }

        // Тесты 56-75: Графы с несколькими компонентами
        for (int i = 56; i <= 75; i++)
        {
            if (RunTest(i, GenerateMultiComponentTest(20 + (i % 30), 2 + (i % 4))))
                passed++;
            else
                failed++;
        }

        // Тесты 76-85: Цепочки и циклы
        for (int i = 76; i <= 85; i++)
        {
            if (RunTest(i, GenerateChainOrCycleTest(10 + (i % 20), i % 2 == 0)))
                passed++;
            else
                failed++;
        }

        // Тесты 86-95: Деревья (каждое ребро - отдельная компонента)
        for (int i = 86; i <= 95; i++)
        {
            if (RunTest(i, GenerateTreeTest(5 + (i % 15))))
                passed++;
            else
                failed++;
        }

        // Тесты 96-105: Большие графы
        for (int i = 96; i <= 105; i++)
        {
            if (RunTest(i, GenerateRandomTest(100 + (i % 100), 200 + (i % 300))))
                passed++;
            else
                failed++;
        }

        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"Результаты: {passed} пройдено, {failed} провалено из 105");

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

    private static bool RunTest(int testNum, (int n, int m, List<(int u, int v)> edges, string expectedOutput, bool isVerifiable) testData)
    {
        var (n, m, edges, expectedOutput, isVerifiable) = testData;

        try
        {
            string input = FormatInput(n, m, edges);
            string actualOutput = RunSolution(input);

            // Если ожидаемый ответ известен — проверяем точное совпадение
            if (isVerifiable && expectedOutput != null)
            {
                if (Normalize(actualOutput) == Normalize(expectedOutput))
                {
                    Console.WriteLine($"Тест {testNum,3}: OK (n={n}, m={m})");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Тест {testNum,3}: FAIL");
                    Console.WriteLine($"  Вход: n={n}, m={m}");
                    Console.WriteLine($"  Ожидалось: {Truncate(expectedOutput, 80)}");
                    Console.WriteLine($"  Получено:   {Truncate(actualOutput, 80)}");
                    return false;
                }
            }
            else
            {
                // Проверяем только корректность формата вывода
                if (ValidateOutputFormat(actualOutput, m))
                {
                    Console.WriteLine($"Тест {testNum,3}: OK (n={n}, m={m})");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Тест {testNum,3}: FAIL (неверный формат вывода)");
                    Console.WriteLine($"  Вход: n={n}, m={m}");
                    Console.WriteLine($"  Получено: {Truncate(actualOutput, 80)}");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тест {testNum,3}: ERROR - {ex.Message}");
            return false;
        }
    }

    private static bool ValidateOutputFormat(string output, int expectedEdges)
    {
        var lines = output.Split('\n');
        if (lines.Length < 2)
            return false;

        if (!int.TryParse(lines[0].Trim(), out int k) || k <= 0)
            return false;

        var components = lines[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (components.Length != expectedEdges)
            return false;

        foreach (var c in components)
        {
            if (!int.TryParse(c, out int comp) || comp < 1 || comp > k)
                return false;
        }

        return true;
    }

    private static string FormatInput(int n, int m, List<(int u, int v)> edges)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{n} {m}");
        foreach (var (u, v) in edges)
        {
            sb.AppendLine($"{u} {v}");
        }
        return sb.ToString();
    }

    private static string RunSolution(string input)
    {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        using var reader = new StringReader(input);
        using var writer = new StringWriter();

        Console.SetIn(reader);
        Console.SetOut(writer);

        try
        {
            VertexBiconnectedComponents.Solve();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }

    private static string Normalize(string s)
    {
        return s.Replace("\r\n", "\n").TrimEnd();
    }

    private static string Truncate(string s, int maxLen)
    {
        if (s.Length <= maxLen) return s;
        return s.Substring(0, maxLen) + "...";
    }

    #region Генераторы тестов

    /// <summary>
    /// Базовые тесты (простые случаи)
    /// </summary>
    private static (int n, int m, List<(int, int)>, string expected, bool isVerifiable) GenerateBasicTest(int testNum)
    {
        switch (testNum)
        {
            case 1: // Одна вершина, нет рёбер
                return (1, 0, new List<(int, int)>(), "0\n", true);

            case 2: // Две вершины, одно ребро
                return (2, 1, new List<(int, int)> { (1, 2) }, "1\n1", true);

            case 3: // Три вершины в цикле
                return (3, 3, new List<(int, int)> { (1, 2), (2, 3), (3, 1) }, "1\n1 1 1", true);

            case 4: // Четыре вершины в цикле
                return (4, 4, new List<(int, int)> { (1, 2), (2, 3), (3, 4), (4, 1) }, "1\n1 1 1 1", true);

            case 5: // Два треугольника с общей вершиной
                return (5, 6, new List<(int, int)> { (1, 2), (2, 3), (3, 1), (1, 4), (4, 5), (5, 1) }, "2\n1 1 1 2 2 2", true);

            case 6: // Полный граф K4
                return (4, 6, new List<(int, int)> { (1, 2), (1, 3), (1, 4), (2, 3), (2, 4), (3, 4) }, "1\n1 1 1 1 1 1", true);

            case 7: // Мост между двумя циклами
                return (6, 7, new List<(int, int)> { (1, 2), (2, 3), (3, 1), (3, 4), (4, 5), (5, 6), (6, 4) }, "3\n3 3 3 2 1 1 1", true);

            case 8: // Звезда (все рёбра инцидентны вершине 1)
                return (5, 4, new List<(int, int)> { (1, 2), (1, 3), (1, 4), (1, 5) }, "4\n1 2 3 4", true);

            case 9: // Путь из 4 вершин
                return (4, 3, new List<(int, int)> { (1, 2), (2, 3), (3, 4) }, "3\n3 2 1", true);

            case 10: // Два несвязанных цикла
                return (6, 6, new List<(int, int)> { (1, 2), (2, 3), (3, 1), (4, 5), (5, 6), (6, 4) }, "2\n1 1 1 2 2 2", true);

            default:
                return GenerateBasicTest(1);
        }
    }

    /// <summary>
    /// Случайный граф (проверка формата вывода)
    /// </summary>
    private static (int n, int m, List<(int, int)>, string expected, bool isVerifiable) GenerateRandomTest(int n, int m)
    {
        var edges = new HashSet<(int, int)>();
        var edgeList = new List<(int, int)>();

        while (edgeList.Count < m && edges.Count < n * (n - 1) / 2)
        {
            int u = Rand.Next(1, n + 1);
            int v = Rand.Next(1, n + 1);
            if (u != v)
            {
                var edge = u < v ? (u, v) : (v, u);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                    edgeList.Add((edge.Item1, edge.Item2));
                }
            }
        }

        return (n, edgeList.Count, edgeList, null, false);
    }

    /// <summary>
    /// Граф с кратными рёбрами
    /// </summary>
    private static (int n, int m, List<(int, int)>, string expected, bool isVerifiable) GenerateMultiEdgeTest(int testNum)
    {
        switch (testNum)
        {
            case 11: // Два ребра между 1 и 2
                return (2, 2, new List<(int, int)> { (1, 2), (1, 2) }, "1\n1 1", true);

            case 12: // Три ребра между 1 и 2
                return (2, 3, new List<(int, int)> { (1, 2), (1, 2), (1, 2) }, "1\n1 1 1", true);

            case 13: // Треугольник с кратным ребром
                return (3, 4, new List<(int, int)> { (1, 2), (1, 2), (2, 3), (3, 1) }, "1\n1 1 1 1", true);

            case 14: // Мост с кратными рёбрами в компонентах
                return (4, 5, new List<(int, int)> { (1, 2), (1, 2), (2, 3), (3, 4), (3, 4) }, "3\n3 3 2 1 1", true);

            case 15: // Две вершины с 5 рёбрами
                return (2, 5, new List<(int, int)> { (1, 2), (1, 2), (1, 2), (1, 2), (1, 2) }, "1\n1 1 1 1 1", true);

            default:
                return GenerateMultiEdgeTest(11);
        }
    }

    /// <summary>
    /// Граф с несколькими компонентами двусвязности
    /// </summary>
    private static (int n, int m, List<(int, int)>, string expected, bool isVerifiable) GenerateMultiComponentTest(int n, int numComponents)
    {
        var edges = new List<(int, int)>();
        int verticesPerComponent = n / numComponents;

        for (int c = 0; c < numComponents; c++)
        {
            int start = c * verticesPerComponent + 1;
            int end = Math.Min((c + 1) * verticesPerComponent, n);

            // Создаём цикл внутри компоненты
            for (int i = start; i < end; i++)
            {
                edges.Add((i, i + 1));
            }
            if (end > start)
            {
                edges.Add((end, start));
            }

            // Соединяем компоненты мостом
            if (c > 0 && end < n)
            {
                edges.Add((end, end + 1));
            }
        }

        return (n, edges.Count, edges, null, false);
    }

    /// <summary>
    /// Цепочка или цикл
    /// </summary>
    private static (int n, int m, List<(int, int)>, string expected, bool isVerifiable) GenerateChainOrCycleTest(int n, bool isCycle)
    {
        var edges = new List<(int, int)>();

        for (int i = 1; i < n; i++)
        {
            edges.Add((i, i + 1));
        }

        if (isCycle)
        {
            edges.Add((n, 1));
        }

        return (n, edges.Count, edges, null, false);
    }

    /// <summary>
    /// Дерево (каждое ребро - отдельная компонента)
    /// </summary>
    private static (int n, int m, List<(int, int)>, string expected, bool isVerifiable) GenerateTreeTest(int n)
    {
        var edges = new List<(int, int)>();

        for (int i = 2; i <= n; i++)
        {
            int parent = Rand.Next(1, i);
            edges.Add((parent, i));
        }

        return (n, edges.Count, edges, null, false);
    }

    #endregion
}
