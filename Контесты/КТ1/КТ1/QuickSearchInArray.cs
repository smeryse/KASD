//using System;

//class Program
//{
//    static void Main()
//    {
//        int n = int.Parse(Console.ReadLine());
//        int[] a = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
//        Array.Sort(a);

//        int k = int.Parse(Console.ReadLine());
//        for (int i = 0; i < k; i++)
//        {
//            string[] parts = Console.ReadLine().Split();
//            int l = int.Parse(parts[0]);
//            int r = int.Parse(parts[1]);

//            int left = Bound(a, l);
//            int right = Bound(a, r + 1);
//            Console.WriteLine(right - left);
//        }
//    }

//    static int Bound(int[] arr, int x)
//    {
//        int left = 0, right = arr.Length;
//        while (left < right)
//        {
//            int mid = (left + right) / 2;
//            if (arr[mid] >= x) right = mid;
//            else left = mid + 1;
//        }
//        return left;
//    }
//}
