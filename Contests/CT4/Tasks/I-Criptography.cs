using System;
using System.IO;
using System.Text;

namespace CT4.Tasks
{
    internal static class MatrixSegmentTree
    {
        public static void Solve()
        {
            var fs = new FastScanner(Console.OpenStandardInput());
            int r = fs.NextInt();
            int n = fs.NextInt();
            int m = fs.NextInt();

            var matrices = new Matrix2x2[n];
            for (int i = 0; i < n; i++)
            {
                matrices[i] = new Matrix2x2(
                    fs.NextInt() % r, fs.NextInt() % r,
                    fs.NextInt() % r, fs.NextInt() % r,
                    r
                );
            }

            var st = new SegmentTree(n, matrices, r);
            var sb = new StringBuilder();

            for (int i = 0; i < m; i++)
            {
                int l = fs.NextInt() - 1;
                int rr = fs.NextInt();
                var res = st.Query(l, rr);
                sb.AppendLine($"{res.A11} {res.A12}");
                sb.AppendLine($"{res.A21} {res.A22}");
                sb.AppendLine();
            }

            Console.Write(sb.ToString());
        }

        private readonly struct Matrix2x2
        {
            public readonly int A11, A12, A21, A22;
            private readonly int mod;

            public Matrix2x2(int a11, int a12, int a21, int a22, int mod)
            {
                A11 = a11; A12 = a12; A21 = a21; A22 = a22;
                this.mod = mod;
            }

            public Matrix2x2 Multiply(Matrix2x2 other)
            {
                return new Matrix2x2(
                    (int)(((long)A11 * other.A11 + (long)A12 * other.A21) % mod),
                    (int)(((long)A11 * other.A12 + (long)A12 * other.A22) % mod),
                    (int)(((long)A21 * other.A11 + (long)A22 * other.A21) % mod),
                    (int)(((long)A21 * other.A12 + (long)A22 * other.A22) % mod),
                    mod
                );
            }

            public static Matrix2x2 Identity(int mod) => new Matrix2x2(1, 0, 0, 1, mod);
        }

        private sealed class SegmentTree
        {
            private readonly int sizePow2;
            private readonly Matrix2x2[] tree;
            private readonly int mod;

            public SegmentTree(int n, Matrix2x2[] arr, int mod)
            {
                this.mod = mod;
                int s = 1;
                while (s < n) s <<= 1;
                sizePow2 = s;
                tree = new Matrix2x2[2 * sizePow2];
                for (int i = 0; i < sizePow2; i++)
                {
                    tree[sizePow2 + i] = i < n ? arr[i] : Matrix2x2.Identity(mod);
                }
                for (int i = sizePow2 - 1; i >= 1; i--)
                {
                    tree[i] = tree[2 * i].Multiply(tree[2 * i + 1]);
                }
            }

            public Matrix2x2 Query(int l, int r)
            {
                l += sizePow2;
                r += sizePow2;
                Matrix2x2 leftRes = Matrix2x2.Identity(mod);
                Matrix2x2 rightRes = Matrix2x2.Identity(mod);

                while (l < r)
                {
                    if ((l & 1) == 1) leftRes = leftRes.Multiply(tree[l++]);
                    if ((r & 1) == 1) rightRes = tree[--r].Multiply(rightRes);
                    l >>= 1;
                    r >>= 1;
                }

                return leftRes.Multiply(rightRes);
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
}
