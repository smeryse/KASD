using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using ЛР3.Models;
class Course
{
    public static int _nextId = 1;
    public int CourseId;
    public int Number { get; set; }
    public List<Group> Groups;
    public List<Subject> Subjects;

    public Course(int Number)
    {
        CourseId = _nextId;
        _nextId += 1;
        Groups = new List<Group>();
        Subjects = new List<Subject>();
    }

    // Methods
    // Работа с группами
    public void AddGroup(Group group)
    {
        Groups.Add(group);
    }

    public void RemoveGroup(int groupId)
    {
        var group = Groups.Find(g => g.GroupId == groupId);
        if (group != null)
            Groups.Remove(group);// Может лучше реализовать уже используя FindGroup
    }

    public Group FindGroup(int groupId)
    {
        var group = Groups.Find(g => g.GroupId == groupId);
        // может лучше конструкция try except
        if (group != null)
            return group;
        else
            return null;
    }

    // Работа с предметами
    public void AddSubject(Subject subject)
    {
        Subjects.Add(subject);
    }

    public void RemoveSubject(int subjectId)
    {
        var subject = Subjects.Find(s => s.SubjectId == subjectId);
        if (subject != null)
            Subjects.Remove(subject); // Аналогично, может стоит реализовать используя SubjectFind
    }

    public Subject FindSubject(int subjectId)
    {
        var subject = Subjects.Find(s => s.SubjectId == subjectId);
        if (subject != null)
            return subject;
        else
            return null;
    }

    public void PrintCouse()
    {
        Console.WriteLine($"Курс: {0}", Number);
        foreach (var group in Groups)
        {
            Console.WriteLine($"Группа: {0}", group.GroupName);
            foreach (var student in group.Students)
            {
                Console.Write(student.Name);
            }
        }
    } // замечание. почти такой же код в PrintInstitute, может можно все это как-то организовать логично
}