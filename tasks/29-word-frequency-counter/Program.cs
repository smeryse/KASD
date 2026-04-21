using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Task18.Collection;

namespace Task29
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 29: Словарь с подсчётом частоты слов ===\n");

            string inputFile = args.Length > 0 ? args[0] : "input.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл '{inputFile}' не найден. Создаю тестовый файл...");
                CreateSampleInput(inputFile);
            }

            string text = File.ReadAllText(inputFile);
            Console.WriteLine($"Прочитано {text.Length} символов из файла\n");

            var wordFrequency = new MyTreeMap<string, int>(new CaseInsensitiveComparer());
            var words = ExtractWords(text);

            foreach (var word in words)
            {
                string normalizedWord = word.ToLower();
                if (wordFrequency.ContainsKey(normalizedWord))
                {
                    int currentCount = wordFrequency.Get(normalizedWord);
                    wordFrequency.Put(normalizedWord, currentCount + 1);
                }
                else
                {
                    wordFrequency.Put(normalizedWord, 1);
                }
            }

            Console.WriteLine($"Всего уникальных слов: {wordFrequency.Size}");
            Console.WriteLine($"\nТоп-10 самых частых слов:");

            var sortedByFrequency = SortByFrequency(wordFrequency);
            int count = 0;
            foreach (var item in sortedByFrequency)
            {
                if (count >= 10) break;
                Console.WriteLine($"  {item.Item1}: {item.Item2} раз(а)");
                count++;
            }

            Console.WriteLine($"\nВсе слова (по алфавиту):");
            count = 0;
            foreach (var key in wordFrequency.KeySet())
            {
                Console.Write($"{key}({wordFrequency.Get(key)})  ");
                count++;
                if (count % 10 == 0) Console.WriteLine();
            }
            if (count % 10 != 0) Console.WriteLine();

            Console.WriteLine("\n=== Готово ===");
        }

        static string[] ExtractWords(string text)
        {
            return Regex.Matches(text, "[a-zA-Z]+")
                        .Select(m => m.Value)
                        .ToArray();
        }

        static List<(string Key, int Value)> SortByFrequency(MyTreeMap<string, int> map)
        {
            var list = new List<(string, int)>();
            foreach (var key in map.KeySet())
            {
                list.Add((key, map.Get(key)));
            }

            list.Sort((a, b) =>
            {
                int cmp = b.Item2.CompareTo(a.Item2);
                if (cmp != 0) return cmp;
                return string.Compare(a.Item1, b.Item1, StringComparison.OrdinalIgnoreCase);
            });

            return list;
        }

        static void CreateSampleInput(string path)
        {
            var text = @"The quick brown fox jumps over the lazy dog.
The dog was not amused by the fox.
The fox was quick and the dog was lazy.
A quick brown fox is a quick brown fox.";
            File.WriteAllText(path, text);
        }
    }

    class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }
}
