using System;
using System.Collections.Generic;

class Institute
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

    public Course FindCourse(int courseId)
    {
        return Courses.Find(c => c.CourseId == courseId);
    }

    public void PrintInstitute()
    {
        Console.WriteLine($"Институт: {0}", Name);
        foreach (var course in Courses)
        {
            Console.WriteLine($"Курс: {0}");
            foreach (var group in course.Groups)
            {
                Console.WriteLine($"Группа: {0}", group.GroupName);
                foreach (var student in group.Students)
                {
                    Console.Write(student.Name);
                }
            }
        }
    }
}