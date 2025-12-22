using System;
using System.Collections.Generic;
using System.IO;
using CT3.Tasks;

namespace CT3;

class Program
{
    private static readonly Dictionary<string, Action> TaskMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["A"] = GrasshopperCollect.Solve,
        ["B"] = TurtleAndCoins.Solve,
        ["B-G"] = TurtleAndCoinsGreedy.Solve,
        ["C"] = LargestSubseq.Solve,
        ["D"] = KnightMove.Solve,
        ["E"] = LevenshteinDist.Solve,
        ["F"] = CafeTask.Solve,
        ["G"] = DeleteStaples.Solve,
        ["H"] = AquariumSeller.Solve,
        ["I"] = ReplaceDomino.Solve,
        ["J"] = CutePatterns.Solve,
        ["K"] = Multybacpack.Solve
    };

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        var key = args[0];
        if (!TaskMap.TryGetValue(key, out var run))
        {
            Console.WriteLine($"Неизвестная задача '{key}'. Доступные: {string.Join(", ", TaskMap.Keys)}");
            return;
        }

        if (args.Length > 1 && !TrySetInputFromArgument(key, args[1]))
        {
            Console.WriteLine($"Не нашел входной файл для '{key}' по аргументу '{args[1]}'. Использую стандартный ввод.");
        }

        run();
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Укажи задачу A-K, напр.: dotnet run -- A");
        Console.WriteLine("Для файлов с тестами:");
        Console.WriteLine("  dotnet run -- A sample   (Contests/CT3/Tests/A.txt)");
        Console.WriteLine("  dotnet run -- A <path>   (любой путь к файлу)");
        Console.WriteLine("  dotnet run -- A <name>   (Contests/CT3/Tests/A-<name>.txt)");
        Console.WriteLine("Дополнительно: B-G (greedy версия задачи B).");
    }

    private static bool TrySetInputFromArgument(string taskKey, string inputArg)
    {
        if (string.IsNullOrWhiteSpace(inputArg))
            return false;

        if (File.Exists(inputArg))
        {
            Console.SetIn(new StreamReader(inputArg));
            return true;
        }

        var testsDir = GetTestsDirectory();
        var samplePath = Path.Combine(testsDir, $"{taskKey}.txt");
        if (IsSampleArg(inputArg) && File.Exists(samplePath))
        {
            Console.SetIn(new StreamReader(samplePath));
            return true;
        }

        var namedPath = Path.Combine(testsDir, $"{taskKey}-{inputArg}.txt");
        if (File.Exists(namedPath))
        {
            Console.SetIn(new StreamReader(namedPath));
            return true;
        }

        return false;
    }

    private static bool IsSampleArg(string inputArg) =>
        inputArg.Equals("sample", StringComparison.OrdinalIgnoreCase) ||
        inputArg.Equals("test", StringComparison.OrdinalIgnoreCase);

    private static string GetTestsDirectory()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        return Path.Combine(projectRoot, "Tests");
    }
}
