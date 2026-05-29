namespace Task29.Interfaces
{
    public interface IMyNavigableMap<K, V> : IMySortedMap<K, V>
    {
        K LowerKey(K key);
        K FloorKey(K key);
        K CeilingKey(K key);
        K HigherKey(K key);
    }
}
