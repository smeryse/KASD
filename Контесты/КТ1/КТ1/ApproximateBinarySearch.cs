//using System;

//class Program
//{
//    static void Main(string[] args)
//    {
//        string[] firstLine = Console.ReadLine().Split(' ');
//        int n = int.Parse(firstLine[0]);
//        int k = int.Parse(firstLine[1]);

//        int[] arr = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
//        int[] queries = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

//        for (int i = 0; i < k; i++)
//        {
//            int q = queries[i];
//            int idx = Bound(arr, q);

//            int cand1 = idx < n ? arr[idx] : int.MaxValue;
//            int cand2 = idx > 0 ? arr[idx - 1] : int.MinValue;

//            int result;
//            if (cand1 == int.MaxValue) result = cand2;
//            else if (cand2 == int.MinValue) result = cand1;
//            else
//            {
//                int diff1 = Math.Abs(cand1 - q);
//                int diff2 = Math.Abs(cand2 - q);

//                if (diff1 < diff2) result = cand1;
//                else if (diff2 < diff1) result = cand2;
//                else result = Math.Min(cand1, cand2);
//            }
//            Console.WriteLine(result);
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
