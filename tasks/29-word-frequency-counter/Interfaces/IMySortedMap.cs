namespace Task29.Interfaces
{
    public interface IMySortedMap<K, V> : IMyMap<K, V>
    {
        K FirstKey();
        K LastKey();
        IMySortedMap<K, V> HeadMap(K end);
        IMySortedMap<K, V> SubMap(K start, K end);
        IMySortedMap<K, V> TailMap(K start);
    }
}
