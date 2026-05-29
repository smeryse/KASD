namespace Task29.Interfaces
{
    public interface IMySortedSet<T> : IMySet<T>
    {
        new T First();
        new T Last();
        new IMySortedSet<T> SubSet(T fromElement, T toElement);
        new IMySortedSet<T> HeadSet(T toElement);
        new IMySortedSet<T> TailSet(T fromElement);
    }
}
