using System;
using System.Collections.Generic;

class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public List<Subject> Subjects { get; set; } = new List<Subject>();

    public void Print()
    {
        Console.WriteLine($"Студент: {Name} {Surname}, {Age} лет, зачетка {StudentId}");
    }
}
