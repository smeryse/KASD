using System;
using System.IO;
using System.Text;

namespace CT4.Tasks;

internal static class BalancedBST
{
    public static void Solve()
    {
        var fs = new FastScanner(Console.OpenStandardInput());
        var tree = new AVLTree();
        var sb = new StringBuilder();

        string command;
        while ((command = fs.NextString()) != null)
        {
            int x = fs.NextInt();

            switch (command)
            {
                case "insert":
                    tree.Insert(x);
                    break;
                case "delete":
                    tree.Delete(x);
                    break;
                case "exists":
                    sb.AppendLine(tree.Exists(x) ? "true" : "false");
                    break;
                case "next":
                    var next = tree.Next(x);
                    sb.AppendLine(next.HasValue ? next.Value.ToString() : "none");
                    break;
                case "prev":
                    var prev = tree.Prev(x);
                    sb.AppendLine(prev.HasValue ? prev.Value.ToString() : "none");
                    break;
            }
        }

        Console.Write(sb.ToString());
    }


    private sealed class AVLTree
    {
        private class Node
        {
            public int Value;
            public Node Left;
            public Node Right;
            public int Height;

            public Node(int value)
            {
                Value = value;
                Height = 1;
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

        private int Height(Node node)
        {
            return node?.Height ?? 0;
        }

        private int Balance(Node node)
        {
            return node == null ? 0 : Height(node.Left) - Height(node.Right);
        }

        private void UpdateHeight(Node node)
        {
            if (node != null)
                node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        }

        private Node RotateRight(Node y)
        {
            Node x = y.Left;
            Node t = x.Right;

            x.Right = y;
            y.Left = t;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        private Node RotateLeft(Node x)
        {
            Node y = x.Right;
            Node t = y.Left;

            y.Left = x;
            x.Right = t;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }

        private Node BalanceNode(Node node)
        {
            if (node == null) return null;

            UpdateHeight(node);
            int balance = Balance(node);

            if (balance > 1)
            {
                if (Balance(node.Left) < 0)
                    node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            if (balance < -1)
            {
                if (Balance(node.Right) > 0)
                    node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private Node InsertNode(Node node, int value)
        {
            if (node == null)
                return new Node(value);

            if (value < node.Value)
                node.Left = InsertNode(node.Left, value);
            else if (value > node.Value)
                node.Right = InsertNode(node.Right, value);
            else
                return node;

            return BalanceNode(node);
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

            return BalanceNode(node);
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