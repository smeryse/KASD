using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Institute
{
    public string Name { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();

    public Institute(string name)
    {
        Name = name;
    }

    public void AddCourse(Course course)
    {
        Courses.Add(course);
    }

    public void PrintInstitute()
    {
        Console.WriteLine($"Институт: {Name}");
        foreach (var c in Courses)
            c.PrintCourse();
    }
}
