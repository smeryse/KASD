using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР2
{
    class Republic : State
    {
        private string _president;

        public string President
        {
            get { return _president; }
            set { _president = value; }
        }


    }
}
