using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;

namespace CT10.Tasks;

internal static class TaskC
{
    public static void Solve()
    {
        var scanner = new FastScanner(Console.OpenStandardInput());
        
        int n = scanner.NextInt();
        int v = scanner.NextInt();

        var points = new Point[n];
        for (int i = 0; i < n; i++)
        {
            string timeStr = scanner.NextToken();
            int minutes = ParseTimeToMinutes(timeStr);
            int x = scanner.NextInt();
            int y = scanner.NextInt();
            points[i] = new Point(minutes, x, y);
        }

        var adj = new List<int>[n];
        for (int i = 0; i < n; i++)
            adj[i] = new List<int>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j) continue;
                
                long dt = points[j].Time - points[i].Time;
                if (dt <= 0) continue;

                long dx = points[i].X - points[j].X;
                long dy = points[i].Y - points[j].Y;
                long distSq = dx * dx + dy * dy;
                
                long rhs = (long)v * dt;
                if (3600L * distSq <= rhs * rhs)
                {
                    adj[i].Add(j);
                }
            }
        }

        int matchingSize = FindMaximumMatching(adj, n);
        
        Console.WriteLine(n - matchingSize);
    }

    private static int ParseTimeToMinutes(string time)
    {
        // Формат "HH:MM"
        int hours = (time[0] - '0') * 10 + (time[1] - '0');
        int minutes = (time[3] - '0') * 10 + (time[4] - '0');
        return hours * 60 + minutes;
    }

    private static int FindMaximumMatching(List<int>[] adj, int n)
    {
        int[] matchB = new int[n];
        Array.Fill(matchB, -1);
        
        int matching = 0;
        bool[] visited = new bool[n];

        for (int u = 0; u < n; u++)
        {
            Array.Fill(visited, false);
            if (Dfs(u, adj, matchB, visited))
            {
                matching++;
            }
        }
        return matching;
    }

    private static bool Dfs(int u, List<int>[] adj, int[] matchB, bool[] visited)
    {
        if (visited[u]) return false;
        visited[u] = true;

        foreach (int v in adj[u])
        {
            if (matchB[v] == -1 || Dfs(matchB[v], adj, matchB, visited))
            {
                matchB[v] = u;
                return true;
            }
        }
        return false;
    }

    private readonly struct Point
    {
        public readonly int Time;
        public readonly int X;
        public readonly int Y;

        public Point(int time, int x, int y)
        {
            Time = time;
            X = x;
            Y = y;
        }
    }
}

internal sealed class FastScanner : IDisposable
{
    private readonly Stream _stream;
    private readonly byte[] _buffer;
    private int _pos;
    private int _len;

    public FastScanner(Stream stream)
    {
        _stream = stream;
        _buffer = new byte[1 << 16];
        _pos = 0;
        _len = 0;
    }

    private byte ReadByte()
    {
        if (_pos >= _len)
        {
            _pos = 0;
            _len = _stream.Read(_buffer, 0, _buffer.Length);
            if (_len == 0) return 0;
        }
        return _buffer[_pos++];
    }

    public int NextInt()
    {
        int c = SkipWhitespace();
        if (c == 0) return 0;

        int sign = 1;
        if (c == '-')
        {
            sign = -1;
            c = ReadByte();
        }

        int res = 0;
        while (c >= '0' && c <= '9')
        {
            res = res * 10 + (c - '0');
            c = ReadByte();
        }
        return res * sign;
    }

    public string NextToken()
    {
        int c = SkipWhitespace();
        if (c == 0) return string.Empty;

        var buffer = ArrayPool<char>.Shared.Rent(64); 
        int idx = 0;
        
        while (c > ' ')
        {
            if (idx == buffer.Length)
            {
                var newBuffer = ArrayPool<char>.Shared.Rent(buffer.Length * 2);
                Array.Copy(buffer, newBuffer, buffer.Length);
                ArrayPool<char>.Shared.Return(buffer);
                buffer = newBuffer;
            }
            buffer[idx++] = (char)c;
            c = ReadByte();
        }
        
        string result = new string(buffer, 0, idx);
        ArrayPool<char>.Shared.Return(buffer);
        return result;
    }

    private int SkipWhitespace()
    {
        int c = ReadByte();
        while (c <= ' ')
        {
            if (c == 0) return 0;
            c = ReadByte();
        }
        return c;
    }

    public void Dispose()
    {
    }
}