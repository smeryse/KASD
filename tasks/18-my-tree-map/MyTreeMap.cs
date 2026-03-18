namespace Task18.Collection
{
    public class MyTreeMap<K, V> where K : IComparable<K>
    {
        private Node? root;
        private int size;
        private IComparer<K>? comparator;

        private class Node
        {
            public K key;
            public V value;
            public Node? left;
            public Node? right;

            public Node(K key, V value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public MyTreeMap()
        {
            this.comparator = null;
            this.root = null;
            this.size = 0;
        }

        public MyTreeMap(IComparer<K> comp)
        {
            ArgumentNullException.ThrowIfNull(comp);
            this.comparator = comp;
            this.root = null;
            this.size = 0;
        }

        public int Size => size;
        public bool IsEmpty() => size == 0;

        public void Clear()
        {
            root = null;
            size = 0;
        }

        public V? this[K key]
        {
            get => Get(key);
            set => Put(key, value);
        }

        public void Put(K key, V value)
        {
            ArgumentNullException.ThrowIfNull(key);
            root = PutNode(root, key, value);
        }

        private Node PutNode(Node? node, K key, V value)
        {
            if (node == null)
            {
                size++;
                return new Node(key, value);
            }

            int cmp = Compare(key, node.key);
            if (cmp < 0)
                node.left = PutNode(node.left, key, value);
            else if (cmp > 0)
                node.right = PutNode(node.right, key, value);
            else
                node.value = value;

            return node;
        }

        public V? Get(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            Node? node = GetNode(root, key);
            return node != null ? node.value : default(V);
        }

        private Node? GetNode(Node? node, K key)
        {
            while (node != null)
            {
                int cmp = Compare(key, node.key);
                if (cmp < 0)
                    node = node.left;
                else if (cmp > 0)
                    node = node.right;
                else
                    return node;
            }
            return null;
        }

        public bool ContainsKey(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            return GetNode(root, key) != null;
        }

        public bool ContainsValue(V value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return ContainsValueNode(root, value);
        }

        private bool ContainsValueNode(Node? node, V value)
        {
            if (node == null) return false;
            if (Equals(node.value, value)) return true;
            return ContainsValueNode(node.left, value) || ContainsValueNode(node.right, value);
        }

        public V? Remove(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            var oldNode = GetNode(root, key);
            if (oldNode == null) return default;
            root = RemoveNode(root, key);
            return oldNode.value;
        }

        private Node? RemoveNode(Node? node, K key)
        {
            if (node == null) return null;

            int cmp = Compare(key, node.key);
            if (cmp < 0)
                node.left = RemoveNode(node.left, key);
            else if (cmp > 0)
                node.right = RemoveNode(node.right, key);
            else
            {
                if (node.left == null) return node.right;
                if (node.right == null) return node.left;

                Node? successor = FindMin(node.right);
                node.key = successor.key;
                node.value = successor.value;
                node.right = RemoveNode(node.right, successor.key);
            }
            return node;
        }

        private Node FindMin(Node node)
        {
            while (node.left != null) node = node.left;
            return node;
        }

        public K FirstKey()
        {
            if (root == null) throw new InvalidOperationException("Map is empty");
            Node node = root;
            while (node.left != null) node = node.left;
            return node.key;
        }

        public K LastKey()
        {
            if (root == null) throw new InvalidOperationException("Map is empty");
            Node node = root;
            while (node.right != null) node = node.right;
            return node.key;
        }

        public MyTreeMap<K, V> HeadMap(K end)
        {
            ArgumentNullException.ThrowIfNull(end);
            var result = new MyTreeMap<K, V> { comparator = this.comparator };
            HeadMapCollect(root, result, end);
            return result;
        }

        private void HeadMapCollect(Node? node, MyTreeMap<K, V> map, K end)
        {
            if (node == null) return;
            if (Compare(node.key, end) < 0)
            {
                HeadMapCollect(node.left, map, end);
                map.Put(node.key, node.value);
                HeadMapCollect(node.right, map, end);
            }
            else
            {
                HeadMapCollect(node.left, map, end);
            }
        }

        public MyTreeMap<K, V> SubMap(K start, K end)
        {
            ArgumentNullException.ThrowIfNull(start);
            ArgumentNullException.ThrowIfNull(end);
            if (Compare(start, end) >= 0)
                throw new ArgumentException("Start key must be less than end key");

            var result = new MyTreeMap<K, V> { comparator = this.comparator };
            SubMapCollect(root, result, start, end);
            return result;
        }

        private void SubMapCollect(Node? node, MyTreeMap<K, V> map, K start, K end)
        {
            if (node == null) return;
            int cmpStart = Compare(node.key, start);
            int cmpEnd = Compare(node.key, end);

            if (cmpStart >= 0) SubMapCollect(node.left, map, start, end);
            if (cmpStart >= 0 && cmpEnd < 0) map.Put(node.key, node.value);
            if (cmpEnd < 0) SubMapCollect(node.right, map, start, end);
        }

        public MyTreeMap<K, V> TailMap(K start)
        {
            ArgumentNullException.ThrowIfNull(start);
            var result = new MyTreeMap<K, V> { comparator = this.comparator };
            TailMapCollect(root, result, start);
            return result;
        }

        private void TailMapCollect(Node? node, MyTreeMap<K, V> map, K start)
        {
            if (node == null) return;
            int cmp = Compare(node.key, start);
            if (cmp > 0)
            {
                TailMapCollect(node.left, map, start);
                map.Put(node.key, node.value);
                TailMapCollect(node.right, map, start);
            }
            else
            {
                TailMapCollect(node.right, map, start);
            }
        }

        public KeyValuePair<K, V>? LowerEntry(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            Node? node = FindLowerNode(root, key);
            return node != null ? new KeyValuePair<K, V>(node.key, node.value) : null;
        }

        private Node? FindLowerNode(Node? node, K key)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.key, key);
                if (cmp < 0) { result = node; node = node.right; }
                else { node = node.left; }
            }
            return result;
        }

        public KeyValuePair<K, V>? FloorEntry(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            Node? node = FindFloorNode(root, key);
            return node != null ? new KeyValuePair<K, V>(node.key, node.value) : null;
        }

        private Node? FindFloorNode(Node? node, K key)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.key, key);
                if (cmp == 0) return node;
                if (cmp < 0) { result = node; node = node.right; }
                else { node = node.left; }
            }
            return result;
        }

        public KeyValuePair<K, V>? HigherEntry(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            Node? node = FindHigherNode(root, key);
            return node != null ? new KeyValuePair<K, V>(node.key, node.value) : null;
        }

        private Node? FindHigherNode(Node? node, K key)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.key, key);
                if (cmp > 0) { result = node; node = node.left; }
                else { node = node.right; }
            }
            return result;
        }

        public KeyValuePair<K, V>? CeilingEntry(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            Node? node = FindCeilingNode(root, key);
            return node != null ? new KeyValuePair<K, V>(node.key, node.value) : null;
        }

        private Node? FindCeilingNode(Node? node, K key)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.key, key);
                if (cmp == 0) return node;
                if (cmp > 0) { result = node; node = node.left; }
                else { node = node.right; }
            }
            return result;
        }

        public K? LowerKey(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            var entry = LowerEntry(key);
            return entry.HasValue ? entry.Value.Key : default;
        }

        public K? FloorKey(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            var entry = FloorEntry(key);
            return entry.HasValue ? entry.Value.Key : default;
        }

        public K? HigherKey(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            var entry = HigherEntry(key);
            return entry.HasValue ? entry.Value.Key : default;
        }

        public K? CeilingKey(K key)
        {
            ArgumentNullException.ThrowIfNull(key);
            var entry = CeilingEntry(key);
            return entry.HasValue ? entry.Value.Key : default;
        }

        public KeyValuePair<K, V>? FirstEntry()
        {
            if (root == null) return null;
            Node node = root;
            while (node.left != null) node = node.left;
            return new KeyValuePair<K, V>(node.key, node.value);
        }

        public KeyValuePair<K, V>? LastEntry()
        {
            if (root == null) return null;
            Node node = root;
            while (node.right != null) node = node.right;
            return new KeyValuePair<K, V>(node.key, node.value);
        }

        public KeyValuePair<K, V>? PollFirstEntry()
        {
            var entry = FirstEntry();
            if (entry.HasValue) Remove(entry.Value.Key);
            return entry;
        }

        public KeyValuePair<K, V>? PollLastEntry()
        {
            var entry = LastEntry();
            if (entry.HasValue) Remove(entry.Value.Key);
            return entry;
        }

        public HashSet<K> KeySet()
        {
            var set = new HashSet<K>();
            CollectKeys(root, set);
            return set;
        }

        private void CollectKeys(Node? node, HashSet<K> set)
        {
            if (node == null) return;
            CollectKeys(node.left, set);
            set.Add(node.key);
            CollectKeys(node.right, set);
        }

        public HashSet<KeyValuePair<K, V>> EntrySet()
        {
            var set = new HashSet<KeyValuePair<K, V>>();
            CollectEntries(root, set);
            return set;
        }

        private void CollectEntries(Node? node, HashSet<KeyValuePair<K, V>> set)
        {
            if (node == null) return;
            CollectEntries(node.left, set);
            set.Add(new KeyValuePair<K, V>(node.key, node.value));
            CollectEntries(node.right, set);
        }

        private int Compare(K key1, K key2)
        {
            return comparator?.Compare(key1, key2) ?? key1.CompareTo(key2);
        }

        public void Print()
        {
            Console.Write("[ ");
            var entries = EntrySet();
            bool first = true;
            foreach (var entry in entries)
            {
                if (!first) Console.Write(", ");
                Console.Write($"{entry.Key}={entry.Value}");
                first = false;
            }
            Console.WriteLine(" ]");
        }
    }
}