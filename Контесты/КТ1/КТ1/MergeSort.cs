using System;
using System.Linq;

class MergeSorter
{
    public int[] Array { get; private set; }
    public long Invertions { get; private set; } = 0;
    public MergeSorter(int[] arr) => Array = arr;

    public void Sort() => MergeSort(0, Array.Length - 1);

    private void MergeSort(int left, int right)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;

        MergeSort(left, mid);
        MergeSort(mid + 1, right);

        Merge(left, mid, right);
    }
    private void Merge(int left, int mid, int right)
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
                Array[k++] = left_half[i++];

            else
            {
                Array[k++] = right_half[j++];
                Invertions += amount_left - i;
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
        Console.ReadLine();
        int[] arr = Console.ReadLine()
                   .Split(' ')
                   .Select(int.Parse)
                   .ToArray();
        MergeSorter sorter = new MergeSorter(arr);
        sorter.Sort();
        Console.WriteLine(sorter.Invertions);
    }
}
