using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ЛР2
{
    class ConstitutionalMonarchy : Monarchy
    {
        // Дополнительные свойства: PrimeMinister, ConstitutionDate
        // Метод: ParliamentarySession() - заседание парламента
        // Переопределить Coronation() и ToString()

        private string _primeMinister;
        public string PrimeMinister
        {
            get => _primeMinister;
            set => _primeMinister = !string.IsNullOrWhiteSpace(value) ? value
: throw new ArgumentException("Имя премьерминистра не может быть пустым");
            
        }

        public ConstitutionalMonarchy(string name, uint population, uint area,
                                      string monarch, string dynasty, string primeminister) 
            : base(name, population, area, monarch, dynasty)
        {
            PrimeMinister = primeminister;
        }

        private DateTime _constitutionDate;

        // TODO: Дописать логику установки даты
        public DateTime ConstitutionDate
        {
            get => _constitutionDate;
            set { _constitutionDate = value; }
        }

        public void ParliamentarySession(bool status)
        {
            if (status) Console.WriteLine("Заседание парламента открыто");
            else Console.WriteLine("Заседание парламента закрыто");
        }
    }
}
