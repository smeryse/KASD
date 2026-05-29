namespace Task29.Interfaces
{
    public interface IMyNavigableSet<T> : IMySortedSet<T>
    {
        T Lower(T e);
        T Floor(T e);
        T Ceiling(T e);
        T Higher(T e);
        T PollFirst();
        T PollLast();
    }
}
