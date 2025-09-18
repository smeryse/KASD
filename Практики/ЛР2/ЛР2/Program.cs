using System;

namespace ЛР2
{
    class State
    {
        public string Name { get; set; }
        public int Population { get; set; }


        public static State operator +(State s1, State s2)
        {
            string name = s1.Name;
            int people = s1.Population + s2.Population;

            return new State { Name = name, Population = people };
        }

        public void Display()
        {
            Console.WriteLine($"{0}, {0}", Name, Population);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            State s1 = new State { Name = "Russia", Population = 100 };
            State s2 = new State { Name = "Usa", Population = 1200 };
            State s3 = s1 + s2;

            s1.Display();
            s2.Display();
            s3.Display();
        }
    }
}

