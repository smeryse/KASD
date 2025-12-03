using System;
using System.IO;
using System.Text.RegularExpressions;
using MyArrayList;

namespace 09_MyArrayTagParser
{
    class Program
    {
        #region Main
        // Program entry point: parse input file and print unique tags
        static void Main()
        {
            MyArrayList<string> tags = new MyArrayList<string>();
            string path = "input.txt";
            Regex tagRegex = new Regex(@"<(/?)([A-Za-z][A-Za-z0-9]*)>", RegexOptions.Compiled);

            if (!File.Exists(path))
            {
                Console.WriteLine("Файл input.txt не найден.");
                return;
            }

            // Read file and collect tags
            foreach (var line in File.ReadLines(path))
            {
                foreach (Match m in tagRegex.Matches(line))
                {
                    tags.Add(m.Value);
                }
            }

            // Remove duplicates (ignore '/' and case)
            MyArrayList<string> uniqueTags = new MyArrayList<string>();
            for (int i = 0; i < tags.Size(); i++)
            {
                string tag = tags.Get(i);
                string normalized = NormalizeTag(tag);

                bool exists = false;
                for (int j = 0; j < uniqueTags.Size(); j++)
                {
                    if (NormalizeTag(uniqueTags.Get(j)) == normalized)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    uniqueTags.Add(tag);
            }

            // Output unique tags
            for (int i = 0; i < uniqueTags.Size(); i++)
            {
                Console.WriteLine(uniqueTags.Get(i));
            }
        }
        #endregion

        #region Helpers
        // Normalize tag by removing '/' and converting to lower-case
        static string NormalizeTag(string tag)
        {
            var match = Regex.Match(tag, @"<(/?)([A-Za-z][A-Za-z0-9]*)>");
            if (match.Success)
            {
                return match.Groups[2].Value.ToLower();
            }
            return tag.ToLower();
        }
        #endregion
    }
}
