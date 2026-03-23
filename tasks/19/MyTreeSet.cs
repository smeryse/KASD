namespace Task19.Collection
{
    public class MyTreeSet<T> where T : IComparable<T>
    {
        private Node? root;
        private int size;
        private IComparer<T>? comparator;

        private class Node
        {
            public T value;
            public Node? left;
            public Node? right;

            public Node(T value)
            {
                this.value = value;
            }
        }

        public MyTreeSet()
        {
            this.comparator = null;
            this.root = null;
            this.size = 0;
        }

        public MyTreeSet(IComparer<T> comp)
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

        public bool Add(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            var oldSize = size;
            root = AddNode(root, value);
            return oldSize != size;
        }

        private Node? AddNode(Node? node, T value)
        {
            if (node == null)
            {
                size++;
                return new Node(value);
            }

            int cmp = Compare(value, node.value);
            if (cmp < 0)
                node.left = AddNode(node.left, value);
            else if (cmp > 0)
                node.right = AddNode(node.right, value);
            // если cmp == 0, элемент уже есть, не добавляем

            return node;
        }

        public bool Contains(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return GetNode(root, value) != null;
        }

        private Node? GetNode(Node? node, T value)
        {
            while (node != null)
            {
                int cmp = Compare(value, node.value);
                if (cmp < 0)
                    node = node.left;
                else if (cmp > 0)
                    node = node.right;
                else
                    return node;
            }
            return null;
        }

        public bool Remove(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!Contains(value)) return false;
            root = RemoveNode(root, value);
            return true;
        }

        private Node? RemoveNode(Node? node, T value)
        {
            if (node == null) return null;

            int cmp = Compare(value, node.value);
            if (cmp < 0)
                node.left = RemoveNode(node.left, value);
            else if (cmp > 0)
                node.right = RemoveNode(node.right, value);
            else
            {
                if (node.left == null) return node.right;
                if (node.right == null) return node.left;

                Node? successor = FindMin(node.right);
                node.value = successor.value;
                node.right = RemoveNode(node.right, successor.value);
            }
            return node;
        }

        private Node FindMin(Node node)
        {
            while (node.left != null) node = node.left;
            return node;
        }

        public T? First()
        {
            if (root == null) throw new InvalidOperationException("Set is empty");
            Node node = root;
            while (node.left != null) node = node.left;
            return node.value;
        }

        public T? Last()
        {
            if (root == null) throw new InvalidOperationException("Set is empty");
            Node node = root;
            while (node.right != null) node = node.right;
            return node.value;
        }

        public MyTreeSet<T> HeadSet(T end)
        {
            ArgumentNullException.ThrowIfNull(end);
            var result = new MyTreeSet<T> { comparator = this.comparator };
            HeadSetCollect(root, result, end);
            return result;
        }

        private void HeadSetCollect(Node? node, MyTreeSet<T> set, T end)
        {
            if (node == null) return;
            if (Compare(node.value, end) < 0)
            {
                HeadSetCollect(node.left, set, end);
                set.Add(node.value);
                HeadSetCollect(node.right, set, end);
            }
            else
            {
                HeadSetCollect(node.left, set, end);
            }
        }

        public MyTreeSet<T> SubSet(T start, T end)
        {
            ArgumentNullException.ThrowIfNull(start);
            ArgumentNullException.ThrowIfNull(end);
            if (Compare(start, end) >= 0)
                throw new ArgumentException("Start must be less than end");

            var result = new MyTreeSet<T> { comparator = this.comparator };
            SubSetCollect(root, result, start, end);
            return result;
        }

        private void SubSetCollect(Node? node, MyTreeSet<T> set, T start, T end)
        {
            if (node == null) return;
            int cmpStart = Compare(node.value, start);
            int cmpEnd = Compare(node.value, end);

            if (cmpStart >= 0) SubSetCollect(node.left, set, start, end);
            if (cmpStart >= 0 && cmpEnd < 0) set.Add(node.value);
            if (cmpEnd < 0) SubSetCollect(node.right, set, start, end);
        }

        public MyTreeSet<T> TailSet(T start)
        {
            ArgumentNullException.ThrowIfNull(start);
            var result = new MyTreeSet<T> { comparator = this.comparator };
            TailSetCollect(root, result, start);
            return result;
        }

        private void TailSetCollect(Node? node, MyTreeSet<T> set, T start)
        {
            if (node == null) return;
            int cmp = Compare(node.value, start);
            if (cmp > 0)
            {
                TailSetCollect(node.left, set, start);
                set.Add(node.value);
                TailSetCollect(node.right, set, start);
            }
            else
            {
                TailSetCollect(node.right, set, start);
            }
        }

        public T? Lower(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = FindLowerNode(root, value);
            return node != null ? node.value : default;
        }

        private Node? FindLowerNode(Node? node, T value)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp < 0) { result = node; node = node.right; }
                else { node = node.left; }
            }
            return result;
        }

        public T? Floor(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = FindFloorNode(root, value);
            return node != null ? node.value : default;
        }

        private Node? FindFloorNode(Node? node, T value)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp == 0) return node;
                if (cmp < 0) { result = node; node = node.right; }
                else { node = node.left; }
            }
            return result;
        }

        public T? Higher(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = FindHigherNode(root, value);
            return node != null ? node.value : default;
        }

        private Node? FindHigherNode(Node? node, T value)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp > 0) { result = node; node = node.left; }
                else { node = node.right; }
            }
            return result;
        }

        public T? Ceiling(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = FindCeilingNode(root, value);
            return node != null ? node.value : default;
        }

        private Node? FindCeilingNode(Node? node, T value)
        {
            Node? result = null;
            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp == 0) return node;
                if (cmp > 0) { result = node; node = node.left; }
                else { node = node.right; }
            }
            return result;
        }

        public T? PollFirst()
        {
            var first = First();
            if (first != null) Remove(first);
            return first;
        }

        public T? PollLast()
        {
            var last = Last();
            if (last != null) Remove(last);
            return last;
        }

        public HashSet<T> ToHashSet()
        {
            var set = new HashSet<T>();
            CollectValues(root, set);
            return set;
        }

        private void CollectValues(Node? node, HashSet<T> set)
        {
            if (node == null) return;
            CollectValues(node.left, set);
            set.Add(node.value);
            CollectValues(node.right, set);
        }

        private int Compare(T value1, T value2)
        {
            return comparator?.Compare(value1, value2) ?? value1.CompareTo(value2);
        }

        public void Print()
        {
            Console.Write("[ ");
            var values = ToHashSet();
            var sorted = values.ToList();
            sorted.Sort(Compare);
            bool first = true;
            foreach (var val in sorted)
            {
                if (!first) Console.Write(", ");
                Console.Write(val);
                first = false;
            }
            Console.WriteLine(" ]");
        }
    }
}
