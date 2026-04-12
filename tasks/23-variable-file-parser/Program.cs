using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Task21.Collections;

namespace Task23
{
    public enum VariableType
    {
        Int,
        Float,
        Double
    }

    public class VariableDefinition
    {
        public VariableType Type { get; }
        public string Value { get; }

        public VariableDefinition(VariableType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            string typeName = Type switch
            {
                VariableType.Int => "int", 
                VariableType.Float => "float", 
                VariableType.Double => "double",
                _ => throw new InvalidOperationException()
            };
            return $"{typeName} => Value({Value})";
        }
    }

    internal static class VariableParser
    {
        private static readonly Dictionary<string, VariableType> ValidTypes = new Dictionary<string, VariableType>
        {
            { "int", VariableType.Int },
            { "float", VariableType.Float },
            { "double", VariableType.Double }
        };

        public static void ProcessFile(string inputFilePath, string outputFilePath)
        {
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException($"Входной файл не найден: {inputFilePath}");

            string content = File.ReadAllText(inputFilePath);
            
            content = Regex.Replace(content, @"\s+", " ");

            string pattern = @"([a-zA-Z]+) ([a-zA-Z_][a-zA-Z0-9_]*) = (\d+)\s*;";
            MatchCollection matches = Regex.Matches(content, pattern);

            var variableMap = new MyHashMap<string, VariableDefinition>();
            var redefinitions = new List<string>();
            var invalidDefinitions = new List<string>();

            foreach (Match match in matches)
            {
                string typeName = match.Groups[1].Value;
                string varName = match.Groups[2].Value;
                string value = match.Groups[3].Value;

                if (!ValidTypes.ContainsKey(typeName))
                {
                    invalidDefinitions.Add($"Недопустимый тип '{typeName}' в определении: {typeName} {varName} = {value};");
                    continue;
                }

                if (variableMap.ContainsKey(varName))
                {
                    redefinitions.Add($"Переопределение переменной '{varName}' (оставлено первое определение)");
                    continue;
                }

                VariableType varType = ValidTypes[typeName];
                var definition = new VariableDefinition(varType, value);
                variableMap.Put(varName, definition);
            }

            using (var writer = new StreamWriter(outputFilePath))
            {
                var keySet = variableMap.KeySet();
                foreach (var key in keySet)
                {
                    var def = variableMap.Get(key);
                    string typeName = def!.Type switch
                    {
                        VariableType.Int => "int",
                        VariableType.Float => "float",
                        VariableType.Double => "double",
                        _ => throw new InvalidOperationException()
                    };
                    writer.WriteLine($"{typeName} => {key}({def.Value})");
                }
            }

            if (invalidDefinitions.Count > 0)
            {
                Console.WriteLine("=== Недопустимые определения ===");
                foreach (var msg in invalidDefinitions)
                    Console.WriteLine($"  [ОШИБКА] {msg}");
                Console.WriteLine();
            }

            if (redefinitions.Count > 0)
            {
                Console.WriteLine("=== Переопределения переменных ===");
                foreach (var msg in redefinitions)
                    Console.WriteLine($"  [ПРЕОПР] {msg}");
                Console.WriteLine();
            }

            Console.WriteLine($"Обработано корректных переменных: {variableMap.Size}");
            Console.WriteLine($"Результат записан в: {outputFilePath}");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Использование: Task23 <входной-файл> <выходной-файл>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];

            try
            {
                VariableParser.ProcessFile(inputPath, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
