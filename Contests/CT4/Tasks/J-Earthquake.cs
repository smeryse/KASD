using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace CT4.Tasks;

internal static class Painter
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());

        int n = fs.NextInt();
        
        var coords = new List<int>();
        var operations = new List<(char color, int x, int len)>();
        
        for (int i = 0; i < n; i++)
        {
            char c = (char)fs.Read();
            while (c != 'W' && c != 'B') c = (char)fs.Read();
            
            int x = fs.NextInt();
            int len = fs.NextInt();
            
            operations.Add((c, x, len));
            coords.Add(x);
            coords.Add(x + len);
        }
        
        coords = coords.Distinct().OrderBy(x => x).ToList();
        var coordMap = new Dictionary<int, int>();
        for (int i = 0; i < coords.Count; i++)
            coordMap[coords[i]] = i;
        
        var st = new SegmentTreePainter(coords.Count - 1, coords);
        var sb = new StringBuilder();
        
        foreach (var op in operations)
        {
            int l = coordMap[op.x];
            int r = coordMap[op.x + op.len];
            
            st.Paint(l, r - 1, op.color == 'B');
            
            var result = st.GetBlackSegments();
            sb.Append(result.count).Append(' ').Append(result.totalLength).Append('\n');
        }
        
        Console.Write(sb.ToString());
    }


    private sealed class SegmentTreePainter
    {
        private readonly int sizePow2;
        private readonly int[] segmentCount;
        private readonly long[] totalLength;
        private readonly int?[] lazy;
        private readonly List<int> coords;
        private readonly int n;

        public SegmentTreePainter(int length, List<int> coordinates)
        {
            n = length;
            coords = coordinates;
            
            int s = 1;
            while (s < length) s <<= 1;
            sizePow2 = s;
            
            segmentCount = new int[2 * sizePow2];
            totalLength = new long[2 * sizePow2];
            lazy = new int?[2 * sizePow2];
        }

        public void Paint(int l, int r, bool isBlack)
        {
            Paint(1, 0, sizePow2, l, r, isBlack ? 1 : 0);
        }

        public (int count, long totalLength) GetBlackSegments()
        {
            return (segmentCount[1], totalLength[1]);
        }

        private void Paint(int node, int nl, int nr, int l, int r, int color)
        {
            if (r < nl || nr <= l) return;
            
            if (l <= nl && nr <= r)
            {
                ApplyLazy(node, nl, nr, color);
                return;
            }

            Push(node, nl, nr);
            int mid = (nl + nr) >> 1;
            Paint(2 * node, nl, mid, l, r, color);
            Paint(2 * node + 1, mid, nr, l, r, color);
            Merge(node, nl, nr);
        }

        private void ApplyLazy(int node, int nl, int nr, int color)
        {
            lazy[node] = color;
            
            if (nl >= n)
            {
                segmentCount[node] = 0;
                totalLength[node] = 0;
                return;
            }
            
            int validNr = Math.Min(nr, n);
            long len = 0;
            for (int i = nl; i < validNr; i++)
                len += coords[i + 1] - coords[i];
            
            if (color == 1)
            {
                segmentCount[node] = 1;
                totalLength[node] = len;
            }
            else
            {
                segmentCount[node] = 0;
                totalLength[node] = 0;
            }
        }

        private void Push(int node, int nl, int nr)
        {
            if (!lazy[node].HasValue || nr - nl <= 1) return;
            
            int mid = (nl + nr) >> 1;
            ApplyLazy(2 * node, nl, mid, lazy[node].Value);
            ApplyLazy(2 * node + 1, mid, nr, lazy[node].Value);
            lazy[node] = null;
        }

        private void Merge(int node, int nl, int nr)
        {
            int left = 2 * node;
            int right = 2 * node + 1;
            int mid = (nl + nr) >> 1;
            
            segmentCount[node] = segmentCount[left] + segmentCount[right];
            totalLength[node] = totalLength[left] + totalLength[right];
            
            if (segmentCount[left] > 0 && segmentCount[right] > 0 && 
                mid < n && totalLength[left] > 0 && totalLength[right] > 0)
            {
                segmentCount[node]--;
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

        public byte Read()
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