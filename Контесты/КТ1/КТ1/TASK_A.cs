using System;
using System.Linq;
using System.Threading;

class MergeSorter
{
    public int[] Array { get; private set; }

    public MergeSorter(int[] arr) => Array = arr;

    public void Sort() => merge_sort(0, Array.Length - 1);

    private void merge_sort(int left, int right)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;

        merge_sort(left, mid);
        merge_sort(mid + 1, right);

        merge(left, mid, right);
    }
    private void merge(int left, int mid, int right)
    {
        int amount_left = mid - left + 1;
        int amount_right = right - mid;

        int[] left_half = new int[amount_left];
        int[] right_half = new int[amount_right];

        System.Array.Copy(Array, left, left_half, 0, amount_left);
        System.Array.Copy(Array, mid + 1, right_half, 0, amount_right);

        int i = 0, j = 0, k = left;

        while (i < amount_left && j < amount_right)
        {
            if (left_half[i] <= right_half[j])
            {
                Array[k] = left_half[i];
                k++; i++;
            }

            else
            {
                Array[k] = right_half[j];
                k++; j++;
            }
            
        }

        while (i < amount_left) Array[k++] = left_half[i++];
        while (j < amount_right) Array[k++] = right_half[j++];
    }
}
class Program
{
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine());
        int[] arr = Console.ReadLine()
                   .Split(' ', (char)StringSplitOptions.RemoveEmptyEntries)
                   .Select(int.Parse)
                   .ToArray();
        MergeSorter sorter = new MergeSorter(arr);
        sorter.Sort();
        Console.WriteLine(string.Join(" ", arr));
    }
}
//    static void MainTest(string[] args)
//    {
//        Test(new int[] { 5, 3, 8, 4, 2 }, "Случайный массив");
//        Test(new int[] { 1, 2, 3, 4, 5 }, "Уже отсортированный");
//        Test(new int[] { 5, 4, 3, 2, 1 }, "Отсортированный в обратном порядке");
//        Test(new int[] { 7 }, "Один элемент");
//        Test(new int[] { }, "Пустой массив");
//        Test(new int[] { 2, 3, 2, 1, 1 }, "С дубликатами");
//        Test(new int[] { 10, -5, 3, 0, -1 }, "С отрицательными числами");
//    }

//    static void Test(int[] arr, string description)
//    {
//        MergeSorter sorter = new MergeSorter((int[])arr.Clone()); // клонируем, чтобы исходный не трогать
//        sorter.Sort();
//        Console.WriteLine($"{description}: {string.Join(", ", sorter.Array)}");
//    }
//}
