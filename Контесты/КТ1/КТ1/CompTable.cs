using System;

class Program
{
    static long n;
    static long k;

    static void Main(string[] args)
    {
        // Если хочешь запускать тесты — раскомментируй строку ниже
        RunTests(); return;

        string[] firstString = Console.ReadLine().Split(' ');

        n = long.Parse(firstString[0]);
        k = long.Parse(firstString[1]);

        Console.WriteLine(BinarySearch(1, n * n));
    }

    static long BinarySearch(long left, long right)
    {
        if (left == right) return left;

        long mid = (left + right) / 2;

        if (CountLessOrEqual(mid) >= k)
            return BinarySearch(left, mid);
        else
            return BinarySearch(mid + 1, right);
    }

    static long CountLessOrEqual(long num)
    {
        long result = 0;

        for (long i = 1; i <= n; i++)
            result += Math.Min(n, num / i);

        return result;
    }


    static void RunTests()
    {
        (long n, long k, long expected)[] tests = new (long, long, long)[]
        {
            (3, 5, 3),
            (10, 1, 1),
            (10, 100, 100),
            (1, 1, 1),
            (4, 6, 4),
            (100000, 1, 1),
            (100000, 10000000000, 10000000000),
        };

        int testNum = 1;

        foreach (var t in tests)
        {
            n = t.n;
            k = t.k;

            long result = BinarySearch(1, n * n);
            bool pass = result == t.expected;

            Console.WriteLine(
                $"Test {testNum++}: n={t.n}, k={t.k} → result={result}, expected={t.expected} " +
                (pass ? "+" : "-")
            );
        }
    }
}
