using System;
using Task29.Interfaces;

namespace Task29
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task 29 - Interface Hierarchy");
            Console.WriteLine();
            Console.WriteLine("Defined interfaces:");
            Console.WriteLine("  IMyCollection<T>");
            Console.WriteLine("    IMyList<T>");
            Console.WriteLine("    IMyQueue<T>");
            Console.WriteLine("    IMyDeque<T>");
            Console.WriteLine("    IMySet<T>");
            Console.WriteLine("      IMySortedSet<T>");
            Console.WriteLine("        IMyNavigableSet<T>");
            Console.WriteLine("  IMyEntry<K, V>");
            Console.WriteLine("  IMyMap<K, V>");
            Console.WriteLine("    IMySortedMap<K, V>");
            Console.WriteLine("      IMyNavigableMap<K, V>");
            Console.WriteLine();
            Console.WriteLine("Interface hierarchy created successfully.");
        }
    }
}
