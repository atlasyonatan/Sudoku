using Sudoku;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Context
{
    public static class HashSetInfo
    {
        /// <summary>
        /// Initializes a new <see cref="HashSet{Cell}"/> on context as 'HashSetInfo'
        /// </summary>
        /// <param name="context"></param>
        public static void Init(dynamic context)
        {
            var board = (Cell[,])context.Board;
            context.HashSetInfo = GetInfo(board);
        }

        public static void Mark(Cell[,] board, HashSet<Cell>[,] info, (int x, int y) c1, Cell value)
        {
            //mark it on board
            board[c1.x, c1.y] = value;
            //eliminate on info
            foreach (var (x, y) in GetAllRelevant(c1.x, c1.y))
                info[x, y].Remove(board[c1.x, c1.y]);
            info[c1.x, c1.y] = new HashSet<Cell> { value };
        }

        public static HashSet<Cell>[,] GetInfo(Cell[,] board)
        {
            var info = new HashSet<Cell>[board.GetLength(0), board.GetLength(1)];
            foreach (var (x, y) in board.AllCoordinates())
                info[x, y] = board[x, y] == Cell.Empty
                    ? Enumerable.Range(1, 9).Cast<Cell>().ToHashSet()
                    : new HashSet<Cell> { board[x, y] };
            foreach (var c in board.AllCoordinates().Where(c => board[c.x, c.y] != Cell.Empty))
                foreach (var (x, y) in GetAllRelevant(c.x, c.y).Where(r => c != r))
                    info[x, y].Remove(board[c.x, c.y]);
            return info;
        }

        public static HashSet<Cell> GetInfo(Cell[,] board, int x, int y)
        {
            switch (board[x, y])
            {
                case Cell.Empty:
                    {
                        var info = Enumerable.Range(1, 9).Cast<Cell>().ToHashSet();
                        foreach (var value in GetAllRelevant(x, y).Select(c => board[c.x, c.y]).Where(value => value != Cell.Empty))
                            info.Remove(value);
                        return info;
                    }
                default:
                    return new HashSet<Cell> { board[x, y] };
            }
        }
    }
}
