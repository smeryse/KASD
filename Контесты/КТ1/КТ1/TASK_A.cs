using System;

class MergeSorter
{
    public int[] Array { get; private set; }

    public MergeSorter(int[] arr) => Array = arr;

    // Публичный метод для сортировки
    public void Sort()
    {
        if (Array == null || Array.Length <= 1) return;
        merge_sort(0, Array.Length - 1);
    }

    // Рекурсивная сортировка
    private void merge_sort(int left, int right)
    {
        if (left >= right) return;

        int mid = (left + right) / 2;

        merge_sort(left, mid);        // сортируем левую половину
        merge_sort(mid + 1, right);   // сортируем правую половину

        merge(left, mid, right);      // сливаем отсортированные части
    }

    // Слияние двух отсортированных подмассивов
    private void merge(int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;

        int[] L = new int[n1];
        int[] R = new int[n2];

        System.Array.Copy(Array, left, L, 0, n1);
        System.Array.Copy(Array, mid + 1, R, 0, n2);

        int i = 0, j = 0, k = left;

        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
                Array[k++] = L[i++];
            else
                Array[k++] = R[j++];
        }

        while (i < n1) Array[k++] = L[i++];
        while (j < n2) Array[k++] = R[j++];
    }
}
