using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {

    }
}
public class MaxHeap
{
    private List<int> heap = new List<int>();

    public int Count => heap.Count;

    public void Insert(int val)
    {
        heap.Add(val);
        ShiftUp(Count - 1);
    }

    public int ExtractMax()
    {
        if (Count == 0) throw new InvalidOperationException("Heap is empty");
        int max = heap[0];
        heap[0] = heap[Count - 1];
        heap.RemoveAt(Count - 1);
        ShiftDown(0);
        return max;
    }

    private void ShiftUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (heap[index] <= heap[parent]) break;
            Swap(index, parent);
            index = parent;
        }
    }

    private void ShiftDown(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int largest = index;

            if (left < Count && heap[left] > heap[largest])
                largest = left;
            if (right < Count && heap[right] > heap[largest])
                largest = right;

            if (largest == index) break;

            Swap(index, largest);
            index = largest;
        }
    }

    private void Swap(int i, int j)
    {
        int temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}
