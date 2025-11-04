using System;
using System.Linq;

class Program
{
    static long[] a;
    static long[] b;
    static int n;
    static long k;

    static void Main()
    {
        string[] firstString = Console.ReadLine().Split();
        n = int.Parse(firstString[0]);
        k = long.Parse(firstString[1]);

        a = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
        b = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);

        Array.Sort(a);
        Array.Sort(b);

        Console.WriteLine(KSum());
    }

    static long CountPairs(long curSum)
    {
        long count = 0;
        for (int i = 0; i < n; i++)
        {
            //left, right, mid - indexes
            int left = 0, right = n;
            while (left < right)
            {
                int mid = (left + right) / 2;
                if (b[mid] + a[i] <= curSum)
                    left = mid + 1;
                else
                    right = mid;
            }
            count += left;
        }
        return count;
    }

    // Бинарный поиск k-й суммы
    static long KSum()
    {
        long left = a[0] + b[0];
        long right = a[a.Length - 1] + b[b.Length - 1];

        while (left < right)
        {
            long mid = (right + left) / 2;
            if (CountPairs(mid) < k)
                left = mid + 1;
            else
                right = mid;
        }
        return left;
    }
}
