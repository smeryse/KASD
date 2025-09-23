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

        // Дополнительные свойства: Capital, Regions (массив)
        // Переопределить Coronation() и ToString()
        public Kingdom(string name, uint population, uint area, string monarch, string dynasty, string capital) : base(name, population, area, monarch, dynasty)
        {
            Capital = capital;
        }
    }
}
