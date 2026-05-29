using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task31
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            List<string> words;

            if (args.Length > 0 && File.Exists(args[0]))
            {
                words = File.ReadAllLines(args[0])
                    .Select(line => line.Trim())
                    .Where(w => w.Length > 0)
                    .Distinct()
                    .ToList();
            }
            else if (args.Length > 0)
            {
                words = args.Select(w => w.Trim()).Distinct().ToList();
            }
            else
            {
                words = new List<string>
                {
                    "hello", "world", "low", "old", "car", "arc",
                    "cat", "at", "bat", "tab"
                };
            }

            if (words.Count == 0)
            {
                Console.WriteLine("No words provided.");
                return;
            }

            var solver = new CrissCrossSolver(words);
            Console.WriteLine("Solving criss-cross puzzle with {0} words...", words.Count);

            if (solver.Solve())
            {
                Console.WriteLine("Solution found:");
                solver.PrintGrid();
            }
            else
            {
                Console.WriteLine("No solution exists for the given word list.");
            }
        }
    }
}
