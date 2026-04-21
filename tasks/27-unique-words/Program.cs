using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Task25;

namespace Task27
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 27: Уникальные слова из файла (без учёта регистра) ===\n");

            string inputFile = args.Length > 0 ? args[0] : "input.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл '{inputFile}' не найден!");
                Console.WriteLine("Создаю тестовый файл...");
                CreateSampleInput(inputFile);
                Console.WriteLine($"Файл '{inputFile}' создан.\n");
            }

            string[] lines;
            try
            {
                lines = File.ReadAllLines(inputFile);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return;
            }

            if (lines.Length == 0)
            {
                Console.WriteLine("Файл пустой!");
                Console.WriteLine("\n=== Готово ===");
                return;
            }

            Console.WriteLine($"Прочитано строк: {lines.Length}");
            Console.WriteLine($"\nСодержимое файла:");
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"  [{i + 1}] \"{lines[i]}\"");
            }

            var uniqueWords = new MyHashSet<string>(new CaseInsensitiveComparer());
            int totalWords = 0;
            int duplicateWords = 0;

            foreach (var line in lines)
            {
                var words = ExtractWords(line);
                totalWords += words.Length;

                foreach (var word in words)
                {
                    string normalizedWord = word.ToLower();
                    bool added = uniqueWords.Add(normalizedWord);
                    if (!added)
                        duplicateWords++;
                }
            }

            Console.WriteLine($"\nСтатистика:");
            Console.WriteLine($"  Всего слов: {totalWords}");
            Console.WriteLine($"  Дубликатов: {duplicateWords}");
            Console.WriteLine($"  Уникальных слов: {uniqueWords.Size()}");

            Console.WriteLine($"\nУникальные слова ({uniqueWords.Size()} шт.):");
            
            int count = 0;
            foreach (var word in uniqueWords.ToArray())
            {
                Console.Write($"{word}");
                count++;
                if (count % 10 == 0)
                    Console.WriteLine();
                else
                    Console.Write(", ");
            }
            if (count % 10 != 0)
                Console.WriteLine();

            Console.WriteLine("\nГотово");
        }

        static string[] ExtractWords(string line)
        {
            return Regex.Matches(line, "[a-zA-Z]+")
                        .Select(m => m.Value)
                        .ToArray();
        }

        static void CreateSampleInput(string path)
        {
            var lines = new[]
            {
                "Hello world! Hello Universe.",
                "The quick brown fox jumps over the lazy dog.",
                "Hello again, World!",
                "Programming is fun and interesting.",
                "The fox was quick and the dog was lazy.",
                "ABC abc AbC aBc - all the same word!"
            };
            File.WriteAllLines(path, lines);
        }
    }

    class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }
}
