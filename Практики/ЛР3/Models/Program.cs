using System;
public interface IPrintable
{
    void Print();
}

class Program
{
    static void Main()
    {
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
                    Institute.PrintInstitute();
                    break;
                case "4":
                    SaveToFile(institute);
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


