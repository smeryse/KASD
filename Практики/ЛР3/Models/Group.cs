using System;
using System.Collections.Generic;
class Group
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();

    public Group(string groupName)
    {
        GroupName = groupName;
    }

    public void AddStudent(Student student) => Students.Add(student);
    public void Print()
    {
        Console.WriteLine($"Группа: {GroupName}");
        foreach (var student in Students)
            student.Print();
    }
}