using System;
using System.Collections.Generic;

public class Group : IPrintable
{
    private static int _nextId = 1;
    public static void SetNextId(int v) => _nextId = v;
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();
    
    public Group() { }
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
    public string ToFormattedString(string indent = "")
    {
        string result = $"{indent}Группа {GroupName} (ID={GroupId})\n";
        if (Students.Count == 0)
            return result + $"{indent}   └─ Нет студентов\n";

        for (int i = 0; i < Students.Count; i++)
        {
            string branch = (i == Students.Count - 1) ? "└" : "├";
            result += $"{indent}   {branch}─ {Students[i].ToFormattedString(indent + "   │  ")}";
        }

        return result;
    }
    public override string ToString() => ToFormattedString();
    public void Print(string indent = "") => Console.Write(ToFormattedString(indent));
}

