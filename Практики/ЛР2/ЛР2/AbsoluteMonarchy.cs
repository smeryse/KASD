using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ЛР2
{
    class AbsoluteMonarchy : Monarchy
    {
        // Дополнительные свойства: RoyalGuardCount
        // Метод: IssueDecree() - издание указа
        // Переопределить Coronation() и ToString()

        private uint _royalGuardCount;
        public uint RoyalGuardCount
        {
            get { return _royalGuardCount; }
            set
            {
                if (_royalGuardCount > Population)
                {
                    throw new ArgumentException("Численность армии не может превышать население");
                }
                else { _royalGuardCount = value;  }
            }
        }

        public void IssueDecree(string title, string content)
        {
            Console.WriteLine($"=== Указ абсолютного монарха {Monarch} ===");
            Console.WriteLine($"Заголовок: {title}");
            Console.WriteLine($"Содержание: {content}");
            Console.WriteLine($"Скреплено печатью династии {Dynasty}");
        }

        public AbsoluteMonarchy() { }
        public AbsoluteMonarchy(string name, uint population, uint area, string monarch, string dynasty, uint royalguardcount)
            : base(name, population, area, monarch, dynasty)
        {
            RoyalGuardCount = royalguardcount;
        }

        public override void Coronation()
        {
            Console.WriteLine($"Торжественная коронация абсолютного монарха {Monarch}!");
            Console.WriteLine($"Королевская гвардия из {RoyalGuardCount} солдат отдает честь!");
        }

        public override string ToString()
        {
            return $"{base.ToString()}, абсолютная моранхия, королевская гвардия: {RoyalGuardCount} солдат";
        }
    }
}
