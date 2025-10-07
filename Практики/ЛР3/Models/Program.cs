using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // чтобы дерево и русские символы отображались корректно

        // === 1. Создание института ===
        Institute institute = new Institute("ИТИ");

        // === 2. Добавляем курс ===
        Course course1 = new Course(1);
        institute.AddCourse(course1);

        // === 3. Добавляем предметы ===
        Subject math = new Subject("Математика", "Иванова", 60);
        Subject prog = new Subject("Программирование", "Петров", 90);
        Subject phys = new Subject("Физика", "Сидоров", 45);

        course1.AddSubject(math);
        course1.AddSubject(prog);
        course1.AddSubject(phys);

        // === 4. Добавляем группу ===
        Group groupA = new Group("А1");
        course1.AddGroup(groupA);

        // === 5. Добавляем студентов ===
        Student ivan = new Student("Иван", "Иванов", 19);
        Student anna = new Student("Анна", "Смирнова", 20);
        groupA.AddStudent(ivan);
        groupA.AddStudent(anna);

        // === 6. Назначаем предметы студентам ===
        ivan.AddSubject(math);
        ivan.AddSubject(prog);
        ivan.AddSubject(phys);

        anna.AddSubject(math);
        anna.AddSubject(prog);

        // === 7. Добавляем оценки ===
        ivan.AddGrade(math, 5);
        ivan.AddGrade(math, 4);
        ivan.AddGrade(prog, 5);
        ivan.AddGrade(prog, 3);
        ivan.AddGrade(phys, 4);
        ivan.AddGrade(phys, 5);

        anna.AddGrade(math, 3);
        anna.AddGrade(math, 4);
        anna.AddGrade(prog, 5);
        anna.AddGrade(prog, 5);

        // === 8. Вывод структуры ===
        Console.WriteLine("=== Исходная структура ===");
        institute.Print();

        // === 9. Сохранение в файл ===
        string jsonPath = "institute.json";
        string textPath = "institute.txt";

        institute.SaveToFile(jsonPath);
        System.IO.File.WriteAllText(textPath, institute.ToString());

        Console.WriteLine($"\nДанные сохранены в файлы:\n - {jsonPath}\n - {textPath}");

        // === 10. Загрузка обратно ===
        Console.WriteLine("\n=== Загрузка из JSON ===");
        Institute loaded = Institute.LoadFromFile(jsonPath);
        loaded.Print();

        // === 11. Проверка отдельного студента ===
        Console.WriteLine("\n=== Проверка индивидуального студента ===");
        Console.WriteLine(ivan.ToString());

        Console.WriteLine("\n=== Программа завершена ===");
    }
}
