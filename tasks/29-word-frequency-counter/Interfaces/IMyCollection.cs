namespace Task29.Interfaces
{
    public interface IMyCollection<T>
    {
        void Add(T e);
        void AddAll(T[] a);
        void Clear();
        bool Contains(object o);
        bool ContainsAll(T[] a);
        bool IsEmpty();
        bool Remove(object o);
        void RemoveAll(T[] a);
        void RetainAll(T[] a);
        int Size();
        object[] ToArray();
        T[] ToArray(T[] a);
    }
}
