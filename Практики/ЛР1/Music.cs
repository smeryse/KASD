using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
    class Music : Hobby
    {
        private string tonality;

        public Music(string name, uint hardLvl, string tonality)
            : base(name, hardLvl)
        {
            Tonality = tonality;
        }

        public string Tonality
        {
            get { return tonality; }
            set { tonality = value; }
        }
        public override void DoActivity()
        {
            Console.WriteLine("Music is playing 🎵🎶");
        }
    }
}
