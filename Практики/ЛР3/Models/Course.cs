using System;
using System.Collections.Generic;
class Course : IPrintable
{
    public static int _nextId = 1;
    public int CourseId;
    public int Number { get; set; }
    public List<Group> Groups { get; set; } = new List<Group>();
    public List<Subject> Subjects { get; set; } = new List<Subject>();


    public Course(int Number)
    {
        CourseId = _nextId;
        _nextId += 1;
    }

    // Methods
    // Работа с группами
    public void AddGroup(Group group) => Groups.Add(group);

    public void RemoveGroup(int groupId)
    {
        Group group = FindGroup(groupId);
        if (group != null)
            Groups.Remove(group);// Может лучше реализовать уже используя FindGroup
    }

    public Group FindGroup(int groupId)
    {
        Group group = Groups.Find(g => g.GroupId == groupId);
        // может лучше конструкция try except
        if (group == null)
            throw new Exception($"Группа с id={groupId} не найдена");
        else
            return group;
    }

    // Работа с предметами
    public void AddSubject(Subject subject) => Subjects.Add(subject);

    public void RemoveSubject(int subjectId)
    {
        Subject subject = FindSubject(subjectId);
        if (subject != null)
            Subjects.Remove(subject); // Аналогично, может стоит реализовать используя SubjectFind
    }

    public Subject FindSubject(int subjectId)
    {
        Subject subject = Subjects.Find(s => s.SubjectId == subjectId);
        if (subject == null)
            throw new Exception($"Предмет с id={subjectId} не найден");
        else
            return subject;
    }
    public void Print()
    {
        Console.WriteLine($"Курс {Number} (ID {CourseId}):");

        Console.WriteLine("Предметы:");
        foreach (var subject in Subjects)
            subject.Print();

        Console.WriteLine("Группы:");
        foreach (var group in Groups)
            group.Print();
    }
}