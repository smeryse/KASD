namespace CT12.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;

internal static class RockPaperScissors
{
    public static void Solve()
    {
        var line1 = Console.ReadLine()!.Trim().Split();
        long r1 = long.Parse(line1[0]);
        long s1 = long.Parse(line1[1]);
        long p1 = long.Parse(line1[2]);
        var line2 = Console.ReadLine()!.Trim().Split();
        long r2 = long.Parse(line2[0]);
        long s2 = long.Parse(line2[1]);
        long p2 = long.Parse(line2[2]);

        long total = r1 + s1 + p1;

        long aMax = Math.Min(r1, p2);
        long bMax = Math.Min(s1, r2);
        long cMax = Math.Min(p1, s2);

        var aVals = new HashSet<long> { 0, aMax };
        var bVals = new HashSet<long> { 0, bMax };
        var cVals = new HashSet<long> { 0, cMax };

        for (int iter = 0; iter < 3; iter++)
        {
            var newA = new HashSet<long>(aVals);
            var newB = new HashSet<long>(bVals);
            var newC = new HashSet<long>(cVals);

            foreach (long b in bVals)
            {
                long candidate = r1 - r2 + b;
                if (candidate >= 0 && candidate <= aMax) newA.Add(candidate);
            }
            foreach (long c in cVals)
            {
                long candidate = s1 - s2 + c;
                if (candidate >= 0 && candidate <= bMax) newB.Add(candidate);
            }
            foreach (long a in aVals)
            {
                long candidate = p1 - p2 + a;
                if (candidate >= 0 && candidate <= cMax) newC.Add(candidate);
            }

            aVals = newA; bVals = newB; cVals = newC;
        }

        long minLosses = total;

        foreach (long a in aVals)
        foreach (long b in bVals)
        foreach (long c in cVals)
        {
            if (a < 0 || a > aMax) continue;
            if (b < 0 || b > bMax) continue;
            if (c < 0 || c > cMax) continue;

            long r1l = r1 - a, r2l = r2 - b;
            long s1l = s1 - b, s2l = s2 - c;
            long p1l = p1 - c, p2l = p2 - a;

            long draws = Math.Min(r1l, r2l) + Math.Min(s1l, s2l) + Math.Min(p1l, p2l);
            long mWins = a + b + c;
            long losses = total - mWins - draws;

            if (losses < minLosses) minLosses = losses;
        }

        Console.WriteLine(minLosses);
    }
}