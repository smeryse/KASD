using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР3.Models
{
    class Course
    {
        public int Number { get; set; } // например, 2 курс
        public List<Group> Groups { get; set; } = new List<Group>();

        public Course(int number)
        {
            Number = number;
        }

        public void AddGroup(Group group)
        {
            Groups.Add(group);
        }

        public void PrintCourse()
        {
            Console.WriteLine($"{Number} курс:");
            foreach (var g in Groups)
                g.PrintStudents();
        }
    }
}
