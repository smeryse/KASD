using System;
using System.Linq;

class Complex
{
    private double Re;
    private double Im;

    public Complex(double re, double im)
    {
        Re = re;
        Im = im;
    }

    public double GetReal() => Re;
    public double GetImag() => Im;
    public double Module() => Math.Sqrt(Re * Re + Im * Im);
    public double Argument() => Math.Atan2(Im, Re);
    public override string ToString() => $"({Re} + {Im}i)";

    public static Complex operator +(Complex a, Complex b) =>
        new Complex(a.Re + b.Re, a.Im + b.Im);

    public static Complex operator -(Complex a, Complex b) =>
        new Complex(a.Re - b.Re, a.Im - b.Im);

    public static Complex operator *(Complex a, Complex b) =>
        new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);

    public static Complex operator /(Complex a, Complex b)
    {
        double denom = b.Re * b.Re + b.Im * b.Im;
        if (denom == 0) throw new DivideByZeroException();
        return new Complex(
            (a.Re * b.Re + a.Im * b.Im) / denom,
            (a.Im * b.Re - a.Re * b.Im) / denom
        );
    }
}

class Program
{
    static Complex ReadComplex(string prompt)
    {
        Console.WriteLine(prompt);
        string[] parts = Console.ReadLine().Split();
        return new Complex(double.Parse(parts[0]), double.Parse(parts[1]));
    }

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
}
