using System;
using System.Linq;

class Program
{
    static void CountingSort(int[] arr)
    {
        if (arr.Length == 0) return;

        int max = arr[0];
        int min = arr[0];

        foreach (int num in arr)
        {
            if (num > max) max = num;
            if (num < min) min = num;
        }

        int range = max - min + 1;
        int[] count = new int[range];

        foreach (int num in arr)
        {
            count[num - min]++;
        }
        int index = 0;
        for (int i = 0; i < range; i++)
        {
            while (count[i] > 0)
            {
                arr[index++] = i + min;
                count[i]--;
            }
        }
    }
    static void Main(string[] args)
    {
        Console.ReadLine();
        int[] arr = Console.ReadLine()
                   .Split(' ')
                   .Select(int.Parse)
                   .ToArray();
        CountingSort(arr);
        Console.WriteLine(string.Join(" ", arr));
    }
}
