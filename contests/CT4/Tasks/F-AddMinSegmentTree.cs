using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class AddMinSegmentTree
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        var st = new SegmentTreeAddMin(n);
        var sb = new StringBuilder();

        for (int op = 0; op < m; op++)
        {
            int type = fs.NextInt();
            if (type == 1)
            {
                int l = fs.NextInt();
                int r = fs.NextInt();
                long v = fs.NextLong();
                st.Add(l, r, v);
            }
            else
            {
                int l = fs.NextInt();
                int r = fs.NextInt();
                sb.Append(st.QueryMin(l, r)).Append('\n');
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class SegmentTreeAddMin
    {
        private readonly int length;
        private readonly int sizePow2;
        private readonly long[] tree;
        private readonly long[] lazy;

        public SegmentTreeAddMin(int length)
        {
            this.length = length;

            int s = 1;
            while (s < length) s <<= 1;
            sizePow2 = s;

            tree = new long[2 * sizePow2];
            lazy = new long[2 * sizePow2];
            BuildZeros();
        }

        private void BuildZeros()
        {
            for (int i = 0; i < sizePow2; i++)
                tree[sizePow2 + i] = i < length ? 0 : long.MaxValue;

            for (int node = sizePow2 - 1; node >= 1; node--)
                tree[node] = Math.Min(tree[2 * node], tree[2 * node + 1]);
        }

        public void Add(int l, int r, long value)
        {
            Add(1, 0, sizePow2, l, r, value);
        }

        public long QueryMin(int l, int r)
        {
            return QueryMin(1, 0, sizePow2, l, r);
        }

        private void Add(int node, int nl, int nr, int l, int r, long value)
        {
            if (r <= nl || nr <= l) return;
            if (l <= nl && nr <= r)
            {
                tree[node] += value;
                lazy[node] += value;
                return;
            }

            Push(node);
            int mid = (nl + nr) >> 1;
            Add(2 * node, nl, mid, l, r, value);
            Add(2 * node + 1, mid, nr, l, r, value);
            tree[node] = Math.Min(tree[2 * node], tree[2 * node + 1]);
        }

        private long QueryMin(int node, int nl, int nr, int l, int r)
        {
            if (r <= nl || nr <= l) return long.MaxValue;
            if (l <= nl && nr <= r) return tree[node];

            Push(node);
            int mid = (nl + nr) >> 1;
            long left = QueryMin(2 * node, nl, mid, l, r);
            long right = QueryMin(2 * node + 1, mid, nr, l, r);
            return Math.Min(left, right);
        }

        private void Push(int node)
        {
            long add = lazy[node];
            if (add == 0) return;
            int left = 2 * node;
            int right = left + 1;
            tree[left] += add;
            tree[right] += add;
            lazy[left] += add;
            lazy[right] += add;
            lazy[node] = 0;
        }
    }


    private sealed class FastScanner
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

        public long NextLong()
        {
            int c;
            do c = Read(); while (c <= ' ');

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            long val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val * sign;
        }
    }
}
