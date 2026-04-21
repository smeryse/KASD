using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Task18.Collection;
using Task25;

namespace Task30
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 30: Анализ текстов (группировка по длине слов) ===\n");

            string inputFile = args.Length > 0 ? args[0] : "input.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл '{inputFile}' не найден. Создаю тестовый файл...");
                CreateSampleInput(inputFile);
            }

            string text = File.ReadAllText(inputFile);
            Console.WriteLine($"Прочитано {text.Length} символов из файла\n");

            var wordsByLength = new MyTreeMap<int, MyHashSet<string>>();
            var allUniqueWords = new MyHashSet<string>(new CaseInsensitiveComparer());
            string[] words = ExtractWords(text);

            Console.WriteLine($"Всего слов в тексте: {words.Length}\n");

            foreach (var word in words)
            {
                string normalizedWord = word.ToLower();
                allUniqueWords.Add(normalizedWord);
                int len = normalizedWord.Length;

                if (!wordsByLength.ContainsKey(len))
                {
                    wordsByLength.Put(len, new MyHashSet<string>());
                }

                wordsByLength.Get(len).Add(normalizedWord);
            }

            Console.WriteLine($"Уникальных слов: {allUniqueWords.Size()}");
            int[] allLengths = wordsByLength.KeySet().ToArray();
            Console.WriteLine($"Диапазон длин: от {allLengths[0]} до {allLengths[allLengths.Length - 1]} символов\n");

            Console.WriteLine("Группировка по длине:");
            foreach (var len in allLengths)
            {
                MyHashSet<string> set = wordsByLength.Get(len);
                Console.Write($"  {len} симв: ");
                bool first = true;
                foreach (var w in set.ToArray())
                {
                    if (!first) Console.Write(", ");
                    Console.Write(w);
                    first = false;
                }
                Console.WriteLine($" ({set.Size()} шт.)");
            }

            Console.WriteLine($"\nСтатистика:");
            int totalUnique = allUniqueWords.Size();
            int avgLen = 0;
            object[] wordArr = allUniqueWords.ToArray();
            foreach (var wordObj in wordArr)
            {
                avgLen += ((string)wordObj).Length;
            }
            if (totalUnique > 0)
                avgLen /= totalUnique;

            Console.WriteLine($"  Средняя длина слова: {avgLen} симв.");
            Console.WriteLine($"  Уникальных слов: {totalUnique}");
            Console.WriteLine($"  Всего слов: {words.Length}");

            Console.WriteLine("\n=== Готово ===");
        }

        static string[] ExtractWords(string text)
        {
            return Regex.Matches(text, "[a-zA-Z]+")
                        .Select(m => m.Value)
                        .ToArray();
        }

        static void CreateSampleInput(string path)
        {
            var text = @"A quick brown fox jumps over the lazy dog.
The dog was not amused by the fox.
The fox was quick and the dog was lazy.
Programming is fun and interesting.";
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
