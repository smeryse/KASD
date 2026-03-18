using System;
using System.Text;

namespace CT2.Tasks
{
    static class CalcExp
    {
        static int[] parent;
        static int[] size;
        // add[root] — накопленный опыт, добавленный всему клану с корнем root
        // diff[x] — "личная" поправка игрока x: опыт(x) = add[find(x)] + diff[x]
        static long[] add;
        static long[] diff;

        public static void Solve()
        {
            var first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);

            parent = new int[n + 1];
            size = new int[n + 1];
            add = new long[n + 1];
            diff = new long[n + 1];

            for (int i = 1; i <= n; i++)
            {
                parent[i] = i;
                size[i] = 1;
                add[i] = 0;
                diff[i] = 0;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < m; i++)
            {
                string line = Console.ReadLine();
                var parts = line.Split();

                switch (parts[0])
                {
                    case "join":
                        int x = int.Parse(parts[1]);
                        int y = int.Parse(parts[2]);
                        Union(x, y);
                        break;

                    case "get":
                        int gx = int.Parse(parts[1]);
                        int root = Find(gx);
                        long res = add[root] + diff[gx];
                        sb.Append(res).Append('\n');
                        break;

                    case "add":
                        int ax = int.Parse(parts[1]);
                        int v = int.Parse(parts[2]);
                        int ar = Find(ax);
                        add[ar] += v;
                        break;
                }
            }
            Console.Write(sb.ToString());
        }

        static int Find(int x)
        {
            if (parent[x] == x) return x;
            int p = parent[x];
            int r = Find(p);
            diff[x] += diff[p];
            parent[x] = r;
            return r;
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

            // присоединяем корень ry к rx
            parent[ry] = rx;
            diff[ry] = add[ry] - add[rx];
            size[rx] += size[ry];
        }
    }
}
