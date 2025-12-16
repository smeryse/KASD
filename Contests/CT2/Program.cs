using System;
using System.Collections.Generic;
using System.IO;
using CT2.Tasks;

namespace CT2
{
    class Program
    {
        private static readonly Dictionary<string, Action> TaskMap = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
        {
            ["A"] = MinOnStack.Solve,
            ["B"] = Balloons.Solve,
            ["C"] = Astrograd.Solve,
            ["D"] = GoblinsAndShamans.Solve,
            ["E"] = PostfixEntry.Solve,
            ["F"] = StackSort.Solve,
            ["G"] = SystemDisjointSets.Solve,
            ["H"] = CalcExp.Solve,
            ["I"] = Cuckoo.Solve
        };

        // Минимальные примеры, чтобы можно было быстро запустить: dotnet run -- A sample
        private static readonly Dictionary<string, string> Samples = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["A"] = "8\n1 2\n1 0\n1 3\n3\n2\n3\n2\n3\n",
            ["B"] = "5 1 2 2 2 1\n",
            ["C"] = "6\n1 100\n1 200\n1 300\n5\n2\n4 200\n",
            ["D"] = "5\n+ 1\n+ 2\n* 3\n-\n-\n",
            ["E"] = "2 3 + 4 *\n",
            ["F"] = "3\n1 2 3\n",
            ["G"] = "5\nunion 1 2\nunion 3 4\nget 1\nget 3\n",
            ["H"] = "3 5\njoin 1 2\nadd 1 10\nadd 2 5\nget 1\nget 2\n",
            ["I"] = "3 1 3\n1 2\n1 1 2\n2 1 2\n3\n",
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Укажи задачу A-I, напр.: dotnet run -- A или dotnet run -- A sample");
                return;
            }

            var key = args[0];
            if (!TaskMap.TryGetValue(key, out var run))
            {
                Console.WriteLine($"Неизвестная задача '{key}'. Доступные: {string.Join(", ", TaskMap.Keys)}");
                return;
            }

            // Если указан second аргумент "sample" — подставляем встроенный пример входа
            if (args.Length > 1 && args[1].Equals("sample", StringComparison.OrdinalIgnoreCase))
            {
                if (Samples.TryGetValue(key, out var sample))
                    Console.SetIn(new StringReader(sample));
                else
                    Console.WriteLine("Для этой задачи нет встроенного примера, использую стандартный ввод.");
            }

            run();
        }
    }
}

