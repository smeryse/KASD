using System;

namespace ЛР5
{
    public class StudentEventArgs : EventArgs
    {
        public Exception Exception { get; }
        public string Message => Exception.Message;

        public StudentEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
}