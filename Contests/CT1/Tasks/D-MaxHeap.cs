using System;
using System.Collections.Generic;

namespace CT1.Tasks
{
    static class MaxHeapTask
    {
        public static void Solve()
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
                else
                {
                    Console.WriteLine(heap.Extract());
                }
            }
        }
    }

    class MaxHeap
    {
        private readonly List<int> heap = new List<int>();

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

        private void SiftUp(int i)
        {
            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (heap[p] >= heap[i]) break;
                Swap(p, i);
                i = p;
            }
        }

        private void SiftDown(int i)
        {
            int n = heap.Count;
            while (true)
            {
                int l = 2 * i + 1;
                int r = 2 * i + 2;
                int largest = i;
                if (l < n && heap[l] > heap[largest]) largest = l;
                if (r < n && heap[r] > heap[largest]) largest = r;
                if (largest == i) break;
                Swap(i, largest);
                i = largest;
            }
        }

        private void Swap(int i, int j)
        {
            int t = heap[i];
            heap[i] = heap[j];
            heap[j] = t;
        }
    }
}


