using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    private static int _nextId = 1;
    public int StudentId { get; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    // Список оценок по предметам (Grade уже содержит ссылку на Subject)
    public List<Grade> Grades { get; set; } = new List<Grade>();

    public Student(string name, string surname, int age)
    {
        StudentId = _nextId++;
        Name = name;
        Surname = surname;
        Age = age;
    }

    // Добавление предмета (создаём запись Grade для этого предмета)
    public void AddSubject(Subject subject)
    {
        if (Grades.Any(g => g.Subject.SubjectId == subject.SubjectId))
            throw new Exception($"Предмет {subject.Title} уже есть у студента");
        Grades.Add(new Grade(subject));
    }

    // Удаление предмета и всех оценок по нему
    public void RemoveSubject(int subjectId)
    {
        var grade = Grades.FirstOrDefault(g => g.Subject.SubjectId == subjectId);
        if (grade == null)
            throw new Exception($"Предмет с id={subjectId} не найден");
        Grades.Remove(grade);
    }

    // Добавление оценки по предмету
    public void AddGrade(Subject subject, int score)
    {
        Grade grade = Grades.FirstOrDefault(g => g.Subject.SubjectId == subject.SubjectId);
        if (grade == null)
        {
            grade = new Grade(subject);
            Grades.Add(grade);
        }
        grade.AddScore(score);
    }

    // Удаление оценки
    public void RemoveGrade(Subject subject, int score)
    {
        Grade grade = Grades.FirstOrDefault(g => g.Subject.SubjectId == subject.SubjectId);
        if (grade == null)
            throw new Exception($"Предмет {subject.Title} не найден");
        grade.RemoveScore(score);
    }

    // Получить средний балл по всем предметам
    public double GetAverageAll()
    {
        IEnumerable<Grade> validGrades = Grades.Where(g => g.Scores.Count > 0);
        return validGrades.Any() ? validGrades.Average(g => g.Average) : 0;
    }

    // Поиск оценок по предмету
    public Grade FindGrades(Subject subject)
    {
        Grade grades = Grades.FirstOrDefault(g => g.Subject.SubjectId == subject.SubjectId);
        if (grades == null)
            throw new Exception($"Предмет {subject.Title} не найден у студента");
        return grades;
    }

    // Вывод информации о студенте
    public void Print(string indent = "")
    {
        Console.WriteLine($"{Name} {Surname} ({Age} лет, ID={StudentId})");

        if (Grades.Count == 0)
        {
            Console.WriteLine($"{indent}   └─ Нет предметов и оценок");
            return;
        }

        for (int i = 0; i < Grades.Count; i++)
        {
            string branch = (i == Grades.Count - 1) ? "└" : "├";
            var g = Grades[i];
            string scores = g.Scores.Count > 0 ? $"[{string.Join(", ", g.Scores)}]" : "нет оценок";
            Console.WriteLine($"{indent}   {branch}─ {g.Subject.Title}: {scores} -> ср. {g.Average:F2}");
        }

        Console.WriteLine($"{indent}   Средний балл: {GetAverageAll():F2}");
    }
}
