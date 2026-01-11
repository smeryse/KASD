using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class SimpleBST
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        var bst = new BinarySearchTree();
        var sb = new StringBuilder();

        string command;
        while ((command = fs.NextString()) != null)
        {
            int x = fs.NextInt();

            switch (command)
            {
                case "insert":
                    bst.Insert(x);
                    break;
                case "delete":
                    bst.Delete(x);
                    break;
                case "exists":
                    sb.AppendLine(bst.Exists(x) ? "true" : "false");
                    break;
                case "next":
                    var next = bst.Next(x);
                    sb.AppendLine(next.HasValue ? next.Value.ToString() : "none");
                    break;
                case "prev":
                    var prev = bst.Prev(x);
                    sb.AppendLine(prev.HasValue ? prev.Value.ToString() : "none");
                    break;
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class BinarySearchTree
    {
        private class Node
        {
            public int Value;
            public Node Left;
            public Node Right;

            public Node(int value)
            {
                Value = value;
            }
        }

        private Node root;

        public void Insert(int value)
        {
            root = InsertNode(root, value);
        }

        public void Delete(int value)
        {
            root = DeleteNode(root, value);
        }

        public bool Exists(int value)
        {
            return FindNode(root, value) != null;
        }

        public int? Next(int value)
        {
            Node current = root;
            int? result = null;

            while (current != null)
            {
                if (current.Value > value)
                {
                    result = current.Value;
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return result;
        }

        public int? Prev(int value)
        {
            Node current = root;
            int? result = null;

            while (current != null)
            {
                if (current.Value < value)
                {
                    result = current.Value;
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }
            }

            return result;
        }

        private Node InsertNode(Node node, int value)
        {
            if (node == null)
                return new Node(value);

            if (value < node.Value)
                node.Left = InsertNode(node.Left, value);
            else if (value > node.Value)
                node.Right = InsertNode(node.Right, value);

            return node;
        }

        private Node DeleteNode(Node node, int value)
        {
            if (node == null)
                return null;

            if (value < node.Value)
            {
                node.Left = DeleteNode(node.Left, value);
            }
            else if (value > node.Value)
            {
                node.Right = DeleteNode(node.Right, value);
            }
            else
            {
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;

                Node minRight = FindMin(node.Right);
                node.Value = minRight.Value;
                node.Right = DeleteNode(node.Right, minRight.Value);
            }

            return node;
        }

        private Node FindNode(Node node, int value)
        {
            if (node == null)
                return null;

            if (value == node.Value)
                return node;

            return value < node.Value 
                ? FindNode(node.Left, value) 
                : FindNode(node.Right, value);
        }

        private Node FindMin(Node node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }
    }


    private sealed class FastScanner
    {
        private readonly Stream stream;
        private readonly byte[] buffer;
        private int len;
        private int ptr;

        public FastScanner(Stream stream, int bufferSize = 1 << 16)
        {
            this.stream = stream;
            buffer = new byte[bufferSize];
        }

        private byte Read()
        {
            if (ptr >= len)
            {
                len = stream.Read(buffer, 0, buffer.Length);
                ptr = 0;
                if (len <= 0) return 0;
            }
            return buffer[ptr++];
        }

        public string NextString()
        {
            int c;
            do c = Read(); while (c > 0 && c <= ' ');

            if (c == 0) return null;

            var sb = new StringBuilder();
            while (c > ' ')
            {
                sb.Append((char)c);
                c = Read();
            }
            return sb.ToString();
        }

        public int NextInt()
        {
            int c;
            do c = Read(); while (c <= ' ');

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            int val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val * sign;
        }

        public long NextLong()
        {
            int c;
            do c = Read(); while (c <= ' ');

            int sign = 1;
            if (c == '-')
            {
                sign = -1;
                c = Read();
            }

            long val = 0;
            while (c > ' ')
            {
                val = val * 10 + (c - '0');
                c = Read();
            }
            return val * sign;
        }
    }
}