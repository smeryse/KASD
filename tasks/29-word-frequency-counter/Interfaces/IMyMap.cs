namespace Task29.Interfaces
{
    public interface IMyMap<K, V>
    {
        void Clear();
        bool ContainsKey(object key);
        bool ContainsValue(object value);
        V Get(object key);
        bool IsEmpty();
        void Put(K key, V value);
        void PutAll(IMyMap<K, V> m);
        V Remove(object key);
        int Size();
        IMySet<K> KeySet();
        IMyCollection<V> Values();
        IMySet<IMyEntry<K, V>> EntrySet();
    }
}
