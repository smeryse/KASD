using System;
using System.Collections.Generic;
using Task6.Collections;

public class MyPriorityQueueTestRunner
{
    public static void RunAllTests()
    {
        
        TestAddAndPeek();
        TestPoll();
        TestOffer();
        TestAddAll();
        TestRemove();
        TestRemoveAll();
        TestRetainAll();
        TestContains();
        TestContainsAll();
        TestToArray();
        TestClear();
        Console.WriteLine("All tests finished.");
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            Console.WriteLine("FAILED: " + message);
        else
            Console.WriteLine("PASSED: " + message);
    }

    private static void TestAddAndPeek()
    {
        var pq = new MyPriorityQueue<int>();
        pq.Add(5);
        pq.Add(10);
        Assert(pq.Peek() == 10, "Add and Peek");
    }

    private static void TestPoll()
    {
        var pq = new MyPriorityQueue<int>();
        pq.Add(3);
        pq.Add(7);
        int max = pq.Poll();
        Assert(max == 7 && pq.Peek() == 3, "Poll removes max");
    }

    private static void TestOffer()
    {
        var pq = new MyPriorityQueue<int>();
        bool added = pq.Offer(15);
        Assert(added && pq.Peek() == 15, "Offer adds element");
    }

    private static void TestAddAll()
    {
        var pq = new MyPriorityQueue<int>();
        pq.AddAll(new int[] { 1, 4, 6 });
        Assert(pq.Peek() == 6, "AddAll adds elements");
    }

    private static void TestRemove()
    {
        var pq = new MyPriorityQueue<int>();
        pq.AddAll(new int[] { 2, 5, 7 });
        bool removed = pq.Remove(5);
        Assert(removed && !pq.Contains(5), "Remove element");
    }

    private static void TestRemoveAll()
    {
        var pq = new MyPriorityQueue<int>();
        pq.AddAll(new int[] { 1, 2, 3, 4 });
        bool success = pq.RemoveAll(new int[] { 2, 3 });
        Assert(success && !pq.Contains(2) && !pq.Contains(3), "RemoveAll elements");
    }

    private static void TestRetainAll()
    {
        var pq = new MyPriorityQueue<int>();
        pq.AddAll(new int[] { 1, 2, 3, 4 });
        pq.RetainAll(new int[] { 2, 4 });
        Assert(pq.Contains(2) && pq.Contains(4) && !pq.Contains(1) && !pq.Contains(3), "RetainAll elements");
    }

    private static void TestContains()
    {
        var pq = new MyPriorityQueue<string>();
        pq.Add("hello");
        Assert(pq.Contains("hello"), "Contains true");
        Assert(!pq.Contains("world"), "Contains false");
    }

    private static void TestContainsAll()
    {
        var pq = new MyPriorityQueue<int>();
        pq.AddAll(new int[] { 1, 2, 3 });
        Assert(pq.ContainsAll(new object[] { 1, 2 }), "ContainsAll true");
        Assert(!pq.ContainsAll(new object[] { 1, 4 }), "ContainsAll false");
    }

    private static void TestToArray()
    {
        var pq = new MyPriorityQueue<int>();
        pq.AddAll(new int[] { 5, 2, 8 });
        int[] arr = pq.ToArray();
        Assert(arr.Length == 3, "ToArray length");
        Assert(Array.Exists(arr, x => x == 8), "ToArray content");
    }

    private static void TestClear()
    {
        var pq = new MyPriorityQueue<int>();
        pq.Add(1);
        pq.Clear();
        Assert(pq.Size() == 0, "Clear empties queue");
    }
}
