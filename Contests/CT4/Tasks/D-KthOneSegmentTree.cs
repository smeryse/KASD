using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace CT4.Tasks;

internal static class KthOneSegmentTree
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        int[] a = new int[n];
        for (int i = 0; i < n; i++)
            a[i] = fs.NextInt();

        var st = new SegmentTreeKthOne(a);
        var sb = new StringBuilder();

        for (int q = 0; q < m; q++)
        {
            int type = fs.NextInt();
            if (type == 1)
            {
                int index = fs.NextInt();
                st.Toggle(index);
            }
            else
            {
                int k = fs.NextInt();
                sb.Append(st.KthOne(k)).Append('\n');
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class SegmentTreeKthOne
    {
        private readonly int length;
        private readonly int sizePow2;
        private readonly int[] tree;

        public SegmentTreeKthOne(int[] data)
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
                tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        public void Toggle(int index)
        {
            int node = sizePow2 + index;
            tree[node] = 1 - tree[node];

            while ((node >>= 1) >= 1)
                tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        public int KthOne(int k)
        {
            int node = 1;
            while (node < sizePow2)
            {
                int left = 2 * node;
                if (tree[left] > k)
                {
                    node = left;
                }
                else
                {
                    k -= tree[left];
                    node = left + 1;
                }
            }
            return node - sizePow2;
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
