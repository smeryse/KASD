using System;

namespace ЛР5
{
    /// <summary>
    /// Класс для обработки и уведомления об ошибках различных типов
    /// </summary>
    internal class ErrorNotifier
    {
        /// <summary>
        /// Обработчик ошибок для базового класса Student
        /// </summary>
        public static void HandleStudentError(object sender, StudentEventArgs e)
        {
            Console.WriteLine($"[Ошибка студента] {sender.GetType().Name}: {e.Exception.GetType().Name} - {e.Message}");
        }

        /// <summary>
        /// Обработчик ошибок для производного класса StudentWithErrorHandling
        /// с детальной обработкой различных типов исключений
        /// </summary>
        public static void HandleStudentWithErrorHandling(object sender, StudentEventArgs e)
        {
            string exceptionType = e.Exception.GetType().Name;
            string studentInfo = sender is StudentWithErrorHandling student 
                ? $"{student.Name} {student.Surname} (ID: {student.StudentId})" 
                : sender.GetType().Name;

            Console.WriteLine($"\n[Обработка исключения] Студент: {studentInfo}");
            Console.WriteLine($"Тип исключения: {exceptionType}");
            Console.WriteLine($"Сообщение: {e.Message}");

            // Специфичная обработка для каждого типа исключения
            switch (e.Exception)
            {
                case StackOverflowException _:
                    Console.WriteLine("Действие: Прервана рекурсивная операция");
                    break;
                case ArrayTypeMismatchException _:
                    Console.WriteLine("Действие: Проверьте типы элементов массива");
                    break;
                case DivideByZeroException _:
                    Console.WriteLine("Действие: Проверьте делитель перед делением");
                    break;
                case IndexOutOfRangeException _:
                    Console.WriteLine("Действие: Проверьте индекс перед доступом к массиву");
                    break;
                case InvalidCastException _:
                    Console.WriteLine("Действие: Проверьте совместимость типов перед приведением");
                    break;
                case OutOfMemoryException _:
                    Console.WriteLine("Действие: Освободите память или уменьшите размер данных");
                    break;
                case OverflowException _:
                    Console.WriteLine("Действие: Проверьте диапазон значений перед операцией");
                    break;
                default:
                    Console.WriteLine("Действие: Обратитесь к администратору");
                    break;
            }
            Console.WriteLine();
        }
    }
}
