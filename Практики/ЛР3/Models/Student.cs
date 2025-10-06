using System;
using System.Collections.Generic;

class Student : IPrintable
{
    private static int _nextId = 1;
    public int StudentId { get; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public List<Subject> Subjects { get; set; } = new List<Subject>();

    public List<Grade> Grades { get; set; } = new List<Grade>();

    public Student(string name, string surname, int age)
    {
        StudentId = _nextId++;
        Name = name;
        Surname = surname;
        Age = age;
    }
    public void AddSubject(Subject subject)
    {
        if (!Subjects.Contains(subject))
            Subjects.Add(subject);
    }
    public void RemoveSubject(int subjectId) => Subjects.Remove(FindSubject(subjectId));
    public Subject FindSubject(int subjectId)
    {
        Subject subject = Subjects.Find(s => s.SubjectId == subjectId);
        if (subject == null)
            throw new Exception($"Предмет с id={subjectId} не найден");
        else
            return subject;
    }

    public void AddGrade(Subject subject, int grade)
    {
        Grade g = Grades.Find(x => x.Subject == subject);
        if (g == null)
            Grades.Add(new Grade
            {
                Subject = subject,
                Scores = new List<int> { grade }
            });
    }

    //public void EditGrade(Subject subject, int grade)

    public void RemoveGrade(Subject subject, int grade)
    {
        Grade g = FindGrade(subject);
        if (g == null)
            throw new Exception($"Предмет {subject.Title} не найден");

        g.Scores.Remove(grade);
    }

    public Grade FindGrade(Subject subject) => Grades.Find(x => x.Subject == subject);
        
    public void Print()
    {
        Console.WriteLine($"Студент: {Name} {Surname}, {Age} лет, ID={StudentId}");
        if (Subjects.Count == 0)
            Console.WriteLine("  Нет предметов");
        else
            foreach (var subject in Subjects)
                subject.Print();
    }

}
