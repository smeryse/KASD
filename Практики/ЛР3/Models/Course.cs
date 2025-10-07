using System;
using System.Collections.Generic;
class Course
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

    public string ToFormattedString(string indent = "")
    {
        string result = $"{indent}Курс {Number} (ID={CourseId})\n";

        // Предметы
        if (Subjects.Count == 0)
            result += $"{indent}   ├─ Нет предметов\n";
        else
        {
            result += $"{indent}   ├─ Предметы:\n";
            for (int i = 0; i < Subjects.Count; i++)
            {
                string branch = (i == Subjects.Count - 1) ? "└" : "├";
                result += $"{indent}   │  {branch}─ {Subjects[i].ToFormattedString()}\n";
            }
        }

        // Группы
        if (Groups.Count == 0)
            result += $"{indent}   └─ Нет групп\n";
        else
        {
            result += $"{indent}   └─ Группы:\n";
            foreach (var g in Groups)
                result += g.ToFormattedString(indent + "      ");
        }

        return result;
    }
    public override string ToString() => ToFormattedString();
    public void Print(string indent = "") => Console.Write(ToFormattedString(indent));
}