using System;
using System.Linq;

namespace ЛР5
{
    
    
    
    
    class Student : global::Student
    {
        
        public event EventHandler<StudentEventArgs> ErrorOccurred;

        
        public void RaiseError(Exception ex)
        {
            ErrorOccurred?.Invoke(this, new StudentEventArgs(ex));
        }

        public Student() : base() { }
        public Student(string name, string surname, int age) 
            : base(name, surname, age) { }

        

        
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