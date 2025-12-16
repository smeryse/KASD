using System;
using System.Collections.Generic;
using System.Linq;

namespace ЛР5
{
    /// <summary>
    /// Производный класс Student, переопределяющий событие ErrorOccurred через наследование
    /// и реализующий обработку 7 типов исключений
    /// </summary>
    class StudentWithErrorHandling : Student
    {
        // Переопределение события через ключевое слово new
        public new event EventHandler<StudentEventArgs> ErrorOccurred;

        // Публичный метод для вызова переопределенного события
        public new void RaiseError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, new StudentEventArgs(ex));
        }

        public StudentWithErrorHandling() : base() { }
        public StudentWithErrorHandling(string name, string surname, int age) 
            : base(name, surname, age) { }

        // === Методы для генерации и обработки исключений ===

        /// <summary>
        /// Генерация StackOverflowException через рекурсию
        /// </summary>
        public void TestStackOverflow()
        {
            try
            {
                // StackOverflowException обычно не может быть пойман в .NET,
                // поэтому создаём его явно для демонстрации
                // В реальном приложении это исключение обычно приводит к завершению программы
                throw new StackOverflowException("Переполнение стека при рекурсивном вызове");
            }
            catch (StackOverflowException ex)
            {
                RaiseError(ex);
            }
            catch (Exception ex)
            {
                // Если по какой-то причине не поймался, создаём его явно
                RaiseError(new StackOverflowException("Переполнение стека при рекурсивном вызове", ex));
            }
        }

        /// <summary>
        /// Генерация ArrayTypeMismatchException
        /// </summary>
        public void TestArrayTypeMismatch()
        {
            try
            {
                int[] intArray = new int[5];
                Array objArray = intArray;
                // Попытка установить строку в массив целых чисел
                objArray.SetValue("string", 0);
            }
            catch (ArrayTypeMismatchException ex)
            {
                RaiseError(ex);
            }
        }

        /// <summary>
        /// Генерация DivideByZeroException
        /// </summary>
        public void TestDivideByZero()
        {
            try
            {
                int numerator = 10;
                int denominator = 0;
                int result = numerator / denominator;
            }
            catch (DivideByZeroException ex)
            {
                RaiseError(ex);
            }
        }

        /// <summary>
        /// Генерация IndexOutOfRangeException
        /// </summary>
        public void TestIndexOutOfRange()
        {
            try
            {
                int[] array = new int[5];
                // Попытка доступа к несуществующему индексу
                int value = array[10];
            }
            catch (IndexOutOfRangeException ex)
            {
                RaiseError(ex);
            }
        }

        /// <summary>
        /// Генерация InvalidCastException
        /// </summary>
        public void TestInvalidCast()
        {
            try
            {
                object obj = "Это строка";
                // Попытка приведения строки к целому числу
                int number = (int)obj;
            }
            catch (InvalidCastException ex)
            {
                RaiseError(ex);
            }
        }

        /// <summary>
        /// Генерация OutOfMemoryException
        /// </summary>
        public void TestOutOfMemory()
        {
            try
            {
                // Попытка выделить слишком большой массив
                // В реальности это может не сработать, поэтому создаём исключение явно
                long[] hugeArray = new long[int.MaxValue];
            }
            catch (OutOfMemoryException ex)
            {
                RaiseError(ex);
            }
            catch (Exception)
            {
                // Если реальное исключение не произошло, создаём его явно
                RaiseError(new OutOfMemoryException("Недостаточно памяти для выделения массива"));
            }
        }

        /// <summary>
        /// Генерация OverflowException
        /// </summary>
        public void TestOverflow()
        {
            try
            {
                checked
                {
                    int maxValue = int.MaxValue;
                    // Попытка переполнения при сложении
                    int overflow = maxValue + 1;
                }
            }
            catch (OverflowException ex)
            {
                RaiseError(ex);
            }
        }

        /// <summary>
        /// Метод для запуска всех тестов исключений
        /// </summary>
        public void RunAllExceptionTests()
        {
            Console.WriteLine("=== Запуск тестов исключений ===");
            
            Console.WriteLine("\n1. Тест StackOverflowException...");
            TestStackOverflow();
            
            Console.WriteLine("\n2. Тест ArrayTypeMismatchException...");
            TestArrayTypeMismatch();
            
            Console.WriteLine("\n3. Тест DivideByZeroException...");
            TestDivideByZero();
            
            Console.WriteLine("\n4. Тест IndexOutOfRangeException...");
            TestIndexOutOfRange();
            
            Console.WriteLine("\n5. Тест InvalidCastException...");
            TestInvalidCast();
            
            Console.WriteLine("\n6. Тест OutOfMemoryException...");
            TestOutOfMemory();
            
            Console.WriteLine("\n7. Тест OverflowException...");
            TestOverflow();
            
            Console.WriteLine("\n=== Все тесты завершены ===");
        }
    }
}

