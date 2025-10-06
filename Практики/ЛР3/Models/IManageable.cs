public interface IManageable
{
    void Add<T>(T item);
    void Remove<T>(int id);
    T Find<T>(int id);
    void Print();
}
