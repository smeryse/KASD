using System;
using System.Collections.Generic;
using System.Text;

namespace CT4.Tasks;

internal static class ImplicitKey
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        int m = fs.NextInt();

        var a = new List<int>();
        for (int i = 0; i < n; i++)
            a.Add(fs.NextInt());

        foreach (var x in a)
            root = Merge(root, new Node(x));

        for (int i = 0; i < m; i++)
        {
            string cmd = fs.NextString();
            if (cmd == "add")
            {
                int ind = fs.NextInt();
                int val = fs.NextInt();
                Insert(ind, val);
            }
            else
            {
                int ind = fs.NextInt();
                Remove(ind - 1);
            }
        }

        a.Clear();
        ToList(root, a);

        Console.WriteLine(a.Count);
        Console.WriteLine(string.Join(" ", a));
    }

    private class Node
    {
        public int data, size, p;
        public Node left, right;
        private static Random rnd = new Random();

        public Node(int data)
        {
            this.data = data;
            size = 1;
            p = rnd.Next();
        }
    }

    private static Node root = null;

    private static int GetSize(Node t) => t == null ? 0 : t.size;

    private static void UpdateSize(Node t)
    {
        if (t != null)
            t.size = 1 + GetSize(t.left) + GetSize(t.right);
    }

    private static Node Merge(Node l, Node r)
    {
        if (l == null) return r;
        if (r == null) return l;

        if (l.p > r.p)
        {
            l.right = Merge(l.right, r);
            UpdateSize(l);
            return l;
        }
        else
        {
            r.left = Merge(l, r.left);
            UpdateSize(r);
            return r;
        }
    }

    private static void Split(Node t, int key, out Node l, out Node r)
    {
        if (t == null) { l = r = null; return; }
        int leftSize = GetSize(t.left);
        if (leftSize < key)
        {
            Split(t.right, key - leftSize - 1, out t.right, out r);
            l = t;
        }
        else
        {
            Split(t.left, key, out l, out t.left);
            r = t;
        }
        UpdateSize(t);
    }

    private static void Insert(int pos, int val)
    {
        Split(root, pos, out Node t1, out Node t2);
        root = Merge(Merge(t1, new Node(val)), t2);
    }

    private static void Remove(int pos)
    {
        Split(root, pos, out Node t1, out Node t2);
        Split(t2, 1, out Node tDel, out Node t3);
        root = Merge(t1, t3);
    }

    private static void ToList(Node t, List<int> a)
    {
        if (t == null) return;
        ToList(t.left, a);
        a.Add(t.data);
        ToList(t.right, a);
    }

    private sealed class FastScanner
    {
        private readonly System.IO.Stream stream;
        private readonly byte[] buffer;
        private int len, ptr;

        public FastScanner(System.IO.Stream stream, int bufferSize = 1 << 16)
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
            int c; do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-') { sign = -1; c = Read(); }
            int val = 0;
            while (c > ' ') { val = val * 10 + (c - '0'); c = Read(); }
            return val * sign;
        }

        public string NextString()
        {
            int c; do c = Read(); while (c <= ' ');
            var sb = new StringBuilder();
            while (c > ' ') { sb.Append((char)c); c = Read(); }
            return sb.ToString();
        }
    }
}
