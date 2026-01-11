using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class InsertKeys
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        int m = fs.NextInt();

        Treap tree = new Treap();
        tree.Build(m);

        for (int i = 0; i < n; i++)
        {
            int pos = fs.NextInt();
            tree.Insert(pos - 1);
        }

        var ans = tree.ToVector();
        int last = ans.Length - 1;
        while (last >= 0 && ans[last] == 0) last--;

        var sb = new StringBuilder();
        sb.AppendLine((last + 1).ToString());
        for (int i = 0; i <= last; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(ans[i]);
        }
        sb.AppendLine();
        Console.Write(sb.ToString());
    }

    private class Node
    {
        public int Size = 1;
        public int Priority = Treap.Rand();
        public int Data;
        public bool Zero;

        public Node Left;
        public Node Right;

        public Node(int data = 0, bool zero = true)
        {
            Data = data;
            Zero = zero;
        }
    }

    private class Treap
    {
        public Node Root = null;
        private int Num = 0;

        public void Build(int n)
        {
            for (int i = 0; i < n; i++)
                Root = Merge(new Node(), Root);
        }

        public void Insert(int index)
        {
            Split(Root, index, out Node left, out Node right);
            var pair = SearchNull(right);
            Node z = pair.Item1;
            int i = pair.Item2;
            if (z != null && z.Data == 0)
                right = Remove(right, i);

            Root = Merge(Merge(left, new Node(++Num, false)), right);
        }

        public int[] ToVector()
        {
            var list = new System.Collections.Generic.List<int>();
            ToVector(Root, list);
            return list.ToArray();
        }

        private void ToVector(Node node, System.Collections.Generic.List<int> list)
        {
            if (node == null) return;
            ToVector(node.Left, list);
            list.Add(node.Data);
            ToVector(node.Right, list);
        }

        private Node Remove(Node root, int key)
        {
            Split(root, key, out Node t1, out Node t2);
            Split(t2, 1, out t2, out Node t3);
            root = Merge(t1, t3);
            return root;
        }

        private Tuple<Node, int> SearchNull(Node t)
        {
            Node cur = t;
            int ind = GetSize(cur?.Left);
            while (cur != null && (cur.Zero || cur.Data == 0))
            {
                if (cur.Left != null && cur.Left.Zero)
                {
                    cur = cur.Left;
                    ind -= GetSize(cur.Right) + 1;
                }
                else if (cur.Data == 0)
                {
                    break;
                }
                else
                {
                    cur = cur.Right;
                    ind += GetSize(cur.Left) + 1;
                }
            }
            return Tuple.Create(cur, ind);
        }

        private void Update(Node t)
        {
            if (t == null) return;
            t.Size = GetSize(t.Left) + GetSize(t.Right) + 1;
            t.Zero = (t.Data == 0) || GetZero(t.Left) || GetZero(t.Right);
        }

        private Node Merge(Node l, Node r)
        {
            if (l == null) return r;
            if (r == null) return l;
            if (l.Priority > r.Priority)
            {
                l.Right = Merge(l.Right, r);
                Update(l);
                return l;
            }
            else
            {
                r.Left = Merge(l, r.Left);
                Update(r);
                return r;
            }
        }

        private void Split(Node t, int key, out Node l, out Node r)
        {
            if (t == null) { l = r = null; return; }
            if (GetSize(t.Left) < key)
            {
                Split(t.Right, key - GetSize(t.Left) - 1, out Node tRight, out r);
                t.Right = tRight;
                l = t;
            }
            else
            {
                Split(t.Left, key, out l, out Node tLeft);
                t.Left = tLeft;
                r = t;
            }
            Update(t);
        }

        private static int GetSize(Node t) => t?.Size ?? 0;
        private static bool GetZero(Node t) => t?.Zero ?? false;

        private static readonly Random RandGen = new Random();
        public static int Rand() => RandGen.Next();
    }

    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int len, ptr;

        public FastScanner(Stream stream, int bufferSize = 1 << 16)
        {
            this.stream = stream;
            buffer = new byte[bufferSize];
        }

        private byte Read()
        {
            if (ptr >= len)
            {
                len = stream.Read(buffer, 0, buffer.Length);
                ptr = 0;
                if (len <= 0) return 0;
            }
            return buffer[ptr++];
        }

        public int NextInt()
        {
            int c;
            do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }
            int val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val * sign;
        }
    }
}
