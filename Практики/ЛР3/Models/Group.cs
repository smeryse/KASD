using System;
using System.Collections.Generic;
class Group : IPrintable
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
    public void Print()
    {
        Console.WriteLine($"  Группа {GroupName} (ID={GroupId}):");

        if (Students.Count == 0)
            Console.WriteLine("    Нет студентов");
        else
            foreach (var student in Students)
                student.Print();
    }

}