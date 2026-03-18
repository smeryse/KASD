using System;
using System.Linq;

namespace CT1.Tasks
{
    static class CountSort
    {
        public static void Solve()
        {
            Console.ReadLine(); // n не нужен
            int[] arr = Console.ReadLine()
                               .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                               .Select(int.Parse)
                               .ToArray();

            CountingSort(arr);
            Console.WriteLine(string.Join(" ", arr));
        }

        private static void CountingSort(int[] arr)
        {
            if (arr.Length == 0) return;

            int min = arr.Min();
            int max = arr.Max();
            int range = max - min + 1;
            int[] count = new int[range];

            foreach (var num in arr) count[num - min]++;

            int idx = 0;
            for (int i = 0; i < range; i++)
            {
                while (count[i]-- > 0)
                    arr[idx++] = i + min;
            }
        }
    }
}


