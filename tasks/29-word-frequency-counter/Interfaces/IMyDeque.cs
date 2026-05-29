namespace Task29.Interfaces
{
    public interface IMyDeque<T> : IMyCollection<T>
    {
        void AddFirst(T obj);
        void AddLast(T obj);
        T GetFirst();
        T GetLast();
        bool OfferFirst(T obj);
        bool OfferLast(T obj);
        T Pop();
        void Push(T obj);
        T PeekFirst();
        T PeekLast();
        T PollFirst();
        T PollLast();
        T RemoveLast();
        T RemoveFirst();
        bool RemoveLastOccurrence(object obj);
        bool RemoveFirstOccurrence(object obj);
    }
}
