namespace Task29.Interfaces
{
    public interface IMyEntry<K, V>
    {
        K Key { get; }
        V Value { get; }
        V SetValue(V value);
    }
}
