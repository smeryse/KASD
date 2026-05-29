using System;

namespace Task28
{
    public class MyCollectionException : Exception
    {
        public MyCollectionException() { }
        public MyCollectionException(string message) : base(message) { }
        public MyCollectionException(string message, Exception inner) : base(message, inner) { }
    }

    public class MyNoSuchElementException : MyCollectionException
    {
        public MyNoSuchElementException() : base("No such element") { }
        public MyNoSuchElementException(string message) : base(message) { }
    }

    public class MyIllegalStateException : MyCollectionException
    {
        public MyIllegalStateException() : base("Illegal state") { }
        public MyIllegalStateException(string message) : base(message) { }
    }

    public class MyUnsupportedOperationException : MyCollectionException
    {
        public MyUnsupportedOperationException() : base("Operation not supported") { }
        public MyUnsupportedOperationException(string message) : base(message) { }
    }
}
