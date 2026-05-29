using System;
using System.Collections.Generic;
using System.Linq;

namespace ЛР5
{
    
    
    
    
    class StudentWithErrorHandling : Student
    {
        
        public new event EventHandler<StudentEventArgs> ErrorOccurred;

        
        public new void RaiseError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, new StudentEventArgs(ex));
        }

        public StudentWithErrorHandling() : base() { }
        public StudentWithErrorHandling(string name, string surname, int age) 
            : base(name, surname, age) { }

        

        
        
        
        public void TestStackOverflow()
        {
            try
            {
                
                
                
                throw new StackOverflowException("Переполнение стека при рекурсивном вызове");
            }
            catch (StackOverflowException ex)
            {
                RaiseError(ex);
            }
            catch (Exception ex)
            {
                
                RaiseError(new StackOverflowException("Переполнение стека при рекурсивном вызове", ex));
            }
        }

        
        
        
        public void TestArrayTypeMismatch()
        {
            try
            {
                int[] intArray = new int[5];
                Array objArray = intArray;
                
                objArray.SetValue("string", 0);
            }
            catch (ArrayTypeMismatchException ex)
            {
                RaiseError(ex);
            }
        }

        
        
        
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

        
        
        
        public void TestIndexOutOfRange()
        {
            try
            {
                int[] array = new int[5];
                
                int value = array[10];
            }
            catch (IndexOutOfRangeException ex)
            {
                RaiseError(ex);
            }
        }

        
        
        
        public void TestInvalidCast()
        {
            try
            {
                object obj = "Это строка";
                
                int number = (int)obj;
            }
            catch (InvalidCastException ex)
            {
                RaiseError(ex);
            }
        }

        
        
        
        public void TestOutOfMemory()
        {
            try
            {
                
                
                long[] hugeArray = new long[int.MaxValue];
            }
            catch (OutOfMemoryException ex)
            {
                RaiseError(ex);
            }
            catch (Exception)
            {
                
                RaiseError(new OutOfMemoryException("Недостаточно памяти для выделения массива"));
            }
        }

        
        
        
        public void TestOverflow()
        {
            try
            {
                checked
                {
                    int maxValue = int.MaxValue;
                    
                    int overflow = maxValue + 1;
                }
            }
            catch (OverflowException ex)
            {
                RaiseError(ex);
            }
        }

        
        
        
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

