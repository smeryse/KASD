using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            Groups.Remove(group);
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
        Subjects.Remove(subject);
    }
}