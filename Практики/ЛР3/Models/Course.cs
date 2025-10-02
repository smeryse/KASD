using System;
using System.Collections.Generic;
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
    public void AddGroup(Group group)
    {
        Groups.Add(group);
    }

    public void RemoveGroup(Group group)
    {
        Groups.Remove(group);
    }

    public void AddSubject(Subject subject)
    {
        Subjects.Add(subject);
    }

}