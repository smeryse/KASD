using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Вариант 23
            Music guitar = new Music("Игра на гитаре", 7, "Ля минор");
            Football football = new Football("Футбол", 6, "Динамо");

            guitar.DoActivity();
            football.DoActivity();

            Console.ReadLine();
        }
    }
}