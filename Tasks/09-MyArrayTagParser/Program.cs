using System;
using System.IO;
using System.Text.RegularExpressions;
using Task8.Collections;

class Program
{
    static void Main()
    {
        string inputPath = "input.txt";
        string outputPath = "output.txt";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл input.txt не найден.");
            return;
        }

        Regex tagRegex = new Regex(@"<(/?)([A-Za-z][A-Za-z0-9]*)>", RegexOptions.Compiled);

        MyArrayList<string> uniqueTags = new MyArrayList<string>();

        foreach (var line in File.ReadLines(inputPath))
        {
            foreach (Match m in tagRegex.Matches(line))
            {
                string normalizedTag = $"<{m.Groups[2].Value.ToLower()}>";

                bool exists = false;
                for (int i = 0; i < uniqueTags.Size(); i++)
                {
                    if (uniqueTags.Get(i) == normalizedTag)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    uniqueTags.Add(normalizedTag);
            }
        }

        // Запись уникальных тегов в файл
        using (var writer = new StreamWriter(outputPath))
        {
            for (int i = 0; i < uniqueTags.Size(); i++)
            {
                writer.WriteLine(uniqueTags.Get(i));
            }
        }

        Console.WriteLine($"Уникальные теги успешно записаны в {outputPath}");
    }
}
