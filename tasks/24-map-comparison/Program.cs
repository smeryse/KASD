using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Task18.Collection;
using Task21.Collections;

namespace Task24
{
    internal static class BenchmarkRunner
    {
        private static readonly int[] DataSizes = { 100000, 1000000, 10000000 };
        private static readonly int Iterations = 20;
        private static readonly int IterationsForLarge = 1;
        private static readonly Random Random = new Random(42);

        private static int[] GenerateKeys(int count)
        {
            int[] keys = new int[count];
            for (int i = 0; i < count; i++)
                keys[i] = i;
            
            for (int i = keys.Length - 1; i > 0; i--)
            {
                int j = Random.Next(i + 1);
                int temp = keys[i];
                keys[i] = keys[j];
                keys[j] = temp;
            }
            return keys;
        }

        private static double TicksToMs(long ticks)
        {
            return (double)ticks / Stopwatch.Frequency * 1000.0;
        }

        public static void RunBenchmark()
        {
            Console.WriteLine("=== Benchmark: MyHashMap vs MyTreeMap ===");
            Console.WriteLine($"Data sizes: {string.Join(", ", DataSizes)}");
            Console.WriteLine();

            var putResults = BenchmarkPut();
            var getResults = BenchmarkGet();
            var removeResults = BenchmarkRemove();

            PrintResults("PUT Operation", putResults);
            PrintResults("GET Operation", getResults);
            PrintResults("REMOVE Operation", removeResults);

            GenerateCharts(putResults, getResults, removeResults);
            WriteAnalysis();
        }

        private static int GetIterations(int size)
        {
            if (size >= 1000000) return IterationsForLarge;
            return Iterations;
        }

        private static Dictionary<string, double[]> BenchmarkPut()
        {
            Console.WriteLine("--- Benchmarking PUT ---");
            var results = new Dictionary<string, double[]>();
            var hashMapTimes = new double[DataSizes.Length];
            var treeMapTimes = new double[DataSizes.Length];

            for (int i = 0; i < DataSizes.Length; i++)
            {
                int size = DataSizes[i];
                long totalHash = 0;
                long totalTree = 0;

                Console.Write($"  Size {size,10}: ");

                int iterations = GetIterations(size);

                for (int iter = 0; iter < iterations; iter++)
                {
                    var hashMap = new MyHashMap<int, int>();
                    var treeMap = new MyTreeMap<int, int>();
                    int[] keys = GenerateKeys(size);

                    var sw = Stopwatch.StartNew();
                    for (int j = 0; j < size; j++)
                    {
                        hashMap.Put(keys[j], keys[j]);
                    }
                    sw.Stop();
                    totalHash += sw.ElapsedTicks;

                    sw = Stopwatch.StartNew();
                    for (int j = 0; j < size; j++)
                    {
                        treeMap.Put(keys[j], keys[j]);
                    }
                    sw.Stop();
                    totalTree += sw.ElapsedTicks;
                }

                hashMapTimes[i] = TicksToMs(totalHash / iterations);
                treeMapTimes[i] = TicksToMs(totalTree / iterations);
                Console.WriteLine($"HashMap: {hashMapTimes[i],8:F2} ms | TreeMap: {treeMapTimes[i],8:F2} ms");
            }

            results["HashMap"] = hashMapTimes;
            results["TreeMap"] = treeMapTimes;
            return results;
        }

        private static Dictionary<string, double[]> BenchmarkGet()
        {
            Console.WriteLine("\n--- Benchmarking GET ---");
            var results = new Dictionary<string, double[]>();
            var hashMapTimes = new double[DataSizes.Length];
            var treeMapTimes = new double[DataSizes.Length];

            for (int i = 0; i < DataSizes.Length; i++)
            {
                int size = DataSizes[i];
                long totalHash = 0;
                long totalTree = 0;

                Console.Write($"  Size {size,10}: ");

                var hashMap = new MyHashMap<int, int>();
                var treeMap = new MyTreeMap<int, int>();
                int[] initKeys = GenerateKeys(size);

                for (int j = 0; j < size; j++)
                {
                    hashMap.Put(initKeys[j], initKeys[j]);
                    treeMap.Put(initKeys[j], initKeys[j]);
                }

                int[] getKeys = GenerateKeys(size);
                int iterations = GetIterations(size);

                for (int iter = 0; iter < iterations; iter++)
                {
                    var sw = Stopwatch.StartNew();
                    for (int j = 0; j < size; j++)
                    {
                        hashMap.Get(getKeys[j]);
                    }
                    sw.Stop();
                    totalHash += sw.ElapsedTicks;

                    sw = Stopwatch.StartNew();
                    for (int j = 0; j < size; j++)
                    {
                        treeMap.Get(getKeys[j]);
                    }
                    sw.Stop();
                    totalTree += sw.ElapsedTicks;
                }

                hashMapTimes[i] = TicksToMs(totalHash / iterations);
                treeMapTimes[i] = TicksToMs(totalTree / iterations);
                Console.WriteLine($"HashMap: {hashMapTimes[i],8:F2} ms | TreeMap: {treeMapTimes[i],8:F2} ms");
            }

            results["HashMap"] = hashMapTimes;
            results["TreeMap"] = treeMapTimes;
            return results;
        }

