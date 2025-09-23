using System;

namespace ЛР2
{
    class Republic : State
    {
        public string President { get; set; }
        public string GovernmentType { get; set; } = "Республика";

        // Конструкторы
        public Republic() { }

        public Republic(string name, uint population, uint area, string president)
            : base(name, population, area)
        {
            President = president;
        }

        public void HoldElections()
        {
            Console.WriteLine($"В республике {Name} проводятся выборы президента!");
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Президент: {President}, Тип: {GovernmentType}";
        }
    }
}