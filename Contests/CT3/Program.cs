using System;
using System.Collections.Generic;
using System.IO;
using CT3.Tasks;

namespace CT3;

internal class Program
{
    private static readonly Dictionary<string, Action> TaskMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["A"] = GrasshopperCollect.Solve,
        ["B"] = TurtleAndCoins.Solve,
        ["B-Greedy"] = TurtleAndCoinsGreedy.Solve,
        ["C"] = LargestSubseq.Solve,
        ["D"] = KnightMove.Solve,
        ["E"] = LevenshteinDist.Solve,
        ["F"] = CafeTask.Solve,
        ["G"] = DeleteStaples.Solve,
        ["H"] = AquariumSeller.Solve,
        ["I"] = ReplaceDomino.Solve,
        ["J"] = CutePatterns.Solve,
        ["K"] = MultiBackpack.Solve,
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

        if (args.Length > 1)
        {
            var inputArg = args[1];
            if (inputArg.Equals("sample", StringComparison.OrdinalIgnoreCase))
            {
                if (TryOpenSample(key, out var reader))
                    Console.SetIn(reader);
                else
                    Console.WriteLine("Для этой задачи нет файла с примером, использую стандартный ввод.");
            }
            else if (File.Exists(inputArg))
            {
                Console.SetIn(new StreamReader(inputArg));
            }
            else
            {
                Console.WriteLine($"Файл не найден: {inputArg}. Использую стандартный ввод.");
            }
        }

        run();
    }

    private static bool TryOpenSample(string key, out TextReader reader)
    {
        string samplesDir = Path.Combine(Directory.GetCurrentDirectory(), "Samples");
        string[] candidates =
        [
            Path.Combine(samplesDir, $"{key}.in"),
        ];

        foreach (var path in candidates)
        {
            if (File.Exists(path))
            {
                reader = new StreamReader(path);
                return true;
            }
        }

        reader = TextReader.Null;
        return false;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Укажи задачу A-K, напр.: dotnet run -- A");
        Console.WriteLine("Примеры: dotnet run -- A sample | dotnet run -- A Samples/A.in");
        Console.WriteLine("Дополнительно: B-Greedy");
    }
}