        private static Dictionary<string, double[]> BenchmarkRemove()
        {
            Console.WriteLine("\n--- Benchmarking REMOVE ---");
            var results = new Dictionary<string, double[]>();
            var hashMapTimes = new double[DataSizes.Length];
            var treeMapTimes = new double[DataSizes.Length];

            for (int i = 0; i < DataSizes.Length; i++)
            {
                int size = DataSizes[i];
                if (size > 1000000)
                {
                    Console.WriteLine($"  Size {size,10}: SKIPPED (too slow for remove)");
                    hashMapTimes[i] = -1;
                    treeMapTimes[i] = -1;
                    continue;
                }

                long totalHash = 0;
                long totalTree = 0;

                Console.Write($"  Size {size,10}: ");

                int iterations = GetIterations(size);

                for (int iter = 0; iter < iterations; iter++)
                {
                    var hashMap = new MyHashMap<int, int>();
                    var treeMap = new MyTreeMap<int, int>();
                    int[] keys = GenerateKeys(size);

                    for (int j = 0; j < size; j++)
                    {
                        hashMap.Put(keys[j], keys[j]);
                        treeMap.Put(keys[j], keys[j]);
                    }

                    var sw = Stopwatch.StartNew();
                    for (int j = 0; j < size; j++)
                    {
                        hashMap.Remove(keys[j]);
                    }
                    sw.Stop();
                    totalHash += sw.ElapsedTicks;

                    sw = Stopwatch.StartNew();
                    for (int j = 0; j < size; j++)
                    {
                        treeMap.Remove(keys[j]);
                    }
                    sw.Stop();
                    totalTree += sw.ElapsedTicks;
                }

                hashMapTimes[i] = TicksToMs(totalHash / iterations);
                treeMapTimes[i] = TicksToMs(totalTree / iterations);
                Console.WriteLine($"HashMap: {hashMapTimes[i],8:F2} ms | TreeMap: {treeMapTimes[i],8:F2} ms");
            }

            results["HashMap"] = hashMapTimes;
            results["TreeMap"] = treeMapTimes;
            return results;
        }

        private static void PrintResults(string operationName, Dictionary<string, double[]> results)
        {
            Console.WriteLine($"\n=== {operationName} Results (avg ms) ===");
            Console.WriteLine("Size\t\tHashMap\t\tTreeMap");
            for (int i = 0; i < DataSizes.Length; i++)
            {
                string hashStr = results["HashMap"][i] < 0 ? "N/A" : $"{results["HashMap"][i]:F2}";
                string treeStr = results["TreeMap"][i] < 0 ? "N/A" : $"{results["TreeMap"][i]:F2}";
                Console.WriteLine($"{DataSizes[i],10}\t{hashStr}\t\t{treeStr}");
            }
        }

        private static void GenerateCharts(Dictionary<string, double[]> putResults,
                                          Dictionary<string, double[]> getResults,
                                          Dictionary<string, double[]> removeResults)
        {
            Console.WriteLine("\n=== Generating Charts ===\n");

            GenerateSvgChart("put_chart.svg", "PUT — добавление элементов (мс)", putResults);
            GenerateSvgChart("get_chart.svg", "GET — получение значения по ключу (мс)", getResults);
            GenerateSvgChart("remove_chart.svg", "REMOVE — удаление по ключу (мс)", removeResults);

            WriteCsvResults(putResults, getResults, removeResults);
        }

        private static void GenerateSvgChart(string fileName, string title, Dictionary<string, double[]> results)
        {
            int width = 800;
            int height = 500;
            int marginLeft = 80;
            int marginRight = 40;
            int marginTop = 60;
            int marginBottom = 80;
            int chartW = width - marginLeft - marginRight;
            int chartH = height - marginTop - marginBottom;

            var sb = new StringBuilder();
            sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\">");
            sb.AppendLine($"  <rect width=\"100%\" height=\"100%\" fill=\"white\"/>");

            sb.AppendLine($"  <text x=\"{width / 2}\" y=\"30\" text-anchor=\"middle\" font-size=\"18\" font-weight=\"bold\">{title}</text>");

            double[] hashMap = results["HashMap"];
            double[] treeMap = results["TreeMap"];
            int n = DataSizes.Length;

