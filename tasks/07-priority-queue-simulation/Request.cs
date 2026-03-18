using System;
using System.IO;
using Task6.Collections;

namespace Task7
{
    public class Request : IComparable<Request>
    {
        public int Priority { get; set; }
        public int Number { get; set; }
        public int StepAdded { get; set; }
        public int StepRemoved { get; set; } = -1;

        public int CompareTo(Request? other)
        {
            if (other == null) return 1;
            int pCmp = this.Priority.CompareTo(other.Priority);
            if (pCmp != 0) return pCmp;
            return other.Number.CompareTo(this.Number);
        }
    }
}
