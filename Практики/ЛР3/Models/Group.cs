using System;
using System.Collections.Generic;
class Group
{
    private static int _nextId = 1;
    public int GroupId { get; }
    public string GroupName { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();

    public Group(string groupName)
    {
        GroupId = _nextId++;
        GroupName = groupName;
    }

    // Работа со студентами
    public void AddStudent(Student student)
    {
        if (Students.Exists(s => s.StudentId == student.StudentId))
            throw new Exception($"Студент с ID={student.StudentId} уже есть в группе");
        Students.Add(student);
    }
    public void RemoveStudent(int studentId) => Students.Remove(FindStudent(studentId));
    public Student FindStudent(int studentId)
    {
        Student student = Students.Find(s => s.StudentId == studentId);
        if (student == null)
            throw new Exception($"Студент с id={studentId} не найден");
        else
            return student;
    }
    public void Print(string indent = "")
    {
        Console.WriteLine($"{indent}Группа {GroupName} (ID={GroupId})");

        if (Students.Count == 0)
        {
            Console.WriteLine($"{indent}   └─ Нет студентов");
        }
        else
        {
            for (int i = 0; i < Students.Count; i++)
            {
                string branch = (i == Students.Count - 1) ? "└" : "├";
                Console.Write($"{indent}   {branch}─ ");
                Students[i].Print(indent + "   │  ");
            }
        }
    }
}