using System;

class Program
{
    static void Main()
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

        long miroWins = Math.Min(p2, r1) + Math.Min(r2, s1) + Math.Min(s2, p1);

        long r1_left = r1 - Math.Min(p2, r1);
        long s1_left = s1 - Math.Min(r2, s1);
        long p1_left = p1 - Math.Min(s2, p1);

        long r2_left = r2 - Math.Min(r2, s1);
        long s2_left = s2 - Math.Min(s2, p1);
        long p2_left = p2 - Math.Min(p2, r1);

        long draws = Math.Min(r1_left, r2_left) + Math.Min(s1_left, s2_left) + Math.Min(p1_left, p2_left);

        long losses = total - miroWins - draws;
        Console.WriteLine(losses);
    }
}