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
        int[] left_half = { };
        int[] right_half = { };
        Array.CopyTo(left_half, 0);
        Array.CopyTo(left_half, mid);
    }
    void merge()
    {

    }
}
