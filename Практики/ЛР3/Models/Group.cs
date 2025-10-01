using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Group
{
    public string GroupName { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();

    public Group(string groupName)
    {
        GroupName = groupName;
    }

    public void AddStudent(Student student)
    {
        Students.Add(student);
    }

    public void PrintStudents()
    {
        Console.WriteLine($"Группа {GroupName}:");
        foreach (var s in Students)
            Console.WriteLine("  " + s);
    }
}