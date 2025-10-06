using System;
public interface IPrintable
{
    void Print();
}
class Program
{
    static void Main()
    {
        try
        {
            // === 1. Создаём институт ===
            Institute institute = new Institute("ИТИ");

            // === 2. Создаём курс ===
            Course course1 = new Course(2);

            // === 3. Создаём предметы ===
            Subject math = new Subject("Математика", "Иванов", 72);
            Subject prog = new Subject("Программирование", "Петров", 90);
            Subject physics = new Subject("Физика", "Сидоров", 60);

            // Добавляем предметы к курсу
            course1.AddSubject(math);
            course1.AddSubject(prog);
            course1.AddSubject(physics);

            // === 4. Создаём группы ===
            Group group1 = new Group("ИВТ-21");
            Group group2 = new Group("ИВТ-22");

            // === 5. Создаём студентов ===
            Student s1 = new Student("Иванов Иван", 19);
            Student s2 = new Student("Смирнова Анна", 20);
            Student s3 = new Student("Петров Сергей", 19);
            Student s4 = new Student("Кузнецова Мария", 18);

            // Добавляем предметы студентам
            s1.AddSubject(math);
            s1.AddSubject(prog);
            s2.AddSubject(prog);
            s3.AddSubject(physics);
            s4.AddSubject(math);

            // Добавляем студентов в группы
            group1.AddStudent(s1);
            group1.AddStudent(s2);
            group2.AddStudent(s3);
            group2.AddStudent(s4);

            // Добавляем группы к курсу
            course1.AddGroup(group1);
            course1.AddGroup(group2);

            // Добавляем курс к институту
            institute.AddCourse(course1);

            // === 6. Тестируем методы ===

            Console.WriteLine("\n===============================");
            Console.WriteLine("   ПЕЧАТЬ ВСЕЙ СТРУКТУРЫ");
            Console.WriteLine("===============================\n");

            institute.Print();

            Console.WriteLine("\n===============================");
            Console.WriteLine("   УДАЛЯЕМ ОДИН ПРЕДМЕТ");
            Console.WriteLine("===============================\n");

            course1.RemoveSubject(math.SubjectId);
            institute.Print();

            Console.WriteLine("\n===============================");
            Console.WriteLine("   ПРОВЕРКА ИСКЛЮЧЕНИЯ");
            Console.WriteLine("===============================\n");

            try
            {
                course1.RemoveGroup(999); // Несуществующий ID
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            Console.WriteLine("\n===============================");
            Console.WriteLine("   ПРОВЕРКА ДУБЛИРУЮЩЕГО СТУДЕНТА");
            Console.WriteLine("===============================\n");

            try
            {
                group1.AddStudent(s1); // уже добавлен
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка программы: {ex.Message}");
        }

        Console.WriteLine("\n=== Тест завершён ===");
    }
    static void Main1()
    {
        Institute institute = new Institute("ИТИ");

        while (true)
        {
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Удалить студента");
            Console.WriteLine("3. Показать институт");
            Console.WriteLine("4. Сохранить студентов в файл");
            Console.WriteLine("5. Выход");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // ввод данных и добавление студента
                    break;
                case "2":
                    // удаление по Id или имени
                    break;
                case "3":
                    institute.Print();
                    break;
                case "4":
                    SaveToFile(institute);
                    break;
                    break;
                case "5":
                    return; // выход из программы
            }
        }
    }
    static void MainTest()
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


