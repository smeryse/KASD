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
    public void RemoveCourse(int courseId)
    {
        Course course = FindCourse(courseId);
        Courses.Remove(course);
    }
    public Course FindCourse(int courseId)
    {
        Course course = Courses.Find(c => c.CourseId == courseId);
        if (course == null)
            throw new Exception($"Курс с id={courseId} не найден");
        else
            return course;
    }
    public void Print()
    {
        Console.WriteLine($"Институт: {Name}");
        foreach (var course in Courses)
            course.Print();
    }
}