using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ЛР3.Models;
class Course
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
    public void AddGroup(Group group)
    {
        Groups.Add(group);
    }

    public void RemoveGroup(int groupId)
    {
        var group = FindGroup(groupId);
        if (group != null)
            Groups.Remove(group);// Может лучше реализовать уже используя FindGroup
    }

    public Group FindGroup(int groupId)
    {
        var group = Groups.Find(g => g.GroupId == groupId);
        // может лучше конструкция try except
        if (group == null)
            throw new Exception($"Группа с id={groupId} не найдена");
        else
            return group;
    }

    // Работа с предметами
    public void AddSubject(Subject subject)
    {
        Subjects.Add(subject);
    }

    public void RemoveSubject(int subjectId)
    {
        var subject = FindSubject(subjectId);
        if (subject != null)
            Subjects.Remove(subject); // Аналогично, может стоит реализовать используя SubjectFind
    }

    public Subject FindSubject(int subjectId)
    {
        var subject = Subjects.Find(s => s.SubjectId == subjectId);
        if (subject == null)
            throw new Exception($"Предмет с id={subjectId} не найден");
        else
            return subject;
    }

    public void PrintCouse()
    {   
        Console.WriteLine($"Курс: {Number}");
        foreach (var group in Groups)
        {
            Console.WriteLine($"Группа: {group.GroupName}");
            foreach (var student in group.Students)
            {
                Console.Write(student.Name);
            }
        }
    } // замечание. почти такой же код в PrintInstitute, может можно все это как-то организовать логично
}