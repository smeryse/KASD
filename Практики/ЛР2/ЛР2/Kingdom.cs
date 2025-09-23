using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР2
{
    class Kingdom : ConstitutionalMonarchy
    {
        private string _capital;
        public string Capital
        {
            get => _capital;
            set => _capital = !string.IsNullOrWhiteSpace(value) ? value
                : throw new ArgumentException("Название столицы не может быть пустым");
        }

        // Переопределить Coronation() и ToString()
        public Kingdom() { }
        public Kingdom(string name, uint population, uint area,
                       string monarch, string dynasty,
                       string primeMinister,
                       string capital) 
            : base(name, population, area, monarch, dynasty, primeMinister)
        {
            Capital = capital;
        }
        public override void Coronation()
        {
            Console.WriteLine($"Великая коронация в королевстве {Name}!");
            Console.WriteLine($"Церемония проходит в столице {Capital}");
            Console.WriteLine($"По конституции от {ConstitutionDate} под руководством {PrimeMinister}");
        }
        public override string ToString()
        {
            return $"{base.ToString()}, Столица: {Capital}";
        }
    }
}
