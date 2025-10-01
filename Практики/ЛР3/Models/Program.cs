using System;
using System.IO;
using System.Text;
using ЛР3.Models;

class Program
{
    static void Main()
    {
        // Предметы
        Subject math = new Subject("Математика", "Иванов", 5);
        Subject prog = new Subject("Программирование", "Петров", 6);

        // Студенты
        Student s1 = new Student("Иван Иванов", 19, "12345");
        s1.AddSubject(math);
        s1.AddSubject(prog);

        Student s2 = new Student("Мария Смирнова", 20, "67890");
        s2.AddSubject(prog);

        // Группа
        Group g1 = new Group("ИВТ-21");
        g1.AddStudent(s1);
        g1.AddStudent(s2);

        // Курс
        Course c2 = new Course(2);
        c2.AddGroup(g1);

        // Вывод
        c2.PrintCourse();
        Console.WriteLine();
        s1.PrintSubjects();
        s2.PrintSubjects();
    }
}


