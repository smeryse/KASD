using System;
using System.Collections.Generic;

class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Subject> Subjects { get; set; } = new List<Subject>();

    public void PrintStudent()
    {
        Console.WriteLine($"Студент: {Name}, {Age} лет, ID: {StudentId}");

        if (Subjects.Count == 0)
        {
            Console.WriteLine("  Нет предметов");
        }
        else
        {
            Console.WriteLine("  Предметы:");
            foreach (var subject in Subjects)
                Console.WriteLine("   - " + subject.Name);
        }
    }
}
