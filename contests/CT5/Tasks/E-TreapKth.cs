using System;
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
    private static Node treap = null;
    private static readonly Random rnd = new Random();

    private class Node
    {
        public int x, y, size = 1;
        public Node l, r;
        public Node(int x) { this.x = x; y = rnd.Next(); }
    }

    private static int GetSize(Node t) => t == null ? 0 : t.size;
    private static void UpdateSize(Node t)
    {
        if (t != null)
            t.size = 1 + GetSize(t.l) + GetSize(t.r);
    }

    private static void Split(Node t, int x, out Node l, out Node r)
    {
        if (t == null) { l = r = null; return; }
        if (t.x > x)
        {
            Split(t.l, x, out l, out t.l);
            r = t;
        }
        else
        {
            Split(t.r, x, out t.r, out r);
            l = t;
        }
        UpdateSize(t);
    }

    private static void Merge(out Node t, Node l, Node r)
    {
        if (l == null || r == null) { t = l ?? r; return; }
        if (l.y > r.y)
        {
            Merge(out l.r, l.r, r);
            t = l;
        }
        else
        {
            Merge(out r.l, l, r.l);
            t = r;
        }
        UpdateSize(t);
    }

    private static void Insert(ref Node t, Node v)
    {
        if (t == null) { t = v; return; }
        if (t.y < v.y)
        {
            Split(t, v.x, out v.l, out v.r);
            t = v;
        }
        else
        {
            if (v.x < t.x) Insert(ref t.l, v);
            else Insert(ref t.r, v);
        }
        UpdateSize(t);
    }

    private static void Remove(ref Node t, int x)
    {
        if (t == null) return;
        if (t.x == x)
        {
            Merge(out t, t.l, t.r);
            return;
        }
        if (x < t.x) Remove(ref t.l, x);
        else Remove(ref t.r, x);
        UpdateSize(t);
    }

    private static int FindKth(Node t, int k)
    {
        int leftSize = GetSize(t.l);
        if (leftSize == k) return t.x;
        if (leftSize > k) return FindKth(t.l, k);
        return FindKth(t.r, k - leftSize - 1);
    }

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
                    Insert(ref treap, new Node(k));
                    size++;
                    break;
                case 0:
                    sb.AppendLine(FindKth(treap, size - k).ToString());
                    break;
                case -1:
                    Remove(ref treap, k);
                    size--;
                    break;
            }
        }

        Console.Write(sb);
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
            int c;
            do c = Read(); while (c <= ' ');
            int sign = 1;
            if (c == '-') { sign = -1; c = Read(); }
            int val = 0;
            while (c > ' ') { val = val * 10 + (c - '0'); c = Read(); }
            return val * sign;
        }
    }
}

