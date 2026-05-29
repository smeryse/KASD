using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CT9.Tasks;

internal static class MultipleSearch2
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

        int maxNodes = 2;
        for (int i = 0; i < n; i++) maxNodes += patterns[i].Length;

        var go = new int[maxNodes * ALPHA];
        int sz = 1;

        var patEnd = new int[n];

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

        var stateCount = new long[sz];
        int state = 0;
        foreach (char ch in t)
        {
            state = go[state * ALPHA + (ch - 'a')];
            stateCount[state]++;
        }

        for (int i = tail - 1; i >= 0; i--)
        {
            int u = order[i];
            stateCount[fail[u]] += stateCount[u];
        }

        var sb = new StringBuilder(n * 4);
        for (int i = 0; i < n; i++)
        {
            sb.Append(stateCount[patEnd[i]]);
            sb.Append('\n');
        }

        Console.Write(sb);
    }
}
