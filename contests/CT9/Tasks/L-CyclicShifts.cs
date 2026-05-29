using System;
using System.IO;
using System.Text;


internal static class CyclicShifts
{
    static void Main()
    {
        string s = Console.In.ReadLine() ?? "";
        string kLine = Console.In.ReadLine() ?? "0";
        int k = int.Parse(kLine.Trim());

        int n = s.Length;
        if (n == 0)
        {
            Console.WriteLine("IMPOSSIBLE");
            return;
        }

        int[] order = BuildCyclicShiftArray(s, out int[] classes);
        int found = 0;

        for (int i = 0; i < n; i++)
        {
            if (i > 0 && classes[order[i]] == classes[order[i - 1]]) continue;
            found++;
            if (found == k)
            {
                int start = order[i];
                Console.WriteLine(s.Substring(start) + s.Substring(0, start));
                return;
            }
        }

        Console.WriteLine("IMPOSSIBLE");
    }

    private static int[] BuildCyclicShiftArray(string s, out int[] classes)
    {
        int n = s.Length;
        var p = new int[n];
        var c = new int[n];
        var count = new int[Math.Max(256, n)];

        for (int i = 0; i < n; i++) count[s[i]]++;
        for (int i = 1; i < 256; i++) count[i] += count[i - 1];
        for (int i = 0; i < n; i++) p[--count[s[i]]] = i;

        int cls = 1;
        for (int i = 1; i < n; i++)
        {
            if (s[p[i]] != s[p[i - 1]]) cls++;
            c[p[i]] = cls - 1;
        }

        var pn = new int[n];
        var cn = new int[n];
        for (int h = 0; (1 << h) < n; h++)
        {
            int len = 1 << h;
            for (int i = 0; i < n; i++) pn[i] = p[i] - len < 0 ? p[i] - len + n : p[i] - len;

            Array.Clear(count, 0, cls);
            for (int i = 0; i < n; i++) count[c[pn[i]]]++;
            for (int i = 1; i < cls; i++) count[i] += count[i - 1];
            for (int i = n - 1; i >= 0; i--) p[--count[c[pn[i]]]] = pn[i];

            cn[p[0]] = 0;
            int newCls = 1;
            for (int i = 1; i < n; i++)
            {
                var cur = (c[p[i]], c[(p[i] + len) % n]);
                var prev = (c[p[i - 1]], c[(p[i - 1] + len) % n]);
                if (cur != prev) newCls++;
                cn[p[i]] = newCls - 1;
            }

            var temp = c;
            c = cn;
            cn = temp;
            cls = newCls;
        }

        classes = c;
        return p;
    }
}