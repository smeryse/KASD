using System;
using System.Collections.Generic;
using System.Text;

namespace CT4.Tasks;

class Program
{
    static void Main()
    {
        TreapKth.Solve();
    }
}
internal static class TreapKth
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        int size = 0;
        var sb = new StringBuilder();

        for (int i = 0; i < n; i++)
        {
            int c = fs.NextInt();
            int k = fs.NextInt();
            switch (c)
            {
                case 1:
                    Insert(new Node(k));
                    size++;
                    break;
                case 0:
                    sb.AppendLine(FindKth(root, size - k).ToString());
                    break;
                case -1:
                    Remove(k);
                    size--;
                    break;
            }
        }

        Console.Write(sb.ToString());
    }

    private class Node
    {
        public int x, y, size = 1;
        public Node l = null, r = null;
        private static Random rnd = new Random();

        public Node(int x)
        {
            this.x = x;
            y = rnd.Next();
        }
    }

    private static Node root = null;

    private static int GetSize(Node t) => t == null ? 0 : t.size;

    private static void UpdateSize(Node t)
    {
        if (t != null)
            t.size = 1 + GetSize(t.l) + GetSize(t.r);
    }

    private static void Split(Node t, out Node l, out Node r, int x)
    {
        if (t == null) { l = r = null; return; }
        if (t.x > x)
        {
            Split(t.l, out l, out t.l, x);
            r = t;
        }
        else
        {
            Split(t.r, out t.r, out r, x);
            l = t;
        }
        UpdateSize(t);
    }

    private static Node Merge(Node l, Node r)
    {
        if (l == null) return r;
        if (r == null) return l;

        if (l.y > r.y)
        {
            l.r = Merge(l.r, r);
            UpdateSize(l);
            return l;
        }
        else
        {
            r.l = Merge(l, r.l);
            UpdateSize(r);
            return r;
        }
    }

    private static void Insert(Node v)
    {
        if (root == null)
        {
            root = v;
            return;
        }

        if (root.y < v.y)
        {
            Split(root, out v.l, out v.r, v.x);
            root = v;
        }
        else
        {
            Node cur = root;
            while (true)
            {
                if (v.x < cur.x)
                {
                    if (cur.l == null) { cur.l = v; break; }
                    if (cur.l.y < v.y) { Split(cur.l, out v.l, out v.r, v.x); cur.l = v; break; }
                    cur = cur.l;
                }
                else
                {
                    if (cur.r == null) { cur.r = v; break; }
                    if (cur.r.y < v.y) { Split(cur.r, out v.l, out v.r, v.x); cur.r = v; break; }
                    cur = cur.r;
                }
            }
        }

        UpdateSize(root);
    }

    private static void Remove(int x)
    {
        root = Remove(root, x);
    }

    private static Node Remove(Node t, int x)
    {
        if (t == null) return null;
        if (t.x == x)
        {
            return Merge(t.l, t.r);
        }
        if (x < t.x) t.l = Remove(t.l, x);
        else t.r = Remove(t.r, x);
        UpdateSize(t);
        return t;
    }

    private static int FindKth(Node t, int k)
    {
        int leftSize = GetSize(t.l);
        if (leftSize == k) return t.x;
        if (leftSize > k) return FindKth(t.l, k);
        return FindKth(t.r, k - leftSize - 1);
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
