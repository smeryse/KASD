using System;
using System.IO;
using System.Collections.Generic;

namespace CT9.Tasks;

internal static class MultipleSearch
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        int n = fs.NextInt();
        string[] patterns = new string[n];
        for (int i = 0; i < n; i++)
            patterns[i] = fs.NextString();
        string t = fs.NextString();

        var ac = new AhoCorasick(patterns);

        bool[] found = ac.Search(t);

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < n; i++)
        {
            sb.Append(found[i] ? "YES" : "NO").Append('\n');
        }
        Console.Write(sb.ToString());
    }

    private sealed class AhoCorasick
    {
        private sealed class Node
        {
            public Dictionary<char, int> next = new();
            public int fail = 0;
            public List<int> output = new();
        }

        private readonly List<Node> trie = new() { new Node() };

        public AhoCorasick(string[] patterns)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                int node = 0;
                foreach (char c in patterns[i])
                {
                    if (!trie[node].next.ContainsKey(c))
                    {
                        trie[node].next[c] = trie.Count;
                        trie.Add(new Node());
                    }
                    node = trie[node].next[c];
                }
                trie[node].output.Add(i);
            }

            var queue = new Queue<int>();
            foreach (var kvp in trie[0].next)
            {
                queue.Enqueue(kvp.Value);
            }

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                foreach (var kvp in trie[u].next)
                {
                    char c = kvp.Key;
                    int v = kvp.Value;
                    queue.Enqueue(v);

                    int f = trie[u].fail;
                    while (f != 0 && !trie[f].next.ContainsKey(c))
                        f = trie[f].fail;
                    trie[v].fail = trie[f].next.ContainsKey(c) ? trie[f].next[c] : 0;

                    trie[v].output.AddRange(trie[trie[v].fail].output);
                }
            }
        }

        public bool[] Search(string text)
        {
            bool[] found = new bool[trie.Count];
            int node = 0;

            foreach (char c in text)
            {
                while (node != 0 && !trie[node].next.ContainsKey(c))
                    node = trie[node].fail;
                if (trie[node].next.ContainsKey(c))
                    node = trie[node].next[c];

                foreach (int idx in trie[node].output)
                    found[idx] = true;
            }

            bool[] result = new bool[found.Length];
            for (int i = 0; i < found.Length; i++)
                result[i] = found[i];
            return result;
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

            int val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val;
        }

        public string NextString()
        {
            int c;
            do c = Read(); while (c <= ' ');

            var sb = new System.Text.StringBuilder();
            while (c > ' ')
            {
                sb.Append((char)c);
                c = Read();
            }
            return sb.ToString();
        }
    }
}
