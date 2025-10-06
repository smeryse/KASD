using System;
public interface IManageable<T>
{
    void Add(T item);
    void Remove(int id);
    void Edit(int id);
    void Print();
}

class Program
{
    static void Main()
    {
        Institute institute = new Institute("ИТИ");

        while (true)
        {
            Console.WriteLine(
                "1. Добавить студента\n" +
                "2. Удалить студента\n" +
                "3. Показать институт\n" +
                "4. Сохранить студентов в файл\n" +
                "5. Выход\n" +
                "Выберите действие: ");

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
}


