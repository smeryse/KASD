using System;
using System.Collections.Generic;

class Institute : IPrintable
{
    public string Name { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();

    public Institute(string name)
    {
        Name = name;
        Courses = new List<Course> { };
    }

    // Методы
    public void AddCourse(Course course)
    {
        Courses.Add(course);
    }

    public void RemoveCourse(int courseId)
    {
        Courses.Remove(Courses[courseId]);
    }

    public Course FindCourse(int courseId) => Courses.Find(c => c.CourseId == courseId);
    public void Print()
    {
        Console.WriteLine($"Институт: {Name}");
        foreach (var course in Courses)
            course.Print();
    }
}