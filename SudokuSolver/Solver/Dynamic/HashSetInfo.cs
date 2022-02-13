using Sudoku;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Dynamic
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
            var info = new HashSet<Cell>[context.Board.GetLength(0), context.Board.GetLength(1)];
            foreach (var c in info.AllCoordinates())
                info[c.x, c.y] = Enumerable.Range(1, 9).Cast<Cell>().ToHashSet();

            foreach (var c1 in board.AllCoordinates().Where(c => board[c.x, c.y] != Cell.Empty))
            {
                var cellValue = board[c1.x, c1.y];
                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                    info[c2.x, c2.y].Remove(cellValue);
                info[c1.x, c1.y] = new HashSet<Cell> { cellValue };
            }
            context.HashSetInfo = info;
        }

        public static void Mark(Cell[,] board, HashSet<Cell>[,] info, (int x, int y) c1, Cell value)
        {
            //mark it on board
            board[c1.x, c1.y] = value;
            //eliminate on info
            foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                info[c2.x, c2.y].Remove(board[c1.x, c1.y]);
            info[c1.x, c1.y] = new HashSet<Cell> { value };
        }
    }
}
