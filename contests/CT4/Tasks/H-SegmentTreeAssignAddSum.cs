using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class AssignAddSumSegmentTree
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        var st = new SegmentTreeAssignAddSum(n);
        var sb = new StringBuilder();

        for (int op = 0; op < m; op++)
        {
            int type = fs.NextInt();
            if (type == 1)
            {
                int l = fs.NextInt();
                int r = fs.NextInt();
                long v = fs.NextLong();
                st.Assign(l, r, v);
            }
            else if (type == 2)
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
                sb.Append(st.QuerySum(l, r)).Append('\n');
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class SegmentTreeAssignAddSum
    {
        private readonly int sizePow2;
        private readonly long[] tree;
        private readonly long[] addLazy;
        private readonly long[] assignLazy;
        private readonly bool[] hasAssign;

        public SegmentTreeAssignAddSum(int length)
        {
            int s = 1;
            while (s < length) s <<= 1;
            sizePow2 = s;
            tree = new long[2 * sizePow2];
            addLazy = new long[2 * sizePow2];
            assignLazy = new long[2 * sizePow2];
            hasAssign = new bool[2 * sizePow2];
        }

        public void Assign(int l, int r, long value)
        {
            Assign(1, 0, sizePow2, l, r, value);
        }

        public void Add(int l, int r, long value)
        {
            Add(1, 0, sizePow2, l, r, value);
        }

        public long QuerySum(int l, int r)
        {
            return QuerySum(1, 0, sizePow2, l, r);
        }

        private void Assign(int node, int nl, int nr, int l, int r, long value)
        {
            if (r <= nl || nr <= l) return;
            if (l <= nl && nr <= r)
            {
                ApplyAssign(node, nl, nr, value);
                return;
            }

            Push(node, nl, nr);
            int mid = (nl + nr) >> 1;
            Assign(2 * node, nl, mid, l, r, value);
            Assign(2 * node + 1, mid, nr, l, r, value);
            tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        private void Add(int node, int nl, int nr, int l, int r, long value)
        {
            if (r <= nl || nr <= l) return;
            if (l <= nl && nr <= r)
            {
                ApplyAdd(node, nl, nr, value);
                return;
            }

            Push(node, nl, nr);
            int mid = (nl + nr) >> 1;
            Add(2 * node, nl, mid, l, r, value);
            Add(2 * node + 1, mid, nr, l, r, value);
            tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        private long QuerySum(int node, int nl, int nr, int l, int r)
        {
            if (r <= nl || nr <= l) return 0;
            if (l <= nl && nr <= r) return tree[node];

            Push(node, nl, nr);
            int mid = (nl + nr) >> 1;
            long left = QuerySum(2 * node, nl, mid, l, r);
            long right = QuerySum(2 * node + 1, mid, nr, l, r);
            return left + right;
        }

        private void ApplyAssign(int node, int nl, int nr, long value)
        {
            tree[node] = value * (nr - nl);
            assignLazy[node] = value;
            hasAssign[node] = true;
            addLazy[node] = 0;
        }

        private void ApplyAdd(int node, int nl, int nr, long value)
        {
            tree[node] += value * (nr - nl);
            if (hasAssign[node])
                assignLazy[node] += value;
            else
                addLazy[node] += value;
        }

        private void Push(int node, int nl, int nr)
        {
            if (nr - nl <= 1) return;
            int left = 2 * node;
            int right = left + 1;
            int mid = (nl + nr) >> 1;

            if (hasAssign[node])
            {
                long value = assignLazy[node];
                ApplyAssign(left, nl, mid, value);
                ApplyAssign(right, mid, nr, value);
                hasAssign[node] = false;
            }

            if (addLazy[node] != 0)
            {
                long value = addLazy[node];
                ApplyAdd(left, nl, mid, value);
                ApplyAdd(right, mid, nr, value);
                addLazy[node] = 0;
            }
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
