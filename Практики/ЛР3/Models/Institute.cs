using System;
using System.Collections.Generic;

class Institute
{
    public string Name { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
    
    public Institute(string name)
    {
        Name = name;
    }

    // Методы
    public void AddCourse(Course course) => Courses.Add(course);
    public void RemoveCourse(int id) => Courses.Remove(FindCourse(id));
    public Course FindCourse(int id)
    {
        Course course = Courses.Find(c => c.CourseId == id);
        if (course == null)
            throw new Exception($"Курс с id={id} не найден");
        else
            return course;
    }
    public void Print()
    {
        Console.WriteLine($"Институт: {Name}");
        foreach (var course in Courses)
            course.Print("  ");
    }
}