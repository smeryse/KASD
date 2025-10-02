using System;


class Subject : IPrintable
{
    public int SubjectId { get; set; }
    public string Title { get; set; }
    public string Teacher { get; set; }
    public int Hours { get; set; }

    public Subject(string title, string teacher, int hours)
    {
        Title = title;
        Teacher = teacher;
        Hours = hours;
    }
        public void Print()
    {
        Console.WriteLine($"Предмет: {Title}, преподаватель: {Teacher}, часы: {Hours}");
    }
}

}
