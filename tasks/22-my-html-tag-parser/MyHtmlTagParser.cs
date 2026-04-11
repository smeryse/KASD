using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Task21.Collections;

namespace Task22
{
    public class MyHtmlTagParser
    {
        // Регулярное выражение для извлечения тегов:
        // < - начинается с <
        // /? - может быть '/' (закрывающий тег)
        // [a-zA-Z] - первый символ обязательно буква
        // [a-zA-Z0-9]* - остальные символы буквы или цифры
        // [^>]* - могут быть атрибуты (любые символы кроме '>')
        // > - заканчивается >
        private static readonly Regex TagRegex = new Regex(
            @"</?([a-zA-Z][a-zA-Z0-9]*)[^>]*>",
            RegexOptions.Compiled
        );

        public MyHashMap<string, int> ParseFile(string filePath)
        {
            var tagCounts = new MyHashMap<string, int>();

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                MatchCollection matches = TagRegex.Matches(line);

                foreach (Match match in matches)
                {
                    string tagName = match.Groups[1].Value;

                    string normalizedTag = tagName.ToLowerInvariant();

                    if (tagCounts.ContainsKey(normalizedTag))
                    {
                        int currentCount = tagCounts.Get(normalizedTag)!;
                        tagCounts.Put(normalizedTag, currentCount + 1);
                    }
                    else
                    {
                        tagCounts.Put(normalizedTag, 1);
                    }
                }
            }

            return tagCounts;
        }

        public void PrintTagCounts(MyHashMap<string, int> tagCounts)
        {
            Console.WriteLine("=== HTML Tag Counts ===");
            Console.WriteLine($"Total unique tags: {tagCounts.Size}");
            Console.WriteLine();

            var entries = tagCounts.EntrySet();
            entries.Sort((a, b) => a.Key.CompareTo(b.Key));

            foreach (var pair in entries)
            {
                Console.WriteLine($"  <{pair.Key}> : {pair.Value}");
            }
        }
    }
}
