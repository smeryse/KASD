using System;
using System.IO;

namespace CT2.Tasks
{
    static class Cuckoo
    {
        static int[] parent;
        static long[] size_arr;
        static long[] edges_arr;
        static long S0;
        static long Q0;
        static long case1_sum;

        static int Find(int x)
        {
            if (parent[x] != x)
                parent[x] = Find(parent[x]);
            return parent[x];
        }

        public static void Solve()
        {
            var stream = new StreamReader(Console.OpenStandardInput());
            var tokens = stream.ReadLine().Split();
            int n = int.Parse(tokens[0]);
            int m = int.Parse(tokens[1]);
            int q = int.Parse(tokens[2]);

            parent = new int[n + 1];
            size_arr = new long[n + 1];
            edges_arr = new long[n + 1];
            for (int i = 1; i <= n; i++)
            {
                parent[i] = i;
                size_arr[i] = 1;
                edges_arr[i] = 0;
            }
            S0 = 0;
            Q0 = 0;
            case1_sum = 0;

            for (int i = 0; i < m; i++)
            {
                tokens = stream.ReadLine().Split();
                int x = int.Parse(tokens[0]);
                int y = int.Parse(tokens[1]);
                int rx = Find(x);
                int ry = Find(y);
                if (rx == ry)
                {
                    long free_old = size_arr[rx] - edges_arr[rx];
                    if (free_old == 0)
                    {
                        S0 -= size_arr[rx];
                        Q0 -= size_arr[rx] * size_arr[rx];
                        case1_sum -= size_arr[rx] * (size_arr[rx] - 1);
                    }
                    edges_arr[rx]++;
                    long free_new = size_arr[rx] - edges_arr[rx];
                    if (free_new == 0)
                    {
                        S0 += size_arr[rx];
                        Q0 += size_arr[rx] * size_arr[rx];
                        case1_sum += size_arr[rx] * (size_arr[rx] - 1);
                    }
                }
                else
                {
                    long free_rx = size_arr[rx] - edges_arr[rx];
                    long free_ry = size_arr[ry] - edges_arr[ry];
                    if (free_rx == 0)
                    {
                        S0 -= size_arr[rx];
                        Q0 -= size_arr[rx] * size_arr[rx];
                        case1_sum -= size_arr[rx] * (size_arr[rx] - 1);
                    }
                    if (free_ry == 0)
                    {
                        S0 -= size_arr[ry];
                        Q0 -= size_arr[ry] * size_arr[ry];
                        case1_sum -= size_arr[ry] * (size_arr[ry] - 1);
                    }

                    if (size_arr[rx] < size_arr[ry])
                    {
                        int temp = rx;
                        rx = ry;
                        ry = temp;
                    }
                    parent[ry] = rx;
                    size_arr[rx] += size_arr[ry];
                    edges_arr[rx] += edges_arr[ry] + 1;

                    long free_new = size_arr[rx] - edges_arr[rx];
                    if (free_new == 0)
                    {
                        S0 += size_arr[rx];
                        Q0 += size_arr[rx] * size_arr[rx];
                        case1_sum += size_arr[rx] * (size_arr[rx] - 1);
                    }
                }
            }

            var output = new System.Text.StringBuilder();
            for (int i = 0; i < q; i++)
            {
                tokens = stream.ReadLine().Split();
                int t = int.Parse(tokens[0]);
                if (t == 1 || t == 2)
                {
                    int x = int.Parse(tokens[1]);
                    int y = int.Parse(tokens[2]);
                    int rx = Find(x);
                    int ry = Find(y);
                    bool condition;
                    if (rx == ry)
                    {
                        long free_here = size_arr[rx] - edges_arr[rx];
                        condition = (free_here > 0);
                    }
                    else
                    {
                        long free_rx = size_arr[rx] - edges_arr[rx];
                        long free_ry = size_arr[ry] - edges_arr[ry];
                        condition = (free_rx + free_ry > 0);
                    }

                    if (t == 1)
                    {
                        output.AppendLine(condition ? "Yes" : "No");
                    }
                    else
                    {
                        if (condition)
                        {
                            if (rx == ry)
                            {
                                long free_old = size_arr[rx] - edges_arr[rx];
                                if (free_old == 0)
                                {
                                    S0 -= size_arr[rx];
                                    Q0 -= size_arr[rx] * size_arr[rx];
                                    case1_sum -= size_arr[rx] * (size_arr[rx] - 1);
                                }
                                edges_arr[rx]++;
                                long free_new = free_old - 1;
                                if (free_new == 0)
                                {
                                    S0 += size_arr[rx];
                                    Q0 += size_arr[rx] * size_arr[rx];
                                    case1_sum += size_arr[rx] * (size_arr[rx] - 1);
                                }
                            }
                            else
                            {
                                long free_rx = size_arr[rx] - edges_arr[rx];
                                long free_ry = size_arr[ry] - edges_arr[ry];
                                if (free_rx == 0)
                                {
                                    S0 -= size_arr[rx];
                                    Q0 -= size_arr[rx] * size_arr[rx];
                                    case1_sum -= size_arr[rx] * (size_arr[rx] - 1);
                                }
                                if (free_ry == 0)
                                {
                                    S0 -= size_arr[ry];
                                    Q0 -= size_arr[ry] * size_arr[ry];
                                    case1_sum -= size_arr[ry] * (size_arr[ry] - 1);
                                }

                                if (size_arr[rx] < size_arr[ry])
                                {
                                    int temp = rx;
                                    rx = ry;
                                    ry = temp;
                                }
                                parent[ry] = rx;
                                size_arr[rx] += size_arr[ry];
                                edges_arr[rx] += edges_arr[ry] + 1;

                                long free_new = size_arr[rx] - edges_arr[rx];
                                if (free_new == 0)
                                {
                                    S0 += size_arr[rx];
                                    Q0 += size_arr[rx] * size_arr[rx];
                                    case1_sum += size_arr[rx] * (size_arr[rx] - 1);
                                }
                            }
                            output.AppendLine("Yes");
                        }
                        else
                        {
                            output.AppendLine("No");
                        }
                    }
                }
                else
                {
                    long case2 = S0 * S0 - Q0;
                    long F = case1_sum + case2;
                    long total_pairs = (long)n * (n - 1);
                    long ans = total_pairs - F;
                    output.AppendLine(ans.ToString());
                }
            }

            Console.Write(output.ToString());
        }
    }
}
