namespace Task28
{
    public interface IMyIterator<T>
    {
        bool HasNext();
        T Next();
        void Remove();
    }

    public interface IMyListIterator<T> : IMyIterator<T>
    {
        bool HasPrevious();
        T Previous();
        int NextIndex();
        int PreviousIndex();
        void Set(T element);
        void Add(T element);
    }
}
