using System;
using System.Collections.Generic;

class Institute : IManageable
{
    public string Name { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
    
    public Institute(string name)
    {
        Name = name;
    }

    // Методы
    public void Add<Course>(Course course) => Courses.Add(course);

    public void Edit<Course>(int id)
    {

    }
    public void Remove<Course>(int id) => Courses.Remove(Find(id));
    
    public Course Find<Course>(int id)
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
            course.Print();
    }

}