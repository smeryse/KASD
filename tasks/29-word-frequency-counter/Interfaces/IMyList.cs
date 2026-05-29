namespace Task29.Interfaces
{
    public interface IMyList<T> : IMyCollection<T>
    {
        void Add(int index, T e);
        void AddAll(int index, T[] a);
        T Get(int index);
        int IndexOf(object o);
        int LastIndexOf(object o);
        T RemoveAt(int index);
        T Set(int index, T e);
        IMyList<T> SubList(int fromIndex, int toIndex);
    }
}
