using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР3.Models
{
    class Subject
    {
        public string Title { get; set; }
        public string Teacher { get; set; }
        public int Credits { get; set; } // например, количество зачетных единиц

        public Subject(string title, string teacher, int credits)
        {
            Title = title;
            Teacher = teacher;
            Credits = credits;
        }

        public override string ToString()
        {
            return $"{Title} (преподаватель: {Teacher}, {Credits} кр.)";
        }
    }
}
