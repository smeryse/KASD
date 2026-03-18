using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class FirstAtLeastSegmentTree
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        int[] values = new int[n];
        for (int i = 0; i < n; i++)
            values[i] = fs.NextInt();

        var st = new SegmentTreeFirstAtLeast(values);
        var sb = new StringBuilder();

        for (int op = 0; op < m; op++)
        {
            int type = fs.NextInt();
            if (type == 1)
            {
                int index = fs.NextInt();
                int value = fs.NextInt();
                st.Update(index, value);
            }
            else
            {
                int x = fs.NextInt();
                int l = fs.NextInt();
                sb.Append(st.FindFirstAtLeast(x, l)).Append('\n');
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class SegmentTreeFirstAtLeast
    {
        private readonly int length;
        private readonly int sizePow2;
        private readonly int[] tree;

        public SegmentTreeFirstAtLeast(int[] data)
        {
            length = data.Length;

            int s = 1;
            while (s < length) s <<= 1;
            sizePow2 = s;

            tree = new int[2 * sizePow2];
            BuildFromArray(data);
        }

        private void BuildFromArray(int[] data)
        {
            for (int i = 0; i < length; i++)
                tree[sizePow2 + i] = data[i];

            for (int node = sizePow2 - 1; node >= 1; node--)
                tree[node] = Math.Max(tree[2 * node], tree[2 * node + 1]);
        }

        public void Update(int index, int value)
        {
            int node = sizePow2 + index;
            tree[node] = value;

            while ((node >>= 1) >= 1)
                tree[node] = Math.Max(tree[2 * node], tree[2 * node + 1]);
        }

        public int FindFirstAtLeast(int x, int l)
        {
            if (tree[1] < x) return -1;
            return Find(1, 0, sizePow2, l, x);
        }

        private int Find(int node, int nl, int nr, int l, int x)
        {
            if (nr <= l || tree[node] < x) return -1;
            if (nr - nl == 1) return nl;

            int mid = (nl + nr) >> 1;
            int left = 2 * node;
            int res = Find(left, nl, mid, l, x);
            if (res != -1) return res;
            return Find(left + 1, mid, nr, l, x);
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
    }
}
