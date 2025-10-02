using System.Collections.Generic;

class Institute
{
    string Name { get; set; }
    List<Course> Courses { get; set; } = new List<Course>();

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

    public void FindCourse(int courseId)
    {
        Courses.Find(с => c.courseId == courseId);
    }
}