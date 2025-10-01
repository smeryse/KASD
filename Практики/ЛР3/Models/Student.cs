using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЛР3.Models;

class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Id { get; set; }
    public List<Subject> Subjects { get; set; } = new List<Subject>();

    public Student(string name, int age, string id)
    {
        Name = name;
        Age = age;
        Id = id;
    }

    public void AddSubject(Subject subject)
    {
        Subjects.Add(subject);
    }

    public override string ToString()
    {
        return $"{Name}, {Age} лет, зачетка {Id}";
    }

    public void PrintSubjects()
    {
        Console.WriteLine($"Предметы студента {Name}:");
        foreach (var s in Subjects)
            Console.WriteLine("  " + s);
    }
}
