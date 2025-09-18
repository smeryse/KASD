using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР2
{   
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private static int count = 0;

        public static int Count
        {
            get { return count; }
        }
        public Person(string lName, string fName)
        {
            FirstName = fName;
            LastName = lName;
            count++;
        }


        public virtual void Display()
        {
            Console.WriteLine(FirstName + " " + LastName);
        }
    }
}
