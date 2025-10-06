using System;
using System.Collections.Generic;
class Course : IManageable
{
    private static int _nextId = 1;
    public int CourseId { get; }
    public int Number { get; set; }
    public List<Group> Groups { get; set; } = new List<Group>();
    public List<Subject> Subjects { get; set; } = new List<Subject>();


    public Course(int number)
    {
        CourseId = _nextId++;
        Number = number;
    }

    // Работа с группами
    public void AddGroup(Group group)
    {
        if (Groups.Exists(g => g.GroupId == group.GroupId))
            throw new Exception($"Группа с ID={group.GroupId} уже есть на курсе");
        Groups.Add(group);
    }

    public void RemoveGroup(int groupId)
    {
        Group group = FindGroup(groupId);
        Groups.Remove(group);
    }

    public Group FindGroup(int groupId)
    {
        Group group = Groups.Find(g => g.GroupId == groupId);
        if (group == null)
            throw new Exception($"Группа с id={groupId} не найдена");
        return group;
    }

    // Работа с предметами
    public void AddSubject(Subject subject)
    {
        if (Subjects.Exists(s => s.SubjectId == subject.SubjectId))
            throw new Exception($"Предмет с ID={subject.SubjectId} уже добавлен на курс");
        Subjects.Add(subject);
    }
    public void RemoveSubject(int subjectId)
    {
        Subject subject = FindSubject(subjectId);
        Subjects.Remove(subject);
    }

    public Subject FindSubject(int subjectId)
    {
        Subject subject = Subjects.Find(s => s.SubjectId == subjectId);
        if (subject == null)
            throw new Exception($"Предмет с id={subjectId} не найден");
        return subject;
    }

    public void Print()
    {
        Console.WriteLine($"Курс {Number} (ID={CourseId}):");

        // --- Предметы ---
        if (Subjects.Count == 0)
            Console.WriteLine("  Нет предметов");
        else
        {
            Console.WriteLine("  Предметы:");
            foreach (var subject in Subjects)
                subject.Print();
        }

        // --- Группы ---
        if (Groups.Count == 0)
            Console.WriteLine("  Нет групп");
        else
        {
            Console.WriteLine("  Группы:");
            foreach (var group in Groups)
                group.Print();
        }
    }

    public void Add<T>(T item)
    {
        switch (item)
        {
            case Group g:
                AddGroup(g);
                break;

            case Subject s:
                AddSubject(s);
                break;

            default:
                throw new ArgumentException($"Тип {typeof(T).Name} не поддерживается.");
        }
    }

    public void Remove<T>(int id)
    {
        switch (typeof(T).Name)
        {
            case nameof(Group):
                RemoveGroup(id);
                break;
            case nameof(Subject):
                RemoveSubject(id);
                break;

            default:
                throw new ArgumentException($"Тип {typeof(T).Name} не поддерживается для удаления.");
        }
    }

    public T Find<T>(int id)
    {
        object result;
        switch (typeof(T).Name)
        {
            case nameof(Group):
                result = FindGroup(id);
                break;

            case nameof(Subject):
                result = FindSubject(id);
                break;

            default: throw new ArgumentException($"Тип {typeof(T).Name} не поддерживается для поиска.");
        };

        return (T)result;
    }
}