using System;
using System.IO;
using System.Text;

namespace CT9.Tasks;

internal static class MultipleSearch3
{
    const int ALPHA = 26;

    public static void Solve()
    {
        var stdin = new StreamReader(new BufferedStream(Console.OpenStandardInput(), 1 << 20), Encoding.ASCII);

        int n = int.Parse(stdin.ReadLine().TrimEnd());

        var patterns = new string[n];
        for (int i = 0; i < n; i++)
            patterns[i] = stdin.ReadLine().TrimEnd();

        string t = stdin.ReadLine().TrimEnd();
        int tLen = t.Length;

        int maxNodes = 2;
        for (int i = 0; i < n; i++) maxNodes += patterns[i].Length;

        var go = new int[maxNodes * ALPHA];
        int sz = 1;

        var patEnd = new int[n];
        var patLen = new int[n];

        for (int i = 0; i < n; i++)
        {
            int cur = 0;
            foreach (char ch in patterns[i])
            {
                int c = ch - 'a';
                int idx = cur * ALPHA + c;
                if (go[idx] == 0)
                    go[idx] = sz++;
                cur = go[idx];
            }
            patEnd[i] = cur;
            patLen[i] = patterns[i].Length;
        }

        var fail = new int[sz];
        var order = new int[sz];
        int head = 0, tail = 0;

        for (int c = 0; c < ALPHA; c++)
        {
            int v = go[c];
            if (v != 0)
            {
                fail[v] = 0;
                order[tail++] = v;
            }
        }

        while (head < tail)
        {
            int u = order[head++];
            for (int c = 0; c < ALPHA; c++)
            {
                int v = go[u * ALPHA + c];
                if (v != 0)
                {
                    fail[v] = go[fail[u] * ALPHA + c];
                    order[tail++] = v;
                }
                else
                {
                    go[u * ALPHA + c] = go[fail[u] * ALPHA + c];
                }
            }
        }

        const int INF = int.MaxValue;
        var firstEnd = new int[sz];
        var lastEnd  = new int[sz];
        for (int i = 0; i < sz; i++) { firstEnd[i] = INF; lastEnd[i] = -1; }

        int state = 0;
        for (int pos = 0; pos < tLen; pos++)
        {
            state = go[state * ALPHA + (t[pos] - 'a')];
            if (pos < firstEnd[state]) firstEnd[state] = pos;
            if (pos > lastEnd[state])  lastEnd[state]  = pos;
        }

        for (int i = tail - 1; i >= 0; i--)
        {
            int u = order[i];
            int f = fail[u];
            if (firstEnd[u] < firstEnd[f]) firstEnd[f] = firstEnd[u];
            if (lastEnd[u]  > lastEnd[f])  lastEnd[f]  = lastEnd[u];
        }

        var sb = new StringBuilder(n * 8);
        for (int i = 0; i < n; i++)
        {
            int node = patEnd[i];
            int len  = patLen[i];
            if (lastEnd[node] == -1)
            {
                sb.Append("-1 -1\n");
            }
            else
            {
                int left  = firstEnd[node] - len + 1;
                int right = lastEnd[node]  - len + 1;
                sb.Append(left);
                sb.Append(' ');
                sb.Append(right);
                sb.Append('\n');
            }
        }

        Console.Write(sb);
    }
}
