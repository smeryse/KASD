using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class SumSegmentTree
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        int m = fs.NextInt();

        long[] values = new long[n];
        for (int i = 0; i < n; i++)
            values[i] = fs.NextLong();

        var st = new SegmentTreeSum(values);

        var sb = new StringBuilder();

        for (int op = 0; op < m; op++)
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
                sb.Append(st.Query(l, r)).Append('\n');
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class SegmentTreeSum
    {
        private readonly int length;     // n
        private readonly int sizePow2;   // степень двойки >= n
        private readonly long[] tree;    // 1..2*sizePow2-1 (0 не используем)

        public SegmentTreeSum(long[] data)
        {
            length = data.Length;

            int s = 1;
            while (s < length) s <<= 1;
            sizePow2 = s;

            tree = new long[2 * sizePow2];
            BuildFromArray(data);
        }

        private void BuildFromArray(long[] data)
        {
            for (int i = 0; i < length; i++)
                tree[sizePow2 + i] = data[i];

            for (int node = sizePow2 - 1; node >= 1; node--)
                tree[node] = tree[2 * node] + tree[2 * node + 1];
        }



        public void Update(int index, long value)
        {
            int node = sizePow2 + index;
            tree[node] = value;

            while ((node >>= 1) >= 1)
                tree[node] = tree[2 * node] + tree[2 * node + 1];
        }


        public long Query(int l, int r)
        {
            int left = l + sizePow2;
            int right = r + sizePow2;

            long resLeft = 0;
            long resRight = 0;

            while (left < right)
            {
                if ((left & 1) == 1)
                    resLeft += tree[left++];

                if ((right & 1) == 1)
                    resRight += tree[--right];

                left >>= 1;
                right >>= 1;
            }

            return resLeft + resRight;
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

