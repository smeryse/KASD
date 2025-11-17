//using System;
//using System.Collections.Generic;
//using System.Data.SqlTypes;
//using System.Diagnostics.Tracing;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace Algoritms
//{
//    class Program
//    {
//        //static void Main()
//        //{
//        //    int[] arr = { 8, 5, 3, 7, 6, 2, 4, 1 };

//        //    //TreeSort(arr);
//        //    foreach (int i in arr) Console.Write(i + " ");
//        //    Console.WriteLine();
//        //}

//        static void BubbleSort(int[] arr)
//        {
//            void Swap(ref int a, ref int b)
//            {
//                int temp = a;
//                a = b;
//                b = temp;
//            }
//            for (int i = arr.Length - 2; i > 0; i--)
//            {
//                for (int j = 0; j <= i; j++)
//                {
//                    if (arr[j] > arr[j + 1])
//                    {
//                        Swap(ref arr[i], ref arr[i + 1]);
//                    }
//                }
//            }
//        }

//        static void ShakerSort(int[] arr)
//        {
//            void Swap(ref int a, ref int b)
//            {
//                int temp = a;
//                a = b;
//                b = temp;
//            }
//            int left = 0;
//            int right = arr.Length - 1;

//            while (left <= right)
//            {
//                for (int i = left; i <= right - 1; i++)
//                {
//                    if (arr[i] > arr[i + 1])
//                    {
//                        Swap(ref arr[i], ref arr[i + 1]);
//                    }
//                }
//                right--;

//                for (int i = right; i >= left + 1; i--)
//                {
//                    if (arr[i] < arr[i - 1])
//                    {
//                        int temp = arr[i];
//                        arr[i] = arr[i + 1];
//                        arr[i + 1] = temp;
//                    }
//                }
//                left++;
//            }
//        }

//        static void CombSort(int[] arr)
//        {
//            void Swap(ref int a, ref int b)
//            {
//                int temp = a;
//                a = b;
//                b = temp;
//            }

//            for (int gap = arr.Length - 1; gap > 0; gap--)
//            {
//                for (int i = 0; i < arr.Length - gap; i++)
//                {
//                    if (arr[i] > arr[i + gap])
//                    {
//                        Swap(ref arr[i], ref arr[i + gap]);
//                    }
//                }
//            }
//        }

//        static void InsertionSort(int[] arr)
//        {
//            for (int i = 1; i < arr.Length; i++)
//            {
//                int key = arr[i];
//                int pos = i - 1;

//                while (pos >= 0 && arr[pos] > key)
//                {
//                    arr[pos + 1] = arr[pos];
//                    pos--;
//                }

//                arr[pos + 1] = key;
//            }
//        }

//        // Разобраться как работает сортировка Шелла
//        static void ShellSort(int[] arr)
//        {
//            for (int gap = (arr.Length - 1) / 2; gap != 0; gap /= 2)
//                for (int i = 0; i < arr.Length - gap; i++)
//                {
//                    int key = arr[i + gap];
//                    int pos = i;

//                    while (pos >= 0 && arr[pos] > key)
//                    {
//                        arr[pos + gap] = arr[pos];
//                        pos -= gap;
//                    }

//                    arr[pos + gap] = key;
//                }
//        }

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

//        static void TreeSort()
//        { }
//    }
//}


//namespace TreeSortApp
//{
//    public class TreeNode
//    {
//        public TreeNode(int data)
//        {
//            Data = data;
//        }

//        public int Data { get; set; }

//        public TreeNode Left { get; set; }

//        public TreeNode Right { get; set; }

//        public void Insert(TreeNode node)
//        {
//            if (node.Data < Data)
//            {
//                if (Left == null) Left = node;
//                else Left.Insert(node);
//            }
//            else
//            {
//                if (Right == null) Right = node;
//                else Right.Insert(node);
//            }
//        }

//        public int[] Transform(List<int> elements = null)
//        {
//            if (elements == null)
//            {
//                elements = new List<int>();
//            }

//            if (Left != null)
//            {
//                Left.Transform(elements);
//            }

//            elements.Add(Data);

//            if (Right != null)
//            {
//                Right.Transform(elements);
//            }

//            return elements.ToArray();
//        }
//    }

//    class Program
//    {
//        //метод для сортировки с помощью двоичного дерева
//        private static int[] TreeSort(int[] array)
//        {
//            var treeNode = new TreeNode(array[0]);
//            for (int i = 1; i < array.Length; i++)
//            {
//                treeNode.Insert(new TreeNode(array[i]));
//            }

//            return treeNode.Transform();
//        }

//        static void Main(string[] args)
//        {
//            Console.Write("n = ");
//            var n = int.Parse(Console.ReadLine());

//            var a = new int[n];
//            var random = new Random();
//            for (int i = 0; i < a.Length; i++)
//            {
//                a[i] = random.Next(0, 100);
//            }

//            Console.WriteLine("Random Array: {0}", string.Join(" ", a));

//            Console.WriteLine("Sorted Array: {0}", string.Join(" ", TreeSort(a)));
//        }
//    }
//}