using System;
using System.Linq;

namespace ЛР5
{
    /// <summary>
    /// Класс Student, наследуемый от базового Student из Lab 4
    /// Добавляет функциональность событий для обработки ошибок
    /// </summary>
    class Student : global::Student
    {
        // === Событие ошибок ===
        public event EventHandler<StudentEventArgs> ErrorOccurred;

        // Публичный метод для вызова события
        public void RaiseError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, new StudentEventArgs(ex));
        }

        public Student() : base() { }
        public Student(string name, string surname, int age) 
            : base(name, surname, age) { }

        // Переопределяем методы базового класса для добавления обработки ошибок через события

        // Добавление предмета
        public new void AddSubject(global::Subject subject)
        {
            try
            {
                base.AddSubject(subject);
            }
            catch (Exception ex) 
            { 
                RaiseError(ex); 
            }
        }

        // Удаление предмета
        public new void RemoveSubject(int subjectId)
        {
            try
            {
                base.RemoveSubject(subjectId);
            }
            catch (Exception ex) 
            { 
                RaiseError(ex); 
            }
        }

        // Добавление оценки
        public new void AddGrade(global::Subject subject, int score)
        {
            try
            {
                base.AddGrade(subject, score);
            }
            catch (Exception ex) 
            { 
                RaiseError(ex); 
            }
        }

        // Удаление оценки
        public new void RemoveGrade(global::Subject subject, int score)
        {
            try
            {
                base.RemoveGrade(subject, score);
            }
            catch (Exception ex) 
            { 
                RaiseError(ex); 
            }
        }

        // Поиск оценок по предмету
        public new global::Grade FindGrades(global::Subject subject)
        {
            try
            {
                return base.FindGrades(subject);
            }
            catch (Exception ex) 
            { 
                RaiseError(ex); 
                return null; 
            }
        }
    }
}