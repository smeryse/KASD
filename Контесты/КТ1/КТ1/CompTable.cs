class Program
{
    static void Main(string[] args)
    {
        int n = 3;

    }
    static void CountingSort(int n)
    {
        if (n == 0) return;

        int min = 1;
        int max = n*n;

        for (int num = 1; num <= max; num++)
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

}
