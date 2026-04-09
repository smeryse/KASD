namespace Task21.Collections
{
    public class MyHashMap<K, V> where K : notnull
    {
        private class Entry
        {
            public K Key { get; }
            public V Value { get; set; }
            public Entry? Next { get; set; }

            public Entry(K key, V value)
            {
                Key = key;
                Value = value;
            }
        }

        private Entry?[] table;
        private int size;
        private float loadFactor;
        private int threshold;

        public MyHashMap() : this(16, 0.75f) { }

        public MyHashMap(int initialCapacity) : this(initialCapacity, 0.75f) { }

        public MyHashMap(int initialCapacity, float loadFactor)
        {
            if (initialCapacity < 1) throw new ArgumentException("Initial capacity must be >= 1");
            if (loadFactor <= 0 || float.IsNaN(loadFactor)) throw new ArgumentException("Load factor must be > 0");

            this.loadFactor = loadFactor;
            threshold = (int)(initialCapacity * loadFactor);
            table = new Entry?[initialCapacity];
            size = 0;
        }

        public int Size => size;
        public bool IsEmpty() => size == 0;

        private int Hash(K key)
        {
            int h = key.GetHashCode();
            return (h ^ (h >> 16)) & (table.Length - 1);
        }

        public V? Get(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            int index = Hash(key);
            Entry? e = table[index];
            while (e != null)
            {
                if (e.Key.Equals(key))
                    return e.Value;
                e = e.Next;
            }
            return default;
        }

        public void Put(K key, V value)
        {
            ArgumentNullException.ThrowIfNull(key);
            int index = Hash(key);
            Entry? e = table[index];

            while (e != null)
            {
                if (e.Key.Equals(key))
                {
                    e.Value = value;
                    return;
                }
                e = e.Next;
            }

            Entry newEntry = new Entry(key, value);
            newEntry.Next = table[index];
            table[index] = newEntry;
            size++;

            if (size > threshold)
                Resize();
        }

        private void Resize()
        {
            int newCapacity = table.Length * 2;
            Entry?[] newTable = new Entry?[newCapacity];
            int newThreshold = (int)(newCapacity * loadFactor);

            for (int i = 0; i < table.Length; i++)
            {
                Entry? e = table[i];
                while (e != null)
                {
                    Entry? next = e.Next;
                    int newIndex = (e.Key.GetHashCode() ^ (e.Key.GetHashCode() >> 16)) & (newCapacity - 1);
                    e.Next = newTable[newIndex];
                    newTable[newIndex] = e;
                    e = next;
                }
            }

            table = newTable;
            threshold = newThreshold;
        }

        public V? Remove(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            int index = Hash(key);
            Entry? prev = null;
            Entry? e = table[index];

            while (e != null)
            {
                if (e.Key.Equals(key))
                {
                    if (prev == null)
                        table[index] = e.Next;
                    else
                        prev.Next = e.Next;
                    size--;
                    return e.Value;
                }
                prev = e;
                e = e.Next;
            }
            return default;
        }

        public bool ContainsKey(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            int index = Hash(key);
            Entry? e = table[index];
            while (e != null)
            {
                if (e.Key.Equals(key)) return true;
                e = e.Next;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            for (int i = 0; i < table.Length; i++)
            {
                Entry? e = table[i];
                while (e != null)
                {
                    if (EqualityComparer<V>.Default.Equals(e.Value, value))
                        return true;
                    e = e.Next;
                }
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < table.Length; i++)
                table[i] = null;
            size = 0;
        }

        public HashSet<K> KeySet()
        {
            var keySet = new HashSet<K>();
            for (int i = 0; i < table.Length; i++)
            {
                Entry? e = table[i];
                while (e != null)
                {
                    keySet.Add(e.Key);
                    e = e.Next;
                }
            }
            return keySet;
        }

        public List<KeyValuePair<K, V>> EntrySet()
        {
            var entries = new List<KeyValuePair<K, V>>();
            for (int i = 0; i < table.Length; i++)
            {
                Entry? e = table[i];
                while (e != null)
                {
                    entries.Add(new KeyValuePair<K, V>(e.Key, e.Value));
                    e = e.Next;
                }
            }
            return entries;
        }

        public void Print()
        {
            Console.WriteLine($"HashMap: size={size}, capacity={table.Length}, loadFactor={loadFactor:F2}");
            for (int i = 0; i < table.Length; i++)
            {
                Entry? e = table[i];
                if (e != null)
                {
                    Console.Write($"  [{i}] -> ");
                    while (e != null)
                    {
                        Console.Write($"({e.Key}={e.Value})");
                        if (e.Next != null) Console.Write(" -> ");
                        e = e.Next;
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
