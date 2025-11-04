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

        long result = KSum(a, b, k);
        Console.WriteLine(result);
    }

    // Функция подсчета количества пар с суммой <= x
    static long CountPairs(long[] a, long[] b, long x)
    {
        long count = 0;
        int n = b.Length;
        for (int i = 0; i < a.Length; i++)
        {
            // бинарный поиск: сколько элементов b[j] <= x - a[i]
            int left = 0, right = n;
            while (left < right)
            {
                int mid = (left + right) / 2;
                if (b[mid] <= x - a[i])
                    left = mid + 1;
                else
                    right = mid;
            }
            count += left;
        }
        return count;
    }

    // Бинарный поиск k-й суммы
    static long KSum(long[] a, long[] b, long k)
    {
        long left = a[0] + b[0];
        long right = a[a.Length - 1] + b[b.Length - 1];

        while (left < right)
        {
            long mid = left + (right - left) / 2;
            if (CountPairs(a, b, mid) < k)
                left = mid + 1;
            else
                right = mid;
        }
        return left;
    }
}
