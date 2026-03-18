using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class MinCountSegmentTree
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        long[] a = new long[n];
        for (int i = 0; i < n; i++)
            a[i] = fs.NextLong();

        var st = new SegmentTreeMinCount(a);

        var sb = new StringBuilder();

        for (int q = 0; q < m; q++)
        {
            int type = fs.NextInt();
            if (type == 1)
            {
                int index = fs.NextInt();
                long value = fs.NextLong();
                st.Update(index, value);
            }
            else
            {
                int l = fs.NextInt();
                int r = fs.NextInt();
                var ans = st.Query(l, r);
                sb.Append(ans.Min).Append(' ').Append(ans.Cnt).Append('\n');
            }
        }

        Console.Write(sb.ToString());
    }


    private readonly struct Node
    {
        public readonly long Min;
        public readonly int Cnt;

        public Node(long min, int cnt)
        {
            Min = min;
            Cnt = cnt;
        }
    }

    private sealed class SegmentTreeMinCount
    {
        private static readonly Node Neutral = new Node(long.MaxValue, 0);

        private readonly int length;
        private readonly int sizePow2;
        private readonly Node[] tree;

        public SegmentTreeMinCount(long[] data)
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
                tree[sizePow2 + i] = new Node(data[i], 1);

            for (int node = sizePow2 - 1; node >= 1; node--)
                tree[node] = Merge(tree[2 * node], tree[2 * node + 1]);
        }

        public void Update(int index, long value)
        {
            int node = sizePow2 + index;
            tree[node] = new Node(value, 1);

            while ((node >>= 1) >= 1)
                tree[node] = Merge(tree[2 * node], tree[2 * node + 1]);
        }

        public (long Min, int Cnt) Query(int l, int r)
        {
            int left = l + sizePow2;
            int right = r + sizePow2;

            Node resLeft = Neutral;
            Node resRight = Neutral;

            while (left < right)
            {
                if ((left & 1) == 1)
                    resLeft = Merge(resLeft, tree[left++]);

                if ((right & 1) == 1)
                    resRight = Merge(tree[--right], resRight);

                left >>= 1;
                right >>= 1;
            }

            Node res = Merge(resLeft, resRight);
            return (res.Min, res.Cnt);
        }

        private static Node Merge(Node a, Node b)
        {
            if (a.Min < b.Min) return a;
            if (b.Min < a.Min) return b;
            return new Node(a.Min, a.Cnt + b.Cnt);
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
