using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ЛР5
{
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
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string json = File.ReadAllText(path);
            var inst = System.Text.Json.JsonSerializer.Deserialize<Institute>(json, options);

            if (inst == null) return null;

            // вычисляем максимальные ID и ставим следующие значения
            int maxCourseId = inst.Courses.Any() ? inst.Courses.Max(c => c.CourseId) : 0;
            Course.SetNextId(maxCourseId + 1);

            int maxGroupId = inst.Courses.SelectMany(c => c.Groups).Any() ? inst.Courses.SelectMany(c => c.Groups).Max(g => g.GroupId) : 0;
            Group.SetNextId(maxGroupId + 1);

            int maxSubjectId = inst.Courses.SelectMany(c => c.Subjects).Any() ? inst.Courses.SelectMany(c => c.Subjects).Max(s => s.SubjectId) : 0;
            Subject.SetNextId(maxSubjectId + 1);

            int maxStudentId = inst.Courses.SelectMany(c => c.Groups).SelectMany(g => g.Students).Any() ? inst.Courses.SelectMany(c => c.Groups).SelectMany(g => g.Students).Max(s => s.StudentId) : 0;
            Student.SetNextId(maxStudentId + 1);

            return inst;
        }

    }
}