using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    
class MergeSorter
{
    public int[] Array { get; private set; }

    public MergeSorter(int[] arr) => Array = arr;
    private void merge_sort(int left, int right)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;

        int amount_left = mid - left + 1;
        int amount_right = right - mid;

        int[] left_half = new int[amount_left];
        int[] right_half = new int[amount_right];

        Array.CopyTo(
            Array, left, left_half, amount_left);
        Array.CopyTo(left_half, mid);
    }
    void merge()
    {

    }
}
