using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Group
{
    public int GroupId { get; set; }
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

    public void PrintGroup()
    {
        Console.WriteLine($" Группа {GroupName} (ID: {GroupId})");

        if (Students.Count == 0)
        {
            Console.WriteLine("   Нет студентов");
        }
        else
        {
            foreach (var student in Students)
                student.PrintStudent();
        }
    }
}