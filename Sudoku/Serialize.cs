using System;
using System.Linq;

namespace Sudoku
{
    public static class Serialize
    {
        public static Cell[,] FromBoardString(string puzzle)
        {
            puzzle = puzzle.Replace(" ", "").Replace("|", "").Replace("-" + Environment.NewLine, "").Replace("-", "");
            var grid = puzzle.Split(Environment.NewLine).Select(s => s.ToArray()).ToArray();
            var board = new Cell[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i, j] = grid[j][i] switch
                    {
                        '.' => Cell.Empty,
                        _ => (Cell)(grid[j][i] - '0')
                    };
            return board;
        }

        public static string ToBoardString(Cell[,] board) =>
            string.Join('\n', Enumerable.Range(0, board.GetLength(1)).Select(j => 
                (j % 3 == 0 && j > 0 ? "-------|-------|-------\n" : "")
                + string.Join("", Enumerable.Range(0, board.GetLength(0)).Select(i => 
                    (i % 3 == 0 && i > 0 ? " |" : "") 
                    + " " + board[i, j] switch { Cell.Empty => '.', _ => (char)(board[i, j] + '0') }))));

    }
}
