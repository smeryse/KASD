using System;
using System.IO;
using Task21.Collections;

namespace Task22
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new MyHtmlTagParser();

            string inputPath;
            if (args.Length > 0)
            {
                inputPath = args[0];
            }
            else
            {
                inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Samples", "input.txt");
            }

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Error: File '{inputPath}' not found!");
                Console.WriteLine("Usage: dotnet run [path-to-input.txt]");
                return;
            }

            Console.WriteLine($"Parsing file: {inputPath}");
            Console.WriteLine();

            MyHashMap<string, int> tagCounts = parser.ParseFile(inputPath);

            parser.PrintTagCounts(tagCounts);
        }
    }
}
