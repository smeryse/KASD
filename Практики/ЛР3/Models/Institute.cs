using System;
using System.Collections.Generic;

class Institute : IPrintable
{
    public string Name { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();

    public Institute(string name)
    {
        Name = name;
    }

    // Методы
    public void AddCourse(Course course) => Courses.Add(course);

    // Переписать
    public void RemoveCourse(int courseId)
    {
        Course course = FindCourse(courseId);
        if (course == null)
            trow
        else
            Courses.Remove(course);
    }

    public Course FindCourse(int courseId)
    {
        return Courses.Find(c => c.CourseId == courseId);
    }
    public void Print()
    {
        Console.WriteLine($"Институт: {Name}");
        foreach (var course in Courses)
            course.Print();
    }
}