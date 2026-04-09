namespace Task19.Collection
{
    public class MyTreeSet<T> where T : IComparable<T>
    {
        private Node? root;
        private int count;
        private IComparer<T>? comparer;

        private enum Color { Red, Black }

        private class Node
        {
            public T value;
            public Color color = Color.Red;
            public Node? parent;
            public Node? left;
            public Node? right;

            public Node(T value, Node? parent = null)
            {
                this.value = value;
                this.parent = parent;
            }
        }

        public MyTreeSet()
        {
            this.comparer = null;
            this.root = null;
            this.count = 0;
        }

        public MyTreeSet(IComparer<T> comp)
        {
            ArgumentNullException.ThrowIfNull(comp);
            this.comparer = comp;
            this.root = null;
            this.count = 0;
        }

        public MyTreeSet(T[] elements) : this()
        {
            ArgumentNullException.ThrowIfNull(elements);
            foreach (var e in elements)
                Add(e);
        }

        public int Size => count;
        public bool IsEmpty() => count == 0;

        public void Clear()
        {
            root = null;
            count = 0;
        }

        public bool Add(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            
            if (root == null)
            {
                root = new Node(value) { color = Color.Black };
                count++;
                return true;
            }

            Node? parent = null;
            Node current = root;
            int cmp = 0;

            while (current != null)
            {
                parent = current;
                cmp = Compare(value, current.value);
                if (cmp < 0)
                    current = current.left;
                else if (cmp > 0)
                    current = current.right;
                else
                    return false;
            }

            Node newNode = new Node(value, parent);
            if (cmp < 0)
                parent.left = newNode;
            else
                parent.right = newNode;

            FixAfterInsertion(newNode);
            count++;
            return true;
        }

        private void FixAfterInsertion(Node x)
        {
            while (x != root && x.parent.color == Color.Red)
            {
                if (x.parent == x.parent.parent.left)
                {
                    Node? y = x.parent.parent.right;
                    if (y != null && y.color == Color.Red)
                    {
                        x.parent.color = Color.Black;
                        y.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.right)
                        {
                            x = x.parent;
                            RotateLeft(x);
                        }
                        x.parent.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        RotateRight(x.parent.parent);
                    }
                }
                else
                {
                    Node? y = x.parent.parent.left;
                    if (y != null && y.color == Color.Red)
                    {
                        x.parent.color = Color.Black;
                        y.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        x = x.parent.parent;
                    }
                    else
                    {
                        if (x == x.parent.left)
                        {
                            x = x.parent;
                            RotateRight(x);
                        }
                        x.parent.color = Color.Black;
                        x.parent.parent.color = Color.Red;
                        RotateLeft(x.parent.parent);
                    }
                }
            }
            root.color = Color.Black;
        }

        private void RotateLeft(Node p)
        {
            Node r = p.right!;
            p.right = r.left;
            if (r.left != null)
                r.left.parent = p;

            r.parent = p.parent;
            if (p.parent == null)
                root = r;
            else if (p == p.parent.left)
                p.parent.left = r;
            else
                p.parent.right = r;

            r.left = p;
            p.parent = r;
        }

        private void RotateRight(Node p)
        {
            Node l = p.left!;
            p.left = l.right;
            if (l.right != null)
                l.right.parent = p;

            l.parent = p.parent;
            if (p.parent == null)
                root = l;
            else if (p == p.parent.right)
                p.parent.right = l;
            else
                p.parent.left = l;

            l.right = p;
            p.parent = l;
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
            Node? target = GetNode(root, value);
            if (target == null) return false;
            
            RemoveNode(target);
            count--;
            return true;
        }

        private void RemoveNode(Node z)
        {
            Node y = z;
            Node x;
            Color yOriginalColor = y.color;

            if (z.left == null)
            {
                x = z.right!;
                Transplant(z, z.right);
            }
            else if (z.right == null)
            {
                x = z.left!;
                Transplant(z, z.left);
            }
            else
            {
                y = GetMin(z.right);
                yOriginalColor = y.color;
                x = y.right!;
                if (y.parent == z)
                {
                    if (x != null) x.parent = y;
                }
                else
                {
                    Transplant(y, y.right);
                    y.right = z.right;
                    y.right.parent = y;
                }
                Transplant(z, y);
                y.left = z.left;
                y.left.parent = y;
                y.color = z.color;
            }

            if (yOriginalColor == Color.Black && x != null)
                FixAfterDeletion(x);
        }

        private void Transplant(Node u, Node? v)
        {
            if (u.parent == null)
                root = v;
            else if (u == u.parent.left)
                u.parent.left = v;
            else
                u.parent.right = v;
            
            if (v != null)
                v.parent = u.parent;
        }

        private void FixAfterDeletion(Node x)
        {
            while (x != root && x.color == Color.Black)
            {
                if (x == x.parent.left)
                {
                    Node w = x.parent.right!;
                    if (w.color == Color.Red)
                    {
                        w.color = Color.Black;
                        x.parent.color = Color.Red;
                        RotateLeft(x.parent);
                        w = x.parent.right!;
                    }
                    if ((w.left == null || w.left.color == Color.Black) &&
                        (w.right == null || w.right.color == Color.Black))
                    {
                        w.color = Color.Red;
                        x = x.parent;
                    }
                    else
                    {
                        if (w.right == null || w.right.color == Color.Black)
                        {
                            w.left.color = Color.Black;
                            w.color = Color.Red;
                            RotateRight(w);
                            w = x.parent.right!;
                        }
                        w.color = x.parent.color;
                        x.parent.color = Color.Black;
                        w.right.color = Color.Black;
                        RotateLeft(x.parent);
                        x = root;
                    }
                }
                else
                {
                    Node w = x.parent.left!;
                    if (w.color == Color.Red)
                    {
                        w.color = Color.Black;
                        x.parent.color = Color.Red;
                        RotateRight(x.parent);
                        w = x.parent.left!;
                    }
                    if ((w.right == null || w.right.color == Color.Black) &&
                        (w.left == null || w.left.color == Color.Black))
                    {
                        w.color = Color.Red;
                        x = x.parent;
                    }
                    else
                    {
                        if (w.left == null || w.left.color == Color.Black)
                        {
                            w.right.color = Color.Black;
                            w.color = Color.Red;
                            RotateLeft(w);
                            w = x.parent.left!;
                        }
                        w.color = x.parent.color;
                        x.parent.color = Color.Black;
                        w.left.color = Color.Black;
                        RotateRight(x.parent);
                        x = root;
                    }
                }
            }
            x.color = Color.Black;
        }

        private Node GetMin(Node node)
        {
            while (node.left != null) node = node.left;
            return node;
        }

        private Node GetMax(Node node)
        {
            while (node.right != null) node = node.right;
            return node;
        }

        public T? First()
        {
            if (root == null) throw new InvalidOperationException("Set is empty");
            return GetMin(root).value;
        }

        public T? Last()
        {
            if (root == null) throw new InvalidOperationException("Set is empty");
            return GetMax(root).value;
        }

        public MyTreeSet<T> HeadSet(T end)
        {
            return HeadSet(end, false);
        }

        public MyTreeSet<T> HeadSet(T end, bool inclusive)
        {
            ArgumentNullException.ThrowIfNull(end);
            var result = new MyTreeSet<T>(this.comparer ?? Comparer<T>.Default);
            CollectInRange(root, result, default!, end, false, inclusive);
            return result;
        }

        public MyTreeSet<T> SubSet(T start, T end)
        {
            return SubSet(start, true, end, false);
        }

        public MyTreeSet<T> SubSet(T start, bool lowInclusive, T end, bool highInclusive)
        {
            ArgumentNullException.ThrowIfNull(start);
            ArgumentNullException.ThrowIfNull(end);
            var result = new MyTreeSet<T>(this.comparer ?? Comparer<T>.Default);
            CollectInRange(root, result, start, end, lowInclusive, highInclusive);
            return result;
        }

        private void CollectInRange(Node? node, MyTreeSet<T> result, T start, T end, bool lowIncl, bool highIncl)
        {
            if (node == null) return;

            int cmpStart = Compare(node.value, start);
            int cmpEnd = Compare(node.value, end);

            if (cmpStart > 0)
                CollectInRange(node.left, result, start, end, lowIncl, highIncl);

            bool includeLow = lowIncl ? cmpStart >= 0 : cmpStart > 0;
            bool includeHigh = highIncl ? cmpEnd <= 0 : cmpEnd < 0;
            if (includeLow && includeHigh)
                result.Add(node.value);

            if (cmpEnd < 0)
                CollectInRange(node.right, result, start, end, lowIncl, highIncl);
        }

        public MyTreeSet<T> TailSet(T start)
        {
            return TailSet(start, true);
        }

        public MyTreeSet<T> TailSet(T start, bool inclusive)
        {
            ArgumentNullException.ThrowIfNull(start);
            var result = new MyTreeSet<T>(this.comparer ?? Comparer<T>.Default);
            CollectFrom(node: root, result, start, inclusive);
            return result;
        }

        private void CollectFrom(Node? node, MyTreeSet<T> result, T start, bool inclusive)
        {
            if (node == null) return;

            int cmp = Compare(node.value, start);

            if (cmp > 0)
            {
                CollectFrom(node.left, result, start, inclusive);
                result.Add(node.value);
                CollectFrom(node.right, result, start, inclusive);
            }
            else if (cmp == 0 && inclusive)
            {
                result.Add(node.value);
                CollectAll(node.right, result);
            }
            else if (cmp < 0)
            {
                CollectFrom(node.right, result, start, inclusive);
            }
        }

        private void CollectAll(Node? node, MyTreeSet<T> result)
        {
            if (node == null) return;
            CollectAll(node.left, result);
            result.Add(node.value);
            CollectAll(node.right, result);
        }

        public T? Ceiling(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = root;
            T? result = default;
            bool found = false;

            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp == 0) return node.value;
                if (cmp > 0)
                {
                    result = node.value;
                    found = true;
                    node = node.left;
                }
                else
                {
                    node = node.right;
                }
            }
            return found ? result : default;
        }

        public T? Floor(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = root;
            T? result = default;
            bool found = false;

            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp == 0) return node.value;
                if (cmp < 0)
                {
                    result = node.value;
                    found = true;
                    node = node.right;
                }
                else
                {
                    node = node.left;
                }
            }
            return found ? result : default;
        }

        public T? Higher(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = root;
            T? result = default;
            bool found = false;

            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp > 0)
                {
                    result = node.value;
                    found = true;
                    node = node.left;
                }
                else
                {
                    node = node.right;
                }
            }
            return found ? result : default;
        }

        public T? Lower(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Node? node = root;
            T? result = default;
            bool found = false;

            while (node != null)
            {
                int cmp = Compare(node.value, value);
                if (cmp < 0)
                {
                    result = node.value;
                    found = true;
                    node = node.right;
                }
                else
                {
                    node = node.left;
                }
            }
            return found ? result : default;
        }

        public T? PollFirst()
        {
            if (root == null) return default;
            var first = First();
            if (first != null) Remove(first);
            return first;
        }

        public T? PollLast()
        {
            if (root == null) return default;
            var last = Last();
            if (last != null) Remove(last);
            return last;
        }

        public T[] ToArray()
        {
            var list = new List<T>();
            CollectToArray(root, list);
            return list.ToArray();
        }

        public T[] ToArray(T[] a)
        {
            var list = new List<T>();
            CollectToArray(root, list);
            
            if (a.Length < list.Count)
            {
                var newArray = (T[])Array.CreateInstance(typeof(T), list.Count);
                list.CopyTo(newArray);
                return newArray;
            }
            
            list.CopyTo(a);
            if (a.Length > list.Count)
                a[list.Count] = default!;
            return a;
        }

        private void CollectToArray(Node? node, List<T> list)
        {
            if (node == null) return;
            CollectToArray(node.left, list);
            list.Add(node.value);
            CollectToArray(node.right, list);
        }

        public HashSet<T> ToHashSet()
        {
            var set = new HashSet<T>();
            CollectToHashSet(root, set);
            return set;
        }

        private void CollectToHashSet(Node? node, HashSet<T> set)
        {
            if (node == null) return;
            CollectToHashSet(node.left, set);
            set.Add(node.value);
            CollectToHashSet(node.right, set);
        }

        public IEnumerable<T> DescendingIterator()
        {
            var list = new List<T>();
            CollectDescending(root, list);
            return list;
        }

        private void CollectDescending(Node? node, List<T> list)
        {
            if (node == null) return;
            CollectDescending(node.right, list);
            list.Add(node.value);
            CollectDescending(node.left, list);
        }

        public MyTreeSet<T> DescendingSet()
        {
            var result = new MyTreeSet<T>(new ReverseComparer(this.comparer ?? Comparer<T>.Default));
            CollectAll(root, result);
            return result;
        }

        private class ReverseComparer : IComparer<T>
        {
            private readonly IComparer<T> original;
            public ReverseComparer(IComparer<T> original) => this.original = original;
            public int Compare(T? x, T? y) => original.Compare(y, x);
        }

        public bool AddAll(T[] elements)
        {
            ArgumentNullException.ThrowIfNull(elements);
            bool modified = false;
            foreach (var e in elements)
            {
                if (Add(e))
                    modified = true;
            }
            return modified;
        }

        public bool ContainsAll(T[] elements)
        {
            ArgumentNullException.ThrowIfNull(elements);
            foreach (var e in elements)
            {
                if (!Contains(e)) return false;
            }
            return true;
        }

        public bool RemoveAll(T[] elements)
        {
            ArgumentNullException.ThrowIfNull(elements);
            bool modified = false;
            foreach (var e in elements)
            {
                if (Remove(e))
                    modified = true;
            }
            return modified;
        }

        public bool RetainAll(T[] elements)
        {
            ArgumentNullException.ThrowIfNull(elements);
            var toKeep = new HashSet<T>(elements);
            var allElements = ToHashSet();
            bool modified = false;
            foreach (var e in allElements)
            {
                if (!toKeep.Contains(e))
                {
                    Remove(e);
                    modified = true;
                }
            }
            return modified;
        }

        private int Compare(T x, T y)
        {
            return comparer?.Compare(x, y) ?? x.CompareTo(y);
        }

        public void Print()
        {
            Console.Write("[ ");
            var sorted = ToArray();
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
