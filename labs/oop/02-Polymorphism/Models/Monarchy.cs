using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ЛР2
{
    class Monarchy : State
    {
        private string _monarch;
        public string Monarch
        {
            get => _monarch;
            set => _monarch = !string.IsNullOrWhiteSpace(value) ? value
: throw new ArgumentException("Имя правителя не может быть пустым");

        }

        private string _dynasty;
        public string Dynasty
        {
            get => _dynasty;
            set => _dynasty = !string.IsNullOrWhiteSpace(value) ? value
: throw new ArgumentException("Название династии не может быть пустым");

        }

        public Monarchy() { }
        public Monarchy(string name, uint population, uint area,
                        string monarch, string dynasty)
            : base(name, population, area)
        {
            Monarch = monarch;
            Dynasty = dynasty;
        }
        public virtual void Coronation()
        {
            Console.WriteLine($"Торжественная коронация монарха {Monarch} из династии {Dynasty}!");
        }

        public override string ToString()
        {
            return $"{base.ToString()}, правитель: {Monarch}, династия: {Dynasty}";
        }
    }
}
