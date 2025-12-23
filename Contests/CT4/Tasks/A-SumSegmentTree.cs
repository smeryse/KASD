using System;

namespace CT4.Tasks;

internal static class SumSegmentTree
{
    public static void Solve()
    {
        var first = Console.ReadLine()?.Split();
        if (first == null || first.Length < 2) return;
        int n = int.Parse(first[0]);
        int m = int.Parse(first[1]);

        var arrLine = Console.ReadLine()?.Split();
        long[] values = new long[n];
        for (int i = 0; i < n; i++)
            values[i] = long.Parse(arrLine[i]);

        var st = new SegmentTree(values);

        for (int op = 0; op < m; op++)
        {
            var parts = Console.ReadLine()?.Split();
            if (parts == null || parts.Length == 0) continue;
            int type = int.Parse(parts[0]);
            if (type == 1)
            {
                int i = int.Parse(parts[1]);
                long v = long.Parse(parts[2]);
                st.Update(i, v);
            }
            else
            {
                int l = int.Parse(parts[1]);
                int r = int.Parse(parts[2]);
                Console.WriteLine(st.Query(l, r));
            }
        }
    }

    private sealed class SegmentTree
    {
        private readonly long[] tree;
        private readonly int size;

        public SegmentTree(long[] values)
        {
            size = 1;
            while (size < values.Length) size <<= 1;
            tree = new long[size << 1];
            for (int i = 0; i < values.Length; i++)
                tree[size + i] = values[i];
            for (int i = size - 1; i >= 1; i--)
                tree[i] = tree[i << 1] + tree[(i << 1) | 1];
        }

        public void Update(int index, long value)
        {
            int pos = size + index;
            tree[pos] = value;
            pos >>= 1;
            while (pos >= 1)
            {
                tree[pos] = tree[pos << 1] + tree[(pos << 1) | 1];
                pos >>= 1;
            }
        }

        public long Query(int l, int r)
        {
            long res = 0;
            int left = l + size;
            int right = r + size;
            while (left < right)
            {
                if ((left & 1) == 1) res += tree[left++];
                if ((right & 1) == 1) res += tree[--right];
                left >>= 1;
                right >>= 1;
            }
            return res;
        }
    }
}
