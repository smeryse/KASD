using System;
using System.Collections.Generic;

class DominoTiling
{
    private int n, m;
    private char[][] board;
    private Dictionary<long, long> dp; // (col, mask) -> ways

    public DominoTiling(int n, int m, char[][] board)
    {
        this.n = n;
        this.m = m;
        this.board = board;
        this.dp = new Dictionary<long, long>();
    }

    public long Solve()
    {
        return Dfs(0, 0);
    }

    private long Dfs(int col, int mask)
    {
        // Все столбцы обработаны
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
        // Весь столбец заполнен -> идём дальше
        if (row == n)
            return Dfs(col + 1, nextMask);

        // Ячейка уже занята
        if ((curMask & (1 << row)) != 0)
            return Fill(col, row + 1, curMask, nextMask);

        // Ячейка заблокирована
        if (board[row][col] == 'X')
            return Fill(col, row + 1, curMask, nextMask);

        long res = 0;

        // Вертикальное домино
        if (row + 1 < n &&
            (curMask & (1 << (row + 1))) == 0 &&
            board[row + 1][col] == '.')
        {
            res += Fill(col, row + 2, curMask, nextMask);
        }

        // Горизонтальное домино
        if (col + 1 < m && board[row][col + 1] == '.')
        {
            res += Fill(col, row + 1, curMask, nextMask | (1 << row));
        }

        return res;
    }

    private long Encode(int col, int mask)
    {
        return ((long)col << 32) | (uint)mask;
    }
}

class Program
{
    static void Main()
    {
        string[] first = Console.ReadLine().Split();
        int n = int.Parse(first[0]);
        int m = int.Parse(first[1]);

        char[][] board = new char[n][];
        for (int i = 0; i < n; i++)
            board[i] = Console.ReadLine().ToCharArray();

        DominoTiling solver = new DominoTiling(n, m, board);
        Console.WriteLine(solver.Solve());
    }
}
