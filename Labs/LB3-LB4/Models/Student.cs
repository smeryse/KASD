using System;
using System.Collections.Generic;
using System.Linq;

class Student : IPrintable, IManageable
{
    private static int _nextId = 1;
    public static void SetNextId(int v) => _nextId = v;
    public int StudentId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    // Список оценок по предметам (Grade уже содержит ссылку на Subject)
    public List<Grade> Grades { get; set; } = new List<Grade>();

    public Student() { }
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
    public string ToFormattedString(string indent = "")
    {
        string result = $"{indent}{Name} {Surname} ({Age} лет, ID={StudentId})\n";
        if (Grades.Count == 0)
            return result + $"{indent}   └─ Нет предметов и оценок\n";

        for (int i = 0; i < Grades.Count; i++)
        {
            string branch = (i == Grades.Count - 1) ? "└" : "├";
            result += $"{indent}   {branch}─ {Grades[i].ToFormattedString()}\n";
        }

        result += $"{indent}   Средний балл: {GetAverageAll():F2}\n";
        return result;
    }
    public override string ToString() => ToFormattedString();
    public void Print(string indent = "") => Console.Write(ToFormattedString(indent));
}
