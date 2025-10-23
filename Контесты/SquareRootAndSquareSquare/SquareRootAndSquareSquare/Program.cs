using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        double C = 10; // пример
        Application.Run(new BinaryTreeForm(C));
    }
}

class Interval
{
    public double Left, Right, Mid;
    public int Depth;
    public Interval(double l, double r, int d)
    {
        Left = l; Right = r; Depth = d;
        Mid = (l + r) / 2.0;
    }
}

class BinaryTreeForm : Form
{
    double C;
    double eps = 1e-7;
    List<Interval> intervals = new List<Interval>();

    public BinaryTreeForm(double C)
    {
        this.C = C;
        this.Width = 800;
        this.Height = 600;
        this.Text = "Бинарный поиск как дерево";

        this.DoubleBuffered = true;

        InitTree();
    }

    void InitTree()
    {
        double left = 0;
        double right = Math.Max(1, C);

        Queue<Interval> queue = new Queue<Interval>();
        queue.Enqueue(new Interval(left, right, 0));

        while (queue.Count > 0)
        {
            var iv = queue.Dequeue();
            intervals.Add(iv);

            if (iv.Right - iv.Left < eps)
                continue;

            double f = iv.Mid * iv.Mid + Math.Sqrt(iv.Mid);
            if (f < C)
                queue.Enqueue(new Interval(iv.Mid, iv.Right, iv.Depth + 1));
            else
                queue.Enqueue(new Interval(iv.Left, iv.Mid, iv.Depth + 1));
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.Clear(Color.White);

        int marginX = 50;
        int marginY = 50;
        int levelHeight = 40;

        int width = this.ClientSize.Width - 2 * marginX;

        DrawNode(g, intervals[0], marginX, marginY, width);
    }

    void DrawNode(Graphics g, Interval iv, int x, int y, int width)
    {
        int nodeX = x + (int)((iv.Mid - iv.Left) / (iv.Right - iv.Left) * width);
        int nodeY = y;

        // рисуем узел
        g.FillEllipse(Brushes.Green, nodeX - 5, nodeY - 5, 10, 10);
        g.DrawString(iv.Mid.ToString("F4"), new Font("Arial", 8), Brushes.Black, nodeX + 5, nodeY - 10);

        // ищем детей
        var children = intervals.FindAll(ch => ch.Depth == iv.Depth + 1 &&
            ((ch.Left == iv.Left && ch.Right == iv.Mid) || (ch.Left == iv.Mid && ch.Right == iv.Right)));

        int childY = y + 40;
        int childWidth = width / 2;

        foreach (var child in children)
        {
            int childX = x + (int)((child.Mid - child.Left) / (child.Right - child.Left) * childWidth);
            // рисуем линию
            g.DrawLine(Pens.Black, nodeX, nodeY, childX, childY);

            DrawNode(g, child, x, childY, childWidth);
        }
    }
}