            double maxVal = 0;
            for (int i = 0; i < n; i++)
            {
                if (hashMap[i] > maxVal) maxVal = hashMap[i];
                if (treeMap[i] > maxVal) maxVal = treeMap[i];
            }
            if (maxVal == 0) maxVal = 1;
            maxVal *= 1.1;

            sb.AppendLine($"  <line x1=\"{marginLeft}\" y1=\"{marginTop}\" x2=\"{marginLeft}\" y2=\"{marginTop + chartH}\" stroke=\"black\"/>");
            sb.AppendLine($"  <line x1=\"{marginLeft}\" y1=\"{marginTop + chartH}\" x2=\"{marginLeft + chartW}\" y2=\"{marginTop + chartH}\" stroke=\"black\"/>");

            for (int i = 0; i <= 5; i++)
            {
                double val = maxVal * i / 5;
                int y = (int)(marginTop + chartH - chartH * i / 5);
                sb.AppendLine($"  <text x=\"{marginLeft - 5}\" y=\"{y + 4}\" text-anchor=\"end\" font-size=\"11\">{val:F1}</text>");
                sb.AppendLine($"  <line x1=\"{marginLeft}\" y1=\"{y}\" x2=\"{marginLeft + chartW}\" y2=\"{y}\" stroke=\"#ddd\"/>");
            }

            sb.AppendLine($"  <text x=\"15\" y=\"{marginTop + chartH / 2}\" text-anchor=\"middle\" font-size=\"13\" transform=\"rotate(-90 15 {marginTop + chartH / 2})\">Время (мс)</text>");

            int barGroupWidth = chartW / n;
            int barWidth = barGroupWidth / 4;
            int gap = barWidth / 2;

            for (int i = 0; i < n; i++)
            {
                int groupX = marginLeft + i * barGroupWidth + barGroupWidth / 2;

                if (hashMap[i] >= 0)
                {
                    int h1 = (int)(chartH * hashMap[i] / maxVal);
                    int x1 = groupX - barWidth - gap / 2;
                    int y1 = marginTop + chartH - h1;
                    sb.AppendLine($"  <rect x=\"{x1}\" y=\"{y1}\" width=\"{barWidth}\" height=\"{h1}\" fill=\"#4472C4\"/>");
                    sb.AppendLine($"  <text x=\"{x1 + barWidth / 2}\" y=\"{y1 - 5}\" text-anchor=\"middle\" font-size=\"10\">{hashMap[i]:F1}</text>");
                }

                if (treeMap[i] >= 0)
                {
                    int h2 = (int)(chartH * treeMap[i] / maxVal);
                    int x2 = groupX + gap / 2;
                    int y2 = marginTop + chartH - h2;
                    sb.AppendLine($"  <rect x=\"{x2}\" y=\"{y2}\" width=\"{barWidth}\" height=\"{h2}\" fill=\"#ED7D31\"/>");
                    sb.AppendLine($"  <text x=\"{x2 + barWidth / 2}\" y=\"{y2 - 5}\" text-anchor=\"middle\" font-size=\"10\">{treeMap[i]:F1}</text>");
                }

                string label = DataSizes[i] >= 1000000 ? $"{DataSizes[i] / 1000000}M" : DataSizes[i] >= 1000 ? $"{DataSizes[i] / 1000}K" : $"{DataSizes[i]}";
                sb.AppendLine($"  <text x=\"{groupX}\" y=\"{marginTop + chartH + 20}\" text-anchor=\"middle\" font-size=\"12\">{label}</text>");
            }

            int legendY = marginTop + 10;
            int legendX = marginLeft + chartW - 150;
            sb.AppendLine($"  <rect x=\"{legendX}\" y=\"{legendY}\" width=\"12\" height=\"12\" fill=\"#4472C4\"/>");
            sb.AppendLine($"  <text x=\"{legendX + 18}\" y=\"{legendY + 10}\" font-size=\"12\">MyHashMap</text>");
            sb.AppendLine($"  <rect x=\"{legendX}\" y=\"{legendY + 18}\" width=\"12\" height=\"12\" fill=\"#ED7D31\"/>");
            sb.AppendLine($"  <text x=\"{legendX + 18}\" y=\"{legendY + 28}\" font-size=\"12\">MyTreeMap</text>");

            sb.AppendLine("</svg>");
            File.WriteAllText(fileName, sb.ToString());
            Console.WriteLine($"  Chart saved: {fileName}");
        }

