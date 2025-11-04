class Program
{
    static void Main()
    {

    }
    static int BinarySearch(int i, int left, int right, int[] arr)
    {
        int mid = (left + right) / 2;
        if (arr[i] > mid) return BinarySearch(i, mid, right);
        else return BinarySearch(i, left, mid);
    }
}