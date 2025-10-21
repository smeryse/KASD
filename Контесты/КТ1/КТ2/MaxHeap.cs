using System;
using System.Collections.Generic;

class MaxHeap
{
    private List<int> heap = new List<int>();

    public void Insert(int x)
    {
        heap.Add(x);
        SiftUp(heap.Count - 1);
    }

    public int Extract()
    {
        int max = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        if (heap.Count > 0) SiftDown(0);
        return max;
    }

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (heap[parent] >= heap[index]) break;
            Swap(parent, index);
            index = parent;
        }
    }

    private void SiftDown(int index)
    {
        int n = heap.Count;
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int largest = index;

            if (left < n && heap[left] > heap[largest]) largest = left;
            if (right < n && heap[right] > heap[largest]) largest = right;

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

class Program
{
    static void Main()
    {
        int n = int.Parse(Console.ReadLine());
        var heap = new MaxHeap();
        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine().Split();
            if (parts[0] == "0")
            {
                int x = int.Parse(parts[1]);
                heap.Insert(x);
            }
            else // "1"
            {
                Console.WriteLine(heap.Extract());
            }
        }
    }
}
