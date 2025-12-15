using System;
using System.Collections.Generic;
using System.IO;
using CT1.Tasks;

namespace CT1
{
    class Program
    {
        private static readonly Dictionary<string, Action> TaskMap = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
        {
            ["A"] = SimpleSort.Solve,
            ["B"] = CountSort.Solve,
            ["C"] = CountInversions.Solve,
            ["D"] = MaxHeapTask.Solve,
            ["E"] = QuickSearchInArray.Solve,
            ["F"] = ApproximateBinarySearch.Solve,
            ["G"] = VeryEasyTask.Solve,
            ["H"] = SquareRootAndSquareSquare.Solve,
            ["I"] = GladeOfFirewood.Solve,
            ["J"] = KBest.Solve,
            ["K"] = SplitArray.Solve,
            ["L"] = CompTable.Solve,
            ["M"] = KSumm.Solve
        };

        // Минимальные примеры, чтобы можно было быстро запустить: dotnet run -- A sample
        private static readonly Dictionary<string, string> Samples = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["A"] = "5\n1 3 2 4 5\n",
            ["B"] = "5\n5 4 3 2 1\n",
            ["C"] = "5\n2 3 9 2 9\n",
            ["D"] = "5\n0 1\n0 10\n0 5\n1\n1\n",
            ["E"] = "5\n1 2 2 3 5\n3\n1 2\n2 4\n5 6\n",
            ["F"] = "5 3\n1 4 6 8 10\n2 7 9\n",
            ["G"] = "4 1 2\n",
            ["H"] = "2.0\n",
            ["I"] = "5 4\n0.500000\n",
            ["J"] = "3 2\n10 2\n8 1\n7 2\n",
            ["K"] = "5 2\n7 2 5 10 8\n",
            ["L"] = "3 5\n",
            ["M"] = "3 4\n1 7 11\n2 4 6\n",
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Укажи задачу A-M, напр.: dotnet run -- A или dotnet run -- A sample");
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


