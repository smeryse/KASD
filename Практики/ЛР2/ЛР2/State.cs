using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            string name = s1.Name;
            uint population = s1.Population + s2.Population;
            uint area = s1.Area + s2.Area;
            return new State { Name = name, Population = population, Area = area };
        }
        public static bool operator >(State s1, State s2)
        {
            if (s1.Area > s2.Area) { return true; }
            else { return false; }
        }
        public static bool operator <(State s1, State s2)
        {
            if (s1.Area < s2.Area) { return true; }
            else { return false; }
        }
        public static bool operator ==(State s1, State s2)
        {
            if (s1.Area == s2.Area) { return true; }
            else { return false; }
        }
        public static bool operator !=(State s1, State s2)
        {
            if (s1.Area != s2.Area) { return true; }
            else { return false; }
        }
    }
}
