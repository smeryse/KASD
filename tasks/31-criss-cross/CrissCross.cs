using System;
using System.Collections.Generic;
using System.Linq;

namespace Task31
{
    public class WordPlacement
    {
        public string Word { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool Horizontal { get; set; }

        public int EndRow => Horizontal ? Row : Row + Word.Length - 1;
        public int EndCol => Horizontal ? Col + Word.Length - 1 : Col;
    }

    public class CrissCrossSolver
    {
        private const int GridSize = 30;
        private const char Empty = '\0';

        private readonly List<string> _words;
        private readonly char[,] _grid;
        private readonly List<WordPlacement> _placements;
        private bool _solved;

        public CrissCrossSolver(List<string> words)
        {
            _words = words;
            _grid = new char[GridSize, GridSize];
            _placements = new List<WordPlacement>();
            _solved = false;
        }

        public bool Solve()
        {
            var grouped = _words
                .GroupBy(w => w.Length)
                .OrderByDescending(g => g.Key)
                .Select(g => g.OrderBy(w => w).ToList())
                .ToList();

            var ordered = new List<string>();
            foreach (var group in grouped)
                ordered.AddRange(group);

            int center = GridSize / 2;

            var first = ordered[0];
            for (int c = 0; c < first.Length; c++)
                _grid[center, center + c] = first[c];

            _placements.Add(new WordPlacement
            {
                Word = first,
                Row = center,
                Col = center,
                Horizontal = true
            });

            var remaining = new bool[ordered.Count];
            remaining[0] = true;

            _solved = Backtrack(ordered, remaining, 1);
            return _solved;
        }

        private bool Backtrack(List<string> words, bool[] used, int placedCount)
        {
            if (placedCount == words.Count)
                return true;

            for (int i = 0; i < words.Count; i++)
            {
                if (used[i]) continue;

                string word = words[i];

                for (int p = 0; p < _placements.Count; p++)
                {
                    var existing = _placements[p];

                    for (int intersectIdx = 0; intersectIdx < word.Length; intersectIdx++)
                    {
                        char wordChar = word[intersectIdx];

                        for (int existIdx = 0; existIdx < existing.Word.Length; existIdx++)
                        {
                            if (existing.Word[existIdx] != wordChar)
                                continue;

                            int row, col;
                            bool horizontal;

                            if (existing.Horizontal)
                            {
                                row = existing.Row - intersectIdx;
                                col = existing.Col + existIdx;
                                horizontal = false;
                            }
                            else
                            {
                                row = existing.Row + existIdx;
                                col = existing.Col - intersectIdx;
                                horizontal = true;
                            }

                            if (CanPlace(word, row, col, horizontal))
                            {
                                PlaceWord(word, row, col, horizontal);
                                used[i] = true;

                                if (Backtrack(words, used, placedCount + 1))
                                    return true;

                                RemoveWord(word, row, col, horizontal);
                                used[i] = false;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool CanPlace(string word, int startRow, int startCol, bool horizontal)
        {
            int endRow = horizontal ? startRow : startRow + word.Length - 1;
            int endCol = horizontal ? startCol + word.Length - 1 : startCol;

            if (startRow < 0 || startCol < 0 || endRow >= GridSize || endCol >= GridSize)
                return false;

            bool hasIntersection = false;

            for (int i = 0; i < word.Length; i++)
            {
                int r = horizontal ? startRow : startRow + i;
                int c = horizontal ? startCol + i : startCol;

                char existing = _grid[r, c];

                if (existing != Empty)
                {
                    if (existing == word[i])
                        hasIntersection = true;
                    else
                        return false;
                }
            }

            return hasIntersection;
        }

        private void PlaceWord(string word, int startRow, int startCol, bool horizontal)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int r = horizontal ? startRow : startRow + i;
                int c = horizontal ? startCol + i : startCol;
                _grid[r, c] = word[i];
            }

            _placements.Add(new WordPlacement
            {
                Word = word,
                Row = startRow,
                Col = startCol,
                Horizontal = horizontal
            });
        }

        private void RemoveWord(string word, int startRow, int startCol, bool horizontal)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int r = horizontal ? startRow : startRow + i;
                int c = horizontal ? startCol + i : startCol;

                bool shared = false;
                foreach (var pl in _placements)
                {
                    if (pl.Word == word) continue;

                    if (pl.Horizontal)
                    {
                        if (pl.Row == r && c >= pl.Col && c <= pl.EndCol)
                        { shared = true; break; }
                    }
                    else
                    {
                        if (pl.Col == c && r >= pl.Row && r <= pl.EndRow)
                        { shared = true; break; }
                    }
                }

                if (!shared)
                    _grid[r, c] = Empty;
            }

            _placements.RemoveAll(p => p.Word == word
                && p.Row == startRow && p.Col == startCol
                && p.Horizontal == horizontal);
        }

        public void PrintGrid()
        {
            if (!_solved)
            {
                Console.WriteLine("No solution found.");
                return;
            }

            int minR = GridSize, maxR = 0, minC = GridSize, maxC = 0;
            for (int r = 0; r < GridSize; r++)
                for (int c = 0; c < GridSize; c++)
                    if (_grid[r, c] != Empty)
                    {
                        if (r < minR) minR = r;
                        if (r > maxR) maxR = r;
                        if (c < minC) minC = c;
                        if (c > maxC) maxC = c;
                    }

            for (int r = minR; r <= maxR; r++)
            {
                for (int c = minC; c <= maxC; c++)
                {
                    Console.Write(_grid[r, c] == Empty ? ' ' : _grid[r, c]);
                }
                Console.WriteLine();
            }
        }
    }
}
