using Sudoku;
using SudokuSolver.Builder;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Builder
{
    public static class SolverHelper
    {
        public static void InitializeCellInfo(SolverContext<HashSet<Cell>> context)
        {
            var board = context.Board;
            var info = context.Info;

            info = new HashSet<Cell>[context.Board.GetLength(0), context.Board.GetLength(1)];
            foreach (var c in info.AllCoordinates())
                info[c.x, c.y] = Enumerable.Range(1, 9).Cast<Cell>().ToHashSet();

            foreach (var c1 in board.AllCoordinates().Where(c => board[c.x, c.y] != Cell.Empty))
            {
                var cellValue = board[c1.x, c1.y];
                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                    info[c2.x, c2.y].AvailibleValues.Remove(cellValue);
                info[c1.x, c1.y].AvailibleValues = new HashSet<Cell> { cellValue };
            }
        }
    }
}
