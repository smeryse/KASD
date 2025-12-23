using System;
using System.Collections.Generic;

namespace CT3.Tasks;

internal class DominoTiling
{
    private readonly int n;
    private readonly int m;
    private readonly char[][] board;
    private readonly Dictionary<long, long> dp;

    public DominoTiling(int n, int m, char[][] board)
    {
        this.n = n;
        this.m = m;
        this.board = board;
        dp = new Dictionary<long, long>();
    }

    public long Solve()
    {
        return Dfs(0, 0);
    }

    private long Dfs(int col, int mask)
    {
        if (col == m)
            return mask == 0 ? 1 : 0;

        long key = Encode(col, mask);
        if (dp.ContainsKey(key))
            return dp[key];

        long res = Fill(col, 0, mask, 0);
        dp[key] = res;
        return res;
    }

    private long Fill(int col, int row, int curMask, int nextMask)
    {
        if (row == n)
            return Dfs(col + 1, nextMask);

        if ((curMask & (1 << row)) != 0)
            return Fill(col, row + 1, curMask, nextMask);

        if (board[row][col] == 'X')
            return Fill(col, row + 1, curMask, nextMask);

        long res = 0;

        if (row + 1 < n &&
            (curMask & (1 << (row + 1))) == 0 &&
            board[row + 1][col] == '.')
        {
            res += Fill(col, row + 2, curMask, nextMask);
        }

        if (col + 1 < m && board[row][col + 1] == '.')
        {
            res += Fill(col, row + 1, curMask, nextMask | (1 << row));
        }

        return res;
    }

    private static long Encode(int col, int mask)
    {
        return ((long)col << 32) | (uint)mask;
    }
}

internal static class ReplaceDomino
{
    public static void Solve()
    {
        string[] first = Console.ReadLine().Split();
        int n = int.Parse(first[0]);
        int m = int.Parse(first[1]);

        char[][] board = new char[n][];
        for (int i = 0; i < n; i++)
            board[i] = Console.ReadLine().ToCharArray();

        var solver = new DominoTiling(n, m, board);
        Console.WriteLine(solver.Solve());
    }
}
