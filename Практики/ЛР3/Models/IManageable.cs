public interface IManageable<T>
{
    void Add(T item);

    void Remove(int id);
    T Find(int id);
    void Print();
}