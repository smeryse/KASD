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
            Console.WriteLine("2. Добавить данные");
            Console.WriteLine("3. Редактировать данные");
            Console.WriteLine("4. Удалить данные");
            Console.WriteLine("5. Сохранить в файл");
            Console.WriteLine("6. Загрузить из файла");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": ShowStructure(); break;
                case "2": AddMenu(); break;
                case "3": EditMenu(); break;
                case "4": DeleteMenu(); break;
                case "5": SaveToFile(); break;
                case "6": LoadFromFile(); break;
                case "0": return;
                default: Console.WriteLine("Неверный выбор."); break;
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }
    }

    // === 1. Показать структуру ===
    static void ShowStructure()
    {
        Console.Clear();
        Console.WriteLine("=== Текущая структура института ===");
        institute.Print();
    }

    // === 2. Добавление ===
    static void AddMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Добавление ===");
        Console.WriteLine("1. Курс");
        Console.WriteLine("2. Группа");
        Console.WriteLine("3. Студент");
        Console.WriteLine("4. Предмет");
        Console.WriteLine("5. Оценка");
        Console.Write("Выберите тип: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": AddCourse(); break;
            case "2": AddGroup(); break;
            case "3": AddStudent(); break;
            case "4": AddSubject(); break;
            case "5": AddGrade(); break;
            default: Console.WriteLine("Неверный выбор."); break;
        }
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

        Console.Write("Введите номер курса: ");
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

        Console.Write("Введите имя: ");
        string name = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();
        Console.Write("Введите возраст: ");
        int age = int.Parse(Console.ReadLine());

        group.AddStudent(new Student(name, surname, age));
        Console.WriteLine("Студент добавлен.");
    }

    static void AddSubject()
    {
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
        Console.Write("Введите фамилию студента: ");
        string surname = Console.ReadLine();

        var student = institute.Courses
            .SelectMany(c => c.Groups)
            .SelectMany(g => g.Students)
            .FirstOrDefault(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));

        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }

        Console.Write("Введите название предмета: ");
        string subj = Console.ReadLine();
        var subject = institute.Courses.SelectMany(c => c.Subjects)
            .FirstOrDefault(s => s.Title.Equals(subj, StringComparison.OrdinalIgnoreCase));

        if (subject == null)
        {
            Console.WriteLine("Предмет не найден.");
            return;
        }

        Console.Write("Введите оценку: ");
        int val = int.Parse(Console.ReadLine());

        if (student.Grades.All(g => g.Subject.SubjectId != subject.SubjectId))
            student.AddSubject(subject);
        student.AddGrade(subject, val);

        Console.WriteLine("Оценка добавлена.");
    }

    // === 3. Редактирование ===
    static void EditMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Редактирование ===");
        Console.WriteLine("1. Студента");
        Console.WriteLine("2. Предмета");
        Console.WriteLine("3. Группы");
        Console.Write("Выберите тип: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": EditStudent(); break;
            case "2": EditSubject(); break;
            case "3": EditGroup(); break;
            default: Console.WriteLine("Неверный выбор."); break;
        }
    }

    static void EditStudent()
    {
        Console.Write("Введите фамилию студента: ");
        string surname = Console.ReadLine();

        var student = institute.Courses
            .SelectMany(c => c.Groups)
            .SelectMany(g => g.Students)
            .FirstOrDefault(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));

        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }

        Console.Write("Введите новое имя (или Enter, чтобы пропустить): ");
        string name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
            student.Name = name;

        Console.Write("Введите новую фамилию (или Enter, чтобы пропустить): ");
        string newSurname = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newSurname))
            student.Surname = newSurname;

        Console.Write("Введите новый возраст (или Enter): ");
        string ageStr = Console.ReadLine();
        if (int.TryParse(ageStr, out int newAge))
            student.Age = newAge;

        Console.WriteLine("Изменения сохранены.");
    }

    static void EditSubject()
    {
        Console.Write("Введите название предмета: ");
        string title = Console.ReadLine();

        var subject = institute.Courses.SelectMany(c => c.Subjects)
            .FirstOrDefault(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (subject == null)
        {
            Console.WriteLine("Предмет не найден.");
            return;
        }

        Console.Write("Введите новое название (или Enter): ");
        string newTitle = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newTitle))
            subject.Title = newTitle;

        Console.Write("Введите нового преподавателя (или Enter): ");
        string teacher = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(teacher))
            subject.Teacher = teacher;

        Console.Write("Введите новые часы (или Enter): ");
        string hStr = Console.ReadLine();
        if (int.TryParse(hStr, out int h))
            subject.Hours = h;

        Console.WriteLine("Изменения сохранены.");
    }

    static void EditGroup()
    {
        Console.Write("Введите название группы: ");
        string name = Console.ReadLine();

        var group = institute.Courses.SelectMany(c => c.Groups)
            .FirstOrDefault(g => g.GroupName.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (group == null)
        {
            Console.WriteLine("Группа не найдена.");
            return;
        }

        Console.Write("Введите новое название: ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            group.GroupName = newName;

        Console.WriteLine("Название группы изменено.");
    }

    // === 4. Удаление ===
    static void DeleteMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Удаление ===");
        Console.WriteLine("1. Курс");
        Console.WriteLine("2. Группа");
        Console.WriteLine("3. Студент");
        Console.WriteLine("4. Предмет");
        Console.WriteLine("5. Оценку");
        Console.Write("Выберите тип: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": DeleteCourse(); break;
            case "2": DeleteGroup(); break;
            case "3": DeleteStudent(); break;
            case "4": DeleteSubject(); break;
            case "5": DeleteGrade(); break;
            default: Console.WriteLine("Неверный выбор."); break;
        }
    }

    static void DeleteCourse()
    {
        Console.Write("Введите номер курса: ");
        int number = int.Parse(Console.ReadLine());
        var course = institute.Courses.FirstOrDefault(c => c.Number == number);
        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            return;
        }

        institute.Courses.Remove(course);
        Console.WriteLine("Курс удалён.");
    }

    static void DeleteGroup()
    {
        Console.Write("Введите название группы: ");
        string name = Console.ReadLine();
        var group = institute.Courses.SelectMany(c => c.Groups)
            .FirstOrDefault(g => g.GroupName.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (group == null)
        {
            Console.WriteLine("Группа не найдена.");
            return;
        }

        foreach (var c in institute.Courses)
            c.Groups.Remove(group);

        Console.WriteLine("Группа удалена.");
    }

    static void DeleteStudent()
    {
        Console.Write("Введите фамилию студента: ");
        string surname = Console.ReadLine();

        var group = institute.Courses.SelectMany(c => c.Groups)
            .FirstOrDefault(g => g.Students.Any(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase)));

        if (group == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }

        var student = group.Students.First(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
        group.Students.Remove(student);

        Console.WriteLine("Студент удалён.");
    }

    static void DeleteSubject()
    {
        Console.Write("Введите название предмета: ");
        string title = Console.ReadLine();

        foreach (var course in institute.Courses)
        {
            var subj = course.Subjects.FirstOrDefault(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (subj != null)
            {
                course.Subjects.Remove(subj);
                Console.WriteLine("Предмет удалён.");
                return;
            }
        }

        Console.WriteLine("Предмет не найден.");
    }

    static void DeleteGrade()
    {
        Console.Write("Введите фамилию студента: ");
        string surname = Console.ReadLine();
        Console.Write("Введите предмет: ");
        string title = Console.ReadLine();
        Console.Write("Введите оценку: ");
        int val = int.Parse(Console.ReadLine());

        var student = institute.Courses.SelectMany(c => c.Groups)
            .SelectMany(g => g.Students)
            .FirstOrDefault(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));

        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }

        var grade = student.Grades.FirstOrDefault(g => g.Subject.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (grade == null)
        {
            Console.WriteLine("Такого предмета нет у студента.");
            return;
        }

        if (grade.Scores.Remove(val))
            Console.WriteLine("Оценка удалена.");
        else
            Console.WriteLine("Такая оценка не найдена.");
    }

    // === 5. Сохранение / загрузка - File/Directory IO ===
    static void SaveToFile()
    {
        Console.Write("Введите путь к файлу (или Enter для institute.json): ");
        string input = Console.ReadLine();
        string path = string.IsNullOrWhiteSpace(input) ? "institute.json" : input;
        
        try
        {
            institute.SaveToFile(path);
            Console.WriteLine($"Данные сохранены в {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
        }
    }

    static void LoadFromFile()
    {
        Console.Write("Введите путь к файлу (или Enter для institute.json): ");
        string input = Console.ReadLine();
        string path = string.IsNullOrWhiteSpace(input) ? "institute.json" : input;
        
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        try
        {
            institute = Institute.LoadFromFile(path);
            Console.WriteLine("Данные успешно загружены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
        }
    }
}

