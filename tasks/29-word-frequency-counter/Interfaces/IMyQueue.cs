namespace Task29.Interfaces
{
    public interface IMyQueue<T> : IMyCollection<T>
    {
        T Element();
        bool Offer(T obj);
        T Peek();
        T Poll();
    }
}
