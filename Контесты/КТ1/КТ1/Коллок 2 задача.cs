using System;

class Program
{
    static void Main()
    {
        int[] arr = { 2, 4, 7, 11, 15, 21, 30 };
        int num = 11;

        Console.WriteLine(BinarySearch());
    }
    static int BinarySearch(int num, int left, int right, int[] arr)
    {
        int mid = (left + right) / 2;
        if (num > arr[mid]) return BinarySearch(num, mid, right, arr);
        else return BinarySearch(num, left, mid, arr);
    }
}