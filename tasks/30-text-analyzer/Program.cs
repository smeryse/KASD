using System;

namespace Task30
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задача 30: MyString ===\n");

            Console.WriteLine("--- Конструкторы ---");
            MyString s1 = new MyString();
            Console.WriteLine($"Пустая строка: '{s1}', длина: {s1.Length()}");

            MyString s2 = new MyString(new char[] { 'H', 'e', 'l', 'l', 'o' });
            Console.WriteLine($"Из массива: '{s2}', длина: {s2.Length()}");

            MyString s3 = new MyString(s2);
            Console.WriteLine($"Копия: '{s3}', длина: {s3.Length()}");

            Console.WriteLine("\n--- length() и charAt() ---");
            Console.WriteLine($"s2.Length() = {s2.Length()}");
            Console.WriteLine($"s2.CharAt(0) = '{s2.CharAt(0)}'");
            Console.WriteLine($"s2.CharAt(4) = '{s2.CharAt(4)}'");

            Console.WriteLine("\n--- substring() ---");
            MyString sub = s2.Substring(1, 4);
            Console.WriteLine($"s2.Substring(1,4) = '{sub}'");

            Console.WriteLine("\n--- concat() ---");
            MyString s4 = new MyString(new char[] { ' ', 'W', 'o', 'r', 'l', 'd' });
            MyString concat = s2.Concat(s4);
            Console.WriteLine($"s2.Concat(s4) = '{concat}'");

            Console.WriteLine("\n--- equals() и equalsIgnoreCase() ---");
            MyString s5 = new MyString(new char[] { 'h', 'e', 'l', 'l', 'o' });
            Console.WriteLine($"s2.Equals(s3) = {s2.Equals(s3)}");
            Console.WriteLine($"s2.Equals(s5) = {s2.Equals(s5)}");
            Console.WriteLine($"s2.EqualsIgnoreCase(s5) = {s2.EqualsIgnoreCase(s5)}");

            Console.WriteLine("\n--- toLowerCase() и toUpperCase() ---");
            MyString mixed = new MyString(new char[] { 'H', 'e', 'L', 'l', 'O' });
            Console.WriteLine($"toLowerCase: '{mixed.ToLowerCase()}'");
            Console.WriteLine($"toUpperCase: '{mixed.ToUpperCase()}'");

            Console.WriteLine("\n--- trim() ---");
            MyString spaced = new MyString(new char[] { ' ', ' ', 'H', 'i', ' ', ' ' });
            Console.WriteLine($"trim: '{spaced.Trim()}'");

            Console.WriteLine("\n--- replace() ---");
            Console.WriteLine($"replace('l','X'): '{s2.Replace('l', 'X')}'");

            Console.WriteLine("\n--- contains() и indexOf() ---");
            MyString ell = new MyString(new char[] { 'e', 'l', 'l' });
            Console.WriteLine($"s2.Contains('ell'): {s2.Contains(ell)}");
            Console.WriteLine($"s2.IndexOf('ell'): {s2.IndexOf(ell)}");
            MyString xyz = new MyString(new char[] { 'x', 'y', 'z' });
            Console.WriteLine($"s2.Contains('xyz'): {s2.Contains(xyz)}");
            Console.WriteLine($"s2.IndexOf('xyz'): {s2.IndexOf(xyz)}");

            Console.WriteLine("\n--- split() ---");
            MyString csv = new MyString(new char[] { 'a', ',', 'b', ',', 'c' });
            MyString[] parts = csv.Split(',');
            Console.WriteLine($"split(',') на '{csv}':");
            foreach (var p in parts)
                Console.WriteLine($"  '{p}'");

            Console.WriteLine("\n--- startsWith() и endsWith() ---");
            MyString he = new MyString(new char[] { 'H', 'e' });
            MyString lo = new MyString(new char[] { 'l', 'o' });
            Console.WriteLine($"s2.StartsWith('He'): {s2.StartsWith(he)}");
            Console.WriteLine($"s2.EndsWith('lo'): {s2.EndsWith(lo)}");

            Console.WriteLine("\n--- reverse() ---");
            Console.WriteLine($"s2.Reverse() = '{s2.Reverse()}'");

            Console.WriteLine("\n--- valueOf() ---");
            Console.WriteLine($"valueOf(42) = '{MyString.ValueOf(42)}'");
            Console.WriteLine($"valueOf(3.14) = '{MyString.ValueOf(3.14)}'");
            Console.WriteLine($"valueOf(true) = '{MyString.ValueOf(true)}'");

            Console.WriteLine("\n--- toString() ---");
            Console.WriteLine($"s2.ToString() = '{s2.ToString()}'");

            Console.WriteLine("\n=== Готово ===");
        }
    }
}
