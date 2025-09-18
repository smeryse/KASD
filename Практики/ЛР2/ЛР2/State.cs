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

        public State() { }
        public State(string name, uint population, uint area)
        {
            Name = name;
            Population = population;
            Area = area;
        }
        
        // Перегрузка операторов сравнения
        public static State operator +(State s1, State s2)
        {
            return new State ( s1.Name, s1.Population + s2.Population, s1.Area + s2.Area );
        }


        public static bool operator >(State s1, State s2)
        {
            return s1?.Area > s2?.Area;
        }


        public static bool operator <(State s1, State s2)
        {
            return s1?.Area < s2?.Area;
        }


        public static bool operator ==(State s1, State s2)
        {
            return s1?.Area == s2?.Area;
        }


        public static bool operator !=(State s1, State s2)
        {
            return s1?.Area != s2?.Area;
        }
    }
}
