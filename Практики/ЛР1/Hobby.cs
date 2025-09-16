using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
    abstract class Hobby
    {
        private string _name;
        private uint _hard_lvl;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public uint HardLvl
        {
            get { return _hard_lvl; }
            set
            {
                if (value > 10)
                    Console.WriteLine("Hard Lvl will be 0 to 10");
                else
                    _hard_lvl = value;
            }
        }

        public Hobby(string name, uint hardlvl)
        {
            Name = name;
            HardLvl = hardlvl;
        }
        public abstract void DoActivity();
    }
}
