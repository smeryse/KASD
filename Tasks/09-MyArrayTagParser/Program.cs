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
            Console.WriteLine("input.txt file not found.");
            return;
        }

        Regex tagRegex = new Regex(@"<(/?)([A-Za-z][A-Za-z0-9]*)>", RegexOptions.Compiled); // </f13fvr5>

        MyArrayList<string> uniqueTags = new MyArrayList<string>();

        foreach (var line in File.ReadLines(inputPath))
        {
            foreach (Match m in tagRegex.Matches(line))
            {
                // Normalize tag to format <tag> in lowercase, without '/'
                string normalizedTag = $"<{m.Groups[2].Value.ToLower()}>";

                if (!uniqueTags.Contains(normalizedTag))
                {
                    uniqueTags.Add(normalizedTag);
                }
            }
        }

        using (var writer = new StreamWriter(outputPath))
        {
            for (int i = 0; i < uniqueTags.Size(); i++)
            {
                writer.WriteLine(uniqueTags.Get(i));
            }
        }

        Console.WriteLine($"Unique tags successfully written to {outputPath}");
    }
}
