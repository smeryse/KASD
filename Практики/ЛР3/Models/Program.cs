using System;
using System.IO;
using System.Linq;

class Program
{
    static Institute institute = new Institute("ИТИ");

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        MainMenu();
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.Clear(); 
            Console.WriteLine("===== МЕНЮ ИНСТИТУТА =====");
            Console.WriteLine("1. Показать структуру");
            Console.WriteLine("2. Добавить курс");
            Console.WriteLine("3. Добавить группу");
            Console.WriteLine("4. Добавить студента");
            Console.WriteLine("5. Добавить предмет");
            Console.WriteLine("6. Добавить оценку студенту");
            Console.WriteLine("7. Сохранить в файл");
            Console.WriteLine("8. Загрузить из файла");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowStructure(); break;
                case "2": AddCourse(); break;
                case "3": AddGroup(); break;
                case "4": AddStudent(); break;
                case "5": AddSubject(); break;
                case "6": AddGrade(); break;
                case "7": SaveToFile(); break;
                case "8": LoadFromFile(); break;
                case "0": return;
                default: Console.WriteLine("Неверный выбор."); break;
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }
    }

    static void ShowStructure()
    {
        Console.Clear();
        Console.WriteLine("=== Текущая структура института ===");
        institute.Print();
    }

    static void AddCourse()
    {
        Console.Write("Введите номер курса: ");
        int number = int.Parse(Console.ReadLine());
        Course course = new Course(number);
        institute.AddCourse(course);
        Console.WriteLine("Курс добавлен.");
    }

    static void AddGroup()
    {
        if (!institute.Courses.Any())
        {
            Console.WriteLine("Нет курсов. Сначала добавьте курс.");
            return;
        }

        Console.Write("Введите номер курса, куда добавить группу: ");
        int courseNumber = int.Parse(Console.ReadLine());
        var course = institute.Courses.FirstOrDefault(c => c.Number == courseNumber);
        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            return;
        }

        Console.Write("Введите название группы: ");
        string name = Console.ReadLine();
        course.AddGroup(new Group(name));
        Console.WriteLine("Группа добавлена.");
    }

    static void AddStudent()
    {
        if (!institute.Courses.Any())
        {
            Console.WriteLine("Нет курсов. Сначала добавьте курс.");
            return;
        }

        Console.Write("Введите номер курса: ");
        int courseNum = int.Parse(Console.ReadLine());
        var course = institute.Courses.FirstOrDefault(c => c.Number == courseNum);
        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            return;
        }

        if (!course.Groups.Any())
        {
            Console.WriteLine("На этом курсе нет групп. Добавьте группу.");
            return;
        }

        Console.Write("Введите имя: ");
        string name = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();
        Console.Write("Введите возраст: ");
        int age = int.Parse(Console.ReadLine());

        Console.Write("Введите название группы: ");
        string groupName = Console.ReadLine();
        var group = course.Groups.FirstOrDefault(g => g.GroupName == groupName);
        if (group == null)
        {
            Console.WriteLine("Группа не найдена.");
            return;
        }

        group.AddStudent(new Student(name, surname, age));
        Console.WriteLine("Студент добавлен.");
    }

    static void AddSubject()
    {
        if (!institute.Courses.Any())
        {
            Console.WriteLine("Нет курсов. Сначала добавьте курс.");
            return;
        }

        Console.Write("Введите номер курса: ");
        int courseNum = int.Parse(Console.ReadLine());
        var course = institute.Courses.FirstOrDefault(c => c.Number == courseNum);
        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            return;
        }

        Console.Write("Введите название предмета: ");
        string title = Console.ReadLine();
        Console.Write("Введите преподавателя: ");
        string teacher = Console.ReadLine();
        Console.Write("Введите количество часов: ");
        int hours = int.Parse(Console.ReadLine());

        course.AddSubject(new Subject(title, teacher, hours));
        Console.WriteLine("Предмет добавлен.");
    }

    static void AddGrade()
    {
        if (!institute.Courses.Any())
        {
            Console.WriteLine("Нет данных. Добавьте курс, группу и студента.");
            return;
        }

        Console.Write("Введите номер курса: ");
        int courseNum = int.Parse(Console.ReadLine());
        var course = institute.Courses.FirstOrDefault(c => c.Number == courseNum);
        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            return;
        }

        Console.Write("Введите название группы: ");
        string groupName = Console.ReadLine();
        var group = course.Groups.FirstOrDefault(g => g.GroupName == groupName);
        if (group == null)
        {
            Console.WriteLine("Группа не найдена.");
            return;
        }

        Console.Write("Введите фамилию студента: ");
        string surname = Console.ReadLine();
        var student = group.Students.FirstOrDefault(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }

        if (!course.Subjects.Any())
        {
            Console.WriteLine("На курсе нет предметов. Добавьте хотя бы один.");
            return;
        }

        Console.Write("Введите название предмета: ");
        string subjectName = Console.ReadLine();
        var subject = course.Subjects.FirstOrDefault(s => s.Title.Equals(subjectName, StringComparison.OrdinalIgnoreCase));
        if (subject == null)
        {
            Console.WriteLine("Предмет не найден.");
            return;
        }

        Console.Write("Введите оценку: ");
        int value = int.Parse(Console.ReadLine());

        // если у студента нет этого предмета — добавляем
        if (student.Grades.All(g => g.Subject.SubjectId != subject.SubjectId))
            student.AddSubject(subject);

        student.AddGrade(subject, value);
        Console.WriteLine("Оценка добавлена.");
    }

    static void SaveToFile()
    {
        string path = "institute.json";
        institute.SaveToFile(path);
        Console.WriteLine($"Данные сохранены в {path}");
    }

    static void LoadFromFile()
    {
        string path = "institute.json";
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        institute = Institute.LoadFromFile(path);
        Console.WriteLine("Данные успешно загружены.");
    }
}
