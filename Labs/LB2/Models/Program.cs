using System;

namespace ЛР2
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Создание объектов каждого типа
            Console.WriteLine("1. СОЗДАНИЕ ОБЪЕКТОВ:");
            Console.WriteLine("----------------------");

            var russia = new Republic("Российская Федерация", 146000000, 17100000, "Владимир Путин");
            Console.WriteLine($"Создана: {russia}");
        }
    }
    class Test
    {

    }
}

