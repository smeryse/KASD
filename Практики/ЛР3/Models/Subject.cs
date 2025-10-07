using System;

class Subject
{
    private static int _nextId = 1;
    public static void SetNextId(int v) => _nextId = v;
    public int SubjectId { get; set; }
    public string Title { get; set; }
    public string Teacher { get; set; }
    public int Hours { get; set; }

    public Subject() { }
    public Subject(string title, string teacher, int hours)
    {
        SubjectId = _nextId++;
        Title = title;
        Teacher = teacher;
        Hours = hours;
    }
    public string ToFormattedString(string indent = "")
            => $"{indent}{Title} (преподаватель: {Teacher}, часы: {Hours})";

    public override string ToString() => ToFormattedString();
    public void Print() => Console.WriteLine(ToFormattedString());

}

