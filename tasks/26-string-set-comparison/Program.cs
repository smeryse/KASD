using Task25;

namespace Task26
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 26: Множество строк с сравнением по длине слов ===\n");

            string inputFile = "input.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл {inputFile} не найден. Создаю тестовый файл...");
                CreateSampleInput(inputFile);
            }

            var strings = File.ReadAllLines(inputFile);
            Console.WriteLine($"Прочитано строк из файла: {strings.Length}");
            Console.WriteLine($"Содержимое файла:");
            foreach (var line in strings)
            {
                Console.WriteLine($"  \"{line}\"");
            }

            var set = new MyHashSet<string>(new StringWordLengthComparer());
            set.AddAll(strings);

            Console.WriteLine($"\nМножество (размер: {set.Size()}):");
            Console.WriteLine(set);

            Console.WriteLine("\n=== Демонстрация сравнения ===");
            string[] testStrings = {
                "hello world",
                "hi",
                "a b c",
                "test",
                "one two three"
            };

            Console.WriteLine("Сортировка строк по длине слов:");
            Array.Sort(testStrings, new StringWordLengthComparer());
            foreach (var s in testStrings)
            {
                var words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var sortedWords = words.OrderBy(w => w.Length).ToArray();
                Console.WriteLine($"  \"{s}\" -> короткие слова: [{string.Join(", ", sortedWords)}]");
            }

            Console.WriteLine("\n--- Пошаговое сравнение двух строк ---");
            string str1 = "cat dog elephant";
            string str2 = "ant bee";
            
            Console.WriteLine($"\nСравниваем: \"{str1}\" и \"{str2}\"");
            
            var words1 = GetSortedWords(str1);
            var words2 = GetSortedWords(str2);
            
            Console.WriteLine($"\nСтрока 1: \"{str1}\"");
            Console.WriteLine($"  Слова: [{string.Join(", ", words1)}]");
            Console.WriteLine($"  Длины слов (отсортированы): [{string.Join(", ", words1.Select(w => w.Length))}]");
            
            Console.WriteLine($"\nСтрока 2: \"{str2}\"");
            Console.WriteLine($"  Слова: [{string.Join(", ", words2)}]");
            Console.WriteLine($"  Длины слов (отсортированы): [{string.Join(", ", words2.Select(w => w.Length))}]");
            
            int cmp = new StringWordLengthComparer().Compare(str1, str2);
            Console.WriteLine($"\nРезультат сравнения: {cmp}");
            if (cmp < 0)
                Console.WriteLine($"  \"{str1}\" < \"{str2}\" (первая строка МЕНЬШЕ)");
            else if (cmp > 0)
                Console.WriteLine($"  \"{str1}\" > \"{str2}\" (первая строка БОЛЬШЕ)");
            else
                Console.WriteLine($"  \"{str1}\" == \"{str2}\" (строки РАВНЫ)");
            
            Console.WriteLine("\n--- Ещё пример ---");
            str1 = "a bb ccc";
            str2 = "d ee fff";
            
            Console.WriteLine($"\nСравниваем: \"{str1}\" и \"{str2}\"");
            words1 = GetSortedWords(str1);
            words2 = GetSortedWords(str2);
            
            Console.WriteLine($"Строка 1: слова [{string.Join(", ", words1)}], длины [{string.Join(", ", words1.Select(w => w.Length))}]");
            Console.WriteLine($"Строка 2: слова [{string.Join(", ", words2)}], длины [{string.Join(", ", words2.Select(w => w.Length))}]");
            
            cmp = new StringWordLengthComparer().Compare(str1, str2);
            Console.WriteLine($"Результат: {cmp} -> {(cmp < 0 ? "строкa 1 < строка 2" : cmp > 0 ? "строка 1 > строка 2" : "строка 1 == строка 2")}");

            Console.WriteLine("\n=== Готово ===");
        }

        static void CreateSampleInput(string path)
        {
            var lines = new[]
            {
                "hello world this is a test",
                "hi there",
                "a b c d e",
                "programming algorithms data structures",
                "one two three four five six",
                "cat dog bird",
                "I am learning C sharp"
            };
            File.WriteAllLines(path, lines);
        }

        static string[] GetSortedWords(string s)
        {
            var words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.OrderBy(w => w.Length).ToArray();
        }
    }

    class StringWordLengthComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var wordsX = GetSortedWords(x);
            var wordsY = GetSortedWords(y);

            int len = Math.Min(wordsX.Length, wordsY.Length);
            for (int i = 0; i < len; i++)
            {
                int cmp = wordsX[i].Length.CompareTo(wordsY[i].Length);
                if (cmp != 0) return cmp;
            }

            return wordsX.Length.CompareTo(wordsY.Length);
        }

        static string[] GetSortedWords(string s)
        {
            var words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.OrderBy(w => w.Length).ToArray();
        }
    }
}
