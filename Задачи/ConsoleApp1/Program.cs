using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritms
{
    class Program
    {
        //static void Main()
        //{
        //    int[] arr = { 8, 5, 3, 7, 6, 2, 4, 1 };

        //    //TreeSort(arr);
        //    foreach (int i in arr) Console.Write(i + " ");
        //    Console.WriteLine();
        //}

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
                int pos = i - 1;

                while (pos >= 0 && arr[pos] > key)
                {
                    arr[pos + 1] = arr[pos];
                    pos--;
                }

                arr[pos + 1] = key;
            }
        }

        // Разобраться как работает сортировка Шелла
        static void ShellSort(int[] arr)
        {
            for (int gap = (arr.Length - 1) / 2; gap != 0; gap /= 2)
                for (int i = 0; i < arr.Length - gap; i++)
                {
                    int key = arr[i + gap];
                    int pos = i;

                    while (pos >= 0 && arr[pos] > key)
                    {
                        arr[pos + gap] = arr[pos];
                        pos -= gap;
                    }

                    arr[pos + gap] = key;
                }
        }

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
        static void Main()
        {
            Tree newTree = new Tree();

            int[] arr = { 1, 2, 4, 6, 1, 88, 31 };
            int len = arr.Length;

            foreach (int elem in arr)
                newTree.Insert(elem);

            int[] result = new int[len];
            newTree.InOrderTraversal(newTree.root, result, 0);

            foreach (int elem in result)
                Console.WriteLine(elem);
        }
    }


    public class Tree
    {
        public Node root;
        public class Node
        {
            public int val { get; set; }
            public Node left;
            public Node right;

            public Node() { }
            public Node(int _val) => this.val = _val;
        }

        public Tree()
        {
            root = null;
        }
        bool IsEmpty()
        {
            return root == null;
        }

        public void Insert(int value, Node currNode = null)
        {
            if (IsEmpty())
            {
                root = new Node(value);
                return;
            }
            if (currNode == null)
                currNode = root;

            // Правый потомок
            if (value >= currNode.val)
            {
                if (currNode.right == null)
                {
                    currNode.right = new Node(value);
                }
                else Insert(value, currNode.right);
            }
            // Левый потомок
            else 
            {
                if (currNode.left == null)
                {
                    currNode.left = new Node(value);
                }
                else Insert(value, currNode.left);
            }
    }


        public int InOrderTraversal(Node currNode, int[] arr, int index)
        {
            if (currNode == null)
                return index;

            index = InOrderTraversal(currNode.left, arr, index);
            
            arr[index] = currNode.val;
            index++;

            index = InOrderTraversal(currNode.right, arr, index);

            return index;
        }


    }
}