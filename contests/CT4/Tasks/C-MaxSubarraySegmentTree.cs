using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

struct Node
{
    public long Sum;
    public long Pref;
    public long Suf;
    public long Best;

    public Node(long x)
    {
        Sum = x;
        Pref = x > 0 ? x : 0;
        Suf = x > 0 ? x : 0;
        Best = x > 0 ? x : 0;
    }

    public static Node Neutral => default;
}

class MaxSubarraySegmentTree
{
    private readonly int length;
    private readonly int sizePow2;
    private readonly Node[] tree;

    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        long[] a = new long[n];
        for (int i = 0; i < n; i++) a[i] = fs.NextLong();

        var st = new MaxSubarraySegmentTree(a);

        var sb = new StringBuilder();
        sb.Append(st.tree[1].Best).Append('\n');

        for (int q = 0; q < m; q++)
        {
            int index = fs.NextInt();
            long value = fs.NextLong();
            st.Update(index, value);
            sb.Append(st.tree[1].Best).Append('\n');
        }

        Console.Write(sb.ToString());
    }

    public MaxSubarraySegmentTree(long[] data)
    {
        length = data.Length;

        int s = 1;
        while (s < length) s <<= 1;
        sizePow2 = s;

        tree = new Node[2 * sizePow2];
        BuildFromArray(data);
    }

    private void BuildFromArray(long[] data)
    {
        for (int i = 0; i < length; i++)
            tree[sizePow2 + i] = new Node(data[i]);

        for (int v = sizePow2 - 1; v >= 1; v--)
            tree[v] = Merge(tree[v << 1], tree[v << 1 | 1]);
    }

    public void Update(int index, long value)
    {
        int v = sizePow2 + index;
        tree[v] = new Node(value);

        while ((v >>= 1) >= 1)
            tree[v] = Merge(tree[v << 1], tree[v << 1 | 1]);
    }

    private static Node Merge(Node a, Node b)
    {
        Node res;
        res.Sum = a.Sum + b.Sum;
        res.Pref = Math.Max(a.Pref, a.Sum + b.Pref);
        res.Suf = Math.Max(b.Suf, b.Sum + a.Suf);
        res.Best = Math.Max(Math.Max(a.Best, b.Best), a.Suf + b.Pref);
        return res;
    }
}

class FastScanner
{
    private readonly Stream stream;
    private readonly byte[] buffer;
    private int len;
    private int ptr;

    public FastScanner(Stream stream, int bufferSize = 1 << 16)
    {
        this.stream = stream;
        buffer = new byte[bufferSize];
    }

    private int ReadByte()
    {
        if (ptr >= len)
        {
            len = stream.Read(buffer, 0, buffer.Length);
            ptr = 0;
            if (len <= 0) return -1;
        }
        return buffer[ptr++];
    }

    private int ReadNonSpace()
    {
        int c;
        do
        {
            c = ReadByte();
            if (c == -1) return -1;
        } while (c <= ' ');
        return c;
    }

    public int NextInt()
    {
        int c = ReadNonSpace();
        if (c == -1) return 0;

        int sign = 1;
        if (c == '-')
        {
            sign = -1;
            c = ReadByte();
        }

        int val = 0;
        while (c > ' ')
        {
            val = val * 10 + (c - '0');
            c = ReadByte();
            if (c == -1) break;
        }
        return val * sign;
    }

    public long NextLong()
    {
        int c = ReadNonSpace();
        if (c == -1) return 0;

        int sign = 1;
        if (c == '-')
        {
            sign = -1;
            c = ReadByte();
        }

        long val = 0;
        while (c > ' ')
        {
            val = val * 10 + (c - '0');
            c = ReadByte();
            if (c == -1) break;
        }
        return val * sign;
    }
}
