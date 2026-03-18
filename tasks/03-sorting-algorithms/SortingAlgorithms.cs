//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Algoritms
//{
//    class Program
//    {
//        static void Main()
//        {
//            int[] a = { 11, 4, 63, 4, 5, 16, 7, 86, 9, 10, 11, 12, 44413, 14, 15 };
//            BubbleSort(a);
//            foreach (int i in a) Console.Write(i + " ");
//        }

//        static void Swap(ref int a, ref int b)
//        {
//            int temp = a;
//            a = b;
//            b = temp;
//        }
//        static void BubbleSort(int[] arr)
//        {
//            for (int i = arr.Length - 2; i > 0; i--)
//            {
//                for (int j = 0; j <= i; j++)
//                {
//                    if (arr[j] > arr[j + 1])
//                    {
//                        Swap(ref arr[j], ref arr[j + 1]);
//                    }
//                }
//            }
//        }

//        static void ShakerSort()
//        {

//        }

//        static void CombSort()
//        {
//        }

//        static void InsertionSort()
//        {

//        }

//        static void ShellSort()
//        { }

//        static void TreeSort()
//        { }

//        static void GnomeSort()
//        { }

//        static void SelectionSort()
//        { }

//        static void HeapSort()
//        { }

//        static void QuickSort()
//        { }
//        static void MergeSort()
//        { }

//        static void RadixSort()
//        { }

//        static void BitonicSort()
//        { }
//    }

//}
