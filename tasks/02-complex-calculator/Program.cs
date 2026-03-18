using System;
using System.Linq;
using Task2.Collections;


class Program
{
    #region Helpers
    // Reads a complex number from user input with given prompt
    static Complex ReadComplex(string prompt)
    {
        Console.WriteLine(prompt);
        string[] parts = Console.ReadLine().Split();
        return new Complex(double.Parse(parts[0]), double.Parse(parts[1]));
    }
    #endregion

    #region Main
    // Interactive calculator for complex numbers
    static void Main()
    {
        string[] single_operations = { "GetReal", "GetImag", "Module", "Argument", "ToString" };
        string[] double_operations = { "+", "-", "*", "/" };

        Complex a = new Complex(0, 0);
        string choice = "";

        while (true)
        {
            Console.WriteLine($"\nТекущее число: {a}");
            Console.WriteLine("Введите операцию (+, -, *, /, GetReal, GetImag, Module, Argument, ToString) или Q для выхода:");
            choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
                break;

            if (single_operations.Contains(choice))
            {
                a = ReadComplex("Введите число (Действительная и мнимая часть через пробел):");

                switch (choice)
                {
                    case "GetReal":
                        Console.WriteLine(a.GetReal());
                        break;
                    case "GetImag":
                        Console.WriteLine(a.GetImag());
                        break;
                    case "Module":
                        Console.WriteLine(a.Module());
                        break;
                    case "Argument":
                        Console.WriteLine(a.Argument());
                        break;
                    case "ToString":
                        Console.WriteLine(a);
                        break;
                }
            }
            else if (double_operations.Contains(choice))
            {
                Complex b = ReadComplex("Введите второе число (Действительная и мнимая часть через пробел):");

                switch (choice)
                {
                    case "+":
                        a += b;
                        break;
                    case "-":
                        a -= b;
                        break;
                    case "*":
                        a *= b;
                        break;
                    case "/":
                        a /= b;
                        break;
                }

                Console.WriteLine($"Результат: {a}");
            }
            else
            {
                Console.WriteLine("Неизвестная команда");
            }
        }

        Console.WriteLine("Программа завершена.");
    }
    #endregion
}