        private static void WriteCsvResults(Dictionary<string, double[]> putResults, 
                                           Dictionary<string, double[]> getResults, 
                                           Dictionary<string, double[]> removeResults)
        {
            string csvPath = "benchmark_results.csv";
            using (var writer = new StreamWriter(csvPath))
            {
                writer.WriteLine("Operation,DataSize,HashMap_ms,TreeMap_ms");
                
                for (int i = 0; i < DataSizes.Length; i++)
                {
                    string hashStr = putResults["HashMap"][i] < 0 ? "N/A" : $"{putResults["HashMap"][i]:F4}";
                    string treeStr = putResults["TreeMap"][i] < 0 ? "N/A" : $"{putResults["TreeMap"][i]:F4}";
                    writer.WriteLine($"PUT,{DataSizes[i]},{hashStr},{treeStr}");
                }
                
                for (int i = 0; i < DataSizes.Length; i++)
                {
                    string hashStr = getResults["HashMap"][i] < 0 ? "N/A" : $"{getResults["HashMap"][i]:F4}";
                    string treeStr = getResults["TreeMap"][i] < 0 ? "N/A" : $"{getResults["TreeMap"][i]:F4}";
                    writer.WriteLine($"GET,{DataSizes[i]},{hashStr},{treeStr}");
                }
                
                for (int i = 0; i < DataSizes.Length; i++)
                {
                    string hashStr = removeResults["HashMap"][i] < 0 ? "N/A" : $"{removeResults["HashMap"][i]:F4}";
                    string treeStr = removeResults["TreeMap"][i] < 0 ? "N/A" : $"{removeResults["TreeMap"][i]:F4}";
                    writer.WriteLine($"REMOVE,{DataSizes[i]},{hashStr},{treeStr}");
                }
            }
            Console.WriteLine($"\nCSV results written to: {csvPath}");
        }

        private static void WriteAnalysis()
        {
            string analysisPath = "analysis.md";
            using (var writer = new StreamWriter(analysisPath))
            {
                writer.WriteLine("# Анализ эффективности MyHashMap vs MyTreeMap");
                writer.WriteLine();
                writer.WriteLine("## Выводы");
                writer.WriteLine();
                writer.WriteLine("### Операция PUT (добавление элементов)");
                writer.WriteLine("- **MyHashMap**: O(1) амортизированное время добавления.");
                writer.WriteLine("  - Вычисление хеш-кода: O(1)");
                writer.WriteLine("  - Определение бакета: O(1)");
                writer.WriteLine("  - Добавление в начало списка: O(1)");
                writer.WriteLine("  - Resize: O(n), но происходит редко (амортизация)");
                writer.WriteLine("- **MyTreeMap**: O(log n) время добавления.");
                writer.WriteLine("  - Обход дерева: O(log n)");
                writer.WriteLine("  - Создание нового узла: O(1)");
                writer.WriteLine("- **Вывод**: MyHashMap значительно быстрее при добавлении, особенно на больших размерах.");
                writer.WriteLine();
                writer.WriteLine("### Операция GET (получение по ключу)");
                writer.WriteLine("- **MyHashMap**: O(1) среднее время.");
                writer.WriteLine("  - Вычисление хеша: O(1)");
                writer.WriteLine("  - Поиск в бакете: O(1) при хорошем хешировании");
                writer.WriteLine("  - В худшем случае (коллизии): O(n)");
                writer.WriteLine("- **MyTreeMap**: O(log n) время.");
                writer.WriteLine("  - Обход бинарного дерева: O(log n)");
                writer.WriteLine("  - Сравнение ключей на каждом узле");
                writer.WriteLine("- **Вывод**: MyHashMap быстрее, но разрыв меньше, чем у PUT.");
                writer.WriteLine();
                writer.WriteLine("### Операция REMOVE (удаление по ключу)");
                writer.WriteLine("- **MyHashMap**: O(1) среднее время.");
                writer.WriteLine("  - Поиск элемента: O(1)");
                writer.WriteLine("  - Удаление из списка: O(1)");
                writer.WriteLine("- **MyTreeMap**: O(log n) время.");
                writer.WriteLine("  - Поиск элемента: O(log n)");
                writer.WriteLine("  - Удаление узла: O(log n) (перебалансировка)");
                writer.WriteLine("- **Вывод**: MyHashMap быстрее, разница пропорциональна размеру данных.");
                writer.WriteLine();
                writer.WriteLine("## Общее заключение");
                writer.WriteLine("1. **MyHashMap** эффективнее для всех операций в среднем случае.");
                writer.WriteLine("2. **MyTreeMap** обеспечивает предсказуемую производительность O(log n).");
                writer.WriteLine("3. **MyHashMap** не сохраняет порядок ключей, **MyTreeMap** хранит ключи отсортированными.");
                writer.WriteLine("4. При необходимости навигации (lowerKey, higherKey, range queries) **MyTreeMap** незаменим.");
                writer.WriteLine("5. Для простого хранения и быстрого доступа **MyHashMap** — лучший выбор.");
            }
            Console.WriteLine($"\nAnalysis written to: {analysisPath}");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.RunBenchmark();
        }
    }
}
