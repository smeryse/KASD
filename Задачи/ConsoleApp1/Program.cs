using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritms
{
    class Program
    {
        static void Main()
        {
            int[] a = { 11, 4, 63, 4, 5, 16, 7, 86, 9, 10, 11, 12, 44413, 14, 15 };
            InsertionSort(a);
            foreach (int i in a) Console.Write(i + " ");
            Console.WriteLine();
        }

        static void BubbleSort(int[] arr)
        {
            void Swap(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            for (int i = arr.Length - 2; i > 0; i--)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        Swap(ref arr[i], ref arr[i + 1]);
                    }
                }
            }
        }

        static void ShakerSort(int[] arr)
        {
            void Swap(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            int left = 0;
            int right = arr.Length - 1;

            while (left <= right)
            {
                for (int i = left; i <= right - 1; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(ref arr[i], ref arr[i + 1]);
                    }
                }
                right--;

                for (int i = right; i >= left + 1; i--)
                {
                    if (arr[i] < arr[i - 1])
                    {
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                    }
                }
                left++;
            }
        }

        static void CombSort(int[] arr)
        {
            void Swap(ref int a, ref int b)
            {
                int temp = a;
                a = b;
                b = temp;
            }

            for (int gap = arr.Length - 1; gap > 0; gap--)
            {
                for (int i = 0; i < arr.Length - gap; i++)
                {
                    if (arr[i] > arr[i + gap])
                    {
                        Swap(ref arr[i], ref arr[i + gap]);
                    }
                }
            }
        }

        static void InsertionSort(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = key;
            }
        }

        static void ShellSort()
        { }

        static void TreeSort()
        { }

        static void GnomeSort()
        { }

        static void SelectionSort()
        { }

        static void HeapSort()
        { }

        static void QuickSort()
        { }
        static void MergeSort()
        { }

        static void RadixSort()
        { }

        static void BitonicSort()
        { }
    }

}
