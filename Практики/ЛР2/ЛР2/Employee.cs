using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР2
{
    class Employee : Person
    {
        public string Company { get; set; }
        public Employee(string lName, string fName, string comp)
            : base(fName, lName)
        {
            Company = comp;
        }

        public override void Display()
        {
            Console.WriteLine(FirstName + " " + LastName + " работает в компании " + Company); 
        }
    }
}
