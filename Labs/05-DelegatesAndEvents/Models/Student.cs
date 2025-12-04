using System;
using System.Collections.Generic;
using System.Linq;

namespace ЛР5
{

    class Student : IPrintable, IManageable
    {
        private static int _nextId = 1;
        public static void SetNextId(int v) => _nextId = v;
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public List<Grade> Grades { get; set; } = new List<Grade>();

        // === Событие ошибок ===
        public event EventHandler<StudentEventArgs> ErrorOccurred;

        // Публичный метод для вызова события
        public void RaiseError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, new StudentEventArgs(ex));
        }

        public Student() { }
        public Student(string name, string surname, int age)
        {
            StudentId = _nextId++;
            Name = name;
            Surname = surname;
            Age = age;
        }

        // Добавление предмета
        public void AddSubject(Subject subject)
        {
            try
            {
                if (Grades.Any(g => g.Subject.SubjectId == subject.SubjectId))
                    throw new Exception($"Предмет {subject.Title} уже есть у студента");
                Grades.Add(new Grade(subject));
            }
            catch (Exception ex) { RaiseError(ex); }
        }

        // Удаление предмета
        public void RemoveSubject(int subjectId)
        {
            try
            {
                var grade = Grades.FirstOrDefault(g => g.Subject.SubjectId == subjectId);
                if (grade == null)
                    throw new Exception($"Предмет с id={subjectId} не найден");
                Grades.Remove(grade);
            }
            catch (Exception ex) { RaiseError(ex); }
        }

        // Добавление оценки
        public void AddGrade(Subject subject, int score)
        {
            try
            {
                Grade grade = Grades.FirstOrDefault(g => g.Subject.SubjectId == subject.SubjectId);
                if (grade == null)
                {
                    grade = new Grade(subject);
                    Grades.Add(grade);
                }
                grade.AddScore(score);
            }
            catch (Exception ex) { RaiseError(ex); }
        }

        // Удаление оценки
        public void RemoveGrade(Subject subject, int score)
        {
            try
            {
                Grade grade = Grades.FirstOrDefault(g => g.Subject.SubjectId == subject.SubjectId);
                if (grade == null)
                    throw new Exception($"Предмет {subject.Title} не найден");
                grade.RemoveScore(score);
            }
            catch (Exception ex) { RaiseError(ex); }
        }

        public double GetAverageAll()
        {
            var validGrades = Grades.Where(g => g.Scores.Count > 0);
            return validGrades.Any() ? validGrades.Average(g => g.Average) : 0;
        }

        public Grade FindGrades(Subject subject)
        {
            try
            {
                Grade grades = Grades.FirstOrDefault(g => g.Subject.SubjectId == subject.SubjectId);
                if (grades == null)
                    throw new Exception($"Предмет {subject.Title} не найден у студента");
                return grades;
            }
            catch (Exception ex) { RaiseError(ex); return null; }
        }

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
}