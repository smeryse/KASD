using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
    public sealed class Football : Sport
    {
        public Football(string name, uint hardLvl, string teamName) 
            : base(name, hardLvl, 11, teamName)
        {
        }

        public override void DoActivity()
        {
            Console.WriteLine("Football is playing!");
        }
    }
}
