using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР2
{
    class State
    {
        private string _name;
        public string Name
        {
            get => _name; 
            set => _name = !string.IsNullOrWhiteSpace(value) ? value
        : throw new ArgumentException("Название государства не может быть пустым");
        }
        public uint Population { get; set; }
        public uint Area { get; set; }

        public State() { }
        public State(string name, uint population, uint area)
        {
            Name = name;
            Population = population;
            Area = area;
        }
        
        public static State operator +(State s1, State s2)
        {
            if (s1 is null || s2 is null)
                throw new ArgumentNullException("Слагаемые не могут быть null");
            return new State ( s1.Name, s1.Population + s2.Population, s1.Area + s2.Area );
        }


        public static bool operator >(State s1, State s2)
        {
            if (s1 is null) return false;
            if (s2 is null) return true;
            return s1.Area > s2.Area;
        }


        public static bool operator <(State s1, State s2)
        {
            if (s1 is null) return true;
            if (s2 is null) return false;
            return s1.Area < s2.Area;
        }


        public override string ToString()
        {
            return string.Format($"Государство: {Name}, население: {Population}, площадь: {Area}м2");
        }
    }
}
