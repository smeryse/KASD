using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР2
{
    class State
    {
        public string Name { get; set; }
        public uint Population { get; set; }
        public uint Area { get; set; }
        public static State operator +(State s1, State s2)
            {
            }
    }
}
