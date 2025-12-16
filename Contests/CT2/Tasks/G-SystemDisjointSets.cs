using System;
using System.Text;

class Program
{
    static int[] parent;
    static int[] size;
    static int[] minVal;
    static int[] maxVal;

    static void Main()
    {
        int n = int.Parse(Console.ReadLine());

        parent = new int[n + 1];
        size = new int[n + 1];
        minVal = new int[n + 1];
        maxVal = new int[n + 1];

        for (int i = 1; i <= n; i++)
        {
            parent[i] = i;
            size[i] = 1;
            minVal[i] = i;
            maxVal[i] = i;
        }

        var sb = new StringBuilder();
        string line;
        while ((line = Console.ReadLine()) != null)
        {
            var parts = line.Split();

            switch (parts[0])
            {
                case "union":
                    int x = int.Parse(parts[1]);
                    int y = int.Parse(parts[2]);
                    Union(x, y);
                    break;

                case "get":
                    int x1 = int.Parse(parts[1]);
                    int rx = Find(x1);
                    sb.Append(minVal[rx]).Append(' ')
                      .Append(maxVal[rx]).Append(' ')
                      .Append(size[rx]).Append('\n');
                    break;
            }
        }
        Console.Write(sb.ToString());
    }

    static int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    static void Union(int x, int y)
    {
        int rx = Find(x);
        int ry = Find(y);
        if (rx == ry) return;

        // union by size
        if (size[rx] < size[ry])
        {
            int tmp = rx;
            rx = ry;
            ry = tmp;
        }

        parent[ry] = rx;
        size[rx] += size[ry];
        if (minVal[ry] < minVal[rx]) minVal[rx] = minVal[ry];
        if (maxVal[ry] > maxVal[rx]) maxVal[rx] = maxVal[ry];
    }
}
