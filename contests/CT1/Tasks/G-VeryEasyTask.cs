using System;

namespace CT1.Tasks
{
    // Ксерокопии: минимальное время сделать n копий двумя аппаратами
    static class VeryEasyTask
    {
        public static void Solve()
        {
            var parts = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int n = int.Parse(parts[0]);
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);

            Console.WriteLine(CalculateMinTime(n, x, y));
        }

        private static long CalculateMinTime(int n, int x, int y)
        {
            int firstCopy = Math.Min(x, y);
            if (n == 1) return firstCopy;

            n--;
            long l = 0, r = (long)n * Math.Min(x, y);
            while (l < r)
            {
                long mid = (l + r) / 2;
                long copies = mid / x + mid / y;
                if (copies >= n) r = mid;
                else l = mid + 1;
            }
            return firstCopy + l;
        }
    }
}


