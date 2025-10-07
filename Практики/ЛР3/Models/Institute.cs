using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
    public string ToFormattedString(string indent = "")
    {
        string result = $"Институт: {Name}\n";
        if (Courses.Count == 0)
            return result + "  └─ Нет курсов\n";

        foreach (var course in Courses)
            result += course.ToFormattedString("  ");
        return result;
    }

    public override string ToString() => ToFormattedString();
    public void Print() => Console.Write(ToFormattedString());

    // JSON SAVE/LOAD
    public void SaveToFile(string path)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(this, options));
    }

    public static Institute LoadFromFile(string path)
    {
        string json = File.ReadAllText(path);
        return System.Text.Json.JsonSerializer.Deserialize<Institute>(json);
    }
}