using System;
using System.Collections.Generic;
using System.IO;

namespace CT5;

internal class Program
{
    private static readonly Dictionary<string, Action> TaskMap = new(StringComparer.OrdinalIgnoreCase)
    {
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

        RunWithOptionalCheck(run, key, args);
    }

    private static bool TryOpenSample(string key, out TextReader reader)
    {
        string samplesDir = Path.Combine(Directory.GetCurrentDirectory(), "Samples");
        string[] candidates =
        {
            Path.Combine(samplesDir, key + ".in")
        };

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



    private static void RunWithOptionalCheck(Action run, string key, string[] args)
    {
        if (!TryResolveExpectedPath(key, args, out var expectedPath))
        {
            run();
            return;
        }
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        run();
        Console.Out.Flush();
        Console.SetOut(originalOut);
        string actual = Normalize(writer.ToString());
        string expected = Normalize(File.ReadAllText(expectedPath));
        if (actual == expected)
        {
            Console.WriteLine("OK");
        }
        else
        {
            Console.WriteLine("FAIL");
            Console.WriteLine("Expected:");
            Console.WriteLine(expected);
            Console.WriteLine("Actual:");
            Console.WriteLine(actual);
            Environment.ExitCode = 1;
        }
    }
    private static bool TryResolveExpectedPath(string key, string[] args, out string expectedPath)
    {
        expectedPath = "";
        if (args.Length < 3) return false;
        var arg = args[2];
        if (arg.Equals("check", StringComparison.OrdinalIgnoreCase))
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Samples", key + ".out");
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл не найден: {path}. Сравнение отключено.");
                return false;
            }
            expectedPath = path;
            return true;
        }
        if (!File.Exists(arg))
        {
            Console.WriteLine($"Файл не найден: {arg}. Сравнение отключено.");
            return false;
        }
        expectedPath = arg;
        return true;
    }
    private static string Normalize(string value)
    {
        return value.Replace("\r\n", "\n").TrimEnd();
    }


    private static void PrintUsage()
    {
        Console.WriteLine("Укажи задачу, напр.: dotnet run -- A");
        Console.WriteLine("Примеры: dotnet run -- A sample | dotnet run -- A Samples/A.in");
        Console.WriteLine("Сравнение: dotnet run -- A sample check | dotnet run -- A Samples/A.in Samples/A.out");
    }
}
