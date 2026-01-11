using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class EarthquakeBuildings
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        int m = fs.NextInt();

        var st = new SegmentTree(n);
        var sb = new StringBuilder();

        for (int i = 0; i < m; i++)
        {
            int type = fs.NextInt();
            if (type == 1)
            {
                int idx = fs.NextInt();
                int h = fs.NextInt();
                st.Assign(idx, h);
            }
            else
            {
                int l = fs.NextInt();
                int r = fs.NextInt();
                int p = fs.NextInt();
                int destroyed = st.Earthquake(l, r, p);
                sb.AppendLine(destroyed.ToString());
            }
        }

        Console.Write(sb.ToString());
    }

    private sealed class SegmentTree
    {
        private readonly int sizePow2;
        private readonly int[] tree;
        private readonly int[] maxTree;

        public SegmentTree(int n)
        {
            int s = 1;
            while (s < n) s <<= 1;
            sizePow2 = s;
            tree = new int[2 * sizePow2];      // actual heights
            maxTree = new int[2 * sizePow2];   // max in subtree
        }

        public void Assign(int idx, int value)
        {
            int node = idx + sizePow2;
            tree[node] = value;
            maxTree[node] = value;
            while (node > 1)
            {
                node >>= 1;
                maxTree[node] = Math.Max(maxTree[node << 1], maxTree[(node << 1) | 1]);
            }
        }

        public int Earthquake(int l, int r, int p)
        {
            return EarthquakeRec(1, 0, sizePow2, l, r, p);
        }

        private int EarthquakeRec(int node, int nl, int nr, int l, int r, int p)
        {
            if (nr <= l || r <= nl || maxTree[node] <= 0) return 0;

            if (nl + 1 == nr)
            {
                // leaf
                if (tree[node] <= p)
                {
                    tree[node] = 0;
                    maxTree[node] = 0;
                    return 1;
                }
                return 0;
            }

            int mid = (nl + nr) >> 1;
            int left = EarthquakeRec(node << 1, nl, mid, l, r, p);
            int right = EarthquakeRec((node << 1) | 1, mid, nr, l, r, p);
            maxTree[node] = Math.Max(maxTree[node << 1], maxTree[(node << 1) | 1]);
            return left + right;
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
