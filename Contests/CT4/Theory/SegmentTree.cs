using System;

public sealed class SegmentTree
{
    private readonly int n;          // исходная длина
    private readonly int size;       // степень двойки >= n
    private readonly long neutral;   // нейтральный элемент
    private readonly Func<long, long, long> op; // ассоциативная операция
    private readonly long[] tree;    // 1..2*size-1 (0 не используем)

    public SegmentTree(long[] data, Func<long, long, long> op, long neutral)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (op == null) throw new ArgumentNullException(nameof(op));

        this.n = data.Length;
        this.op = op;
        this.neutral = neutral;

        int s = 1;
        while (s < n) s <<= 1;
        this.size = s;

        tree = new long[2 * size];
        Build(data);
    }

    private void Build(long[] data)
    {
        for (int i = 0; i < n; i++)
            tree[size + i] = data[i];

        for (int i = size - 1; i >= 1; i--)
            tree[i] = op(tree[2 * i], tree[2 * i + 1]);
    }


    public long Query(int l, int r)
    {
        if (l < 0 || r < 0 || l > r || r > n)
            throw new ArgumentOutOfRangeException($"Bad range: [{l}, {r}) for n={n}");

        l += size;
        r += size;

        long resLeft = neutral;
        long resRight = neutral;

        while (l < r)
        {
            if ((l & 1) == 1)
            {
                resLeft = op(resLeft, tree[l]);
                l++;
            }

            if ((r & 1) == 1)
            {
                r--;
                resRight = op(tree[r], resRight);
            }

            l >>= 1;
            r >>= 1;
        }

        return op(resLeft, resRight);
    }

    public void Update(int i, long value)
    {
        if (i < 0 || i >= n) throw new ArgumentOutOfRangeException(nameof(i));

        int v = size + i;
        tree[v] = value;

        v >>= 1;
        while (v >= 1)
        {
            tree[v] = op(tree[2 * v], tree[2 * v + 1]);
            v >>= 1;
        }
    }
}
