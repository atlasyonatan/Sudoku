using Sudoku;
using System.Collections.Generic;

namespace SudokuSolver.Builder
{
    public class CellInfo
    {
        public HashSet<Cell> AvailibleValues;

        public static void Initialize(SolverContext<TInfo> context)
        {
            var board = context.Board;
            var ci = context.Info as CellInfo[,];

            ci = new CellInfo[context.Board.GetLength(0), context.Board.GetLength(1)];
            foreach (var c in ci.AllCoordinates())
                ci[c.x, c.y].AvailibleValues = Enumerable.Range(1, 9).Cast<Cell>().ToHashSet();

            foreach (var c1 in board.AllCoordinates().Where(c => board[c.x, c.y] != Cell.Empty))
            {
                var cellValue = board[c1.x, c1.y];
                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                    ci[c2.x, c2.y].AvailibleValues.Remove(cellValue);
                ci[c1.x, c1.y].AvailibleValues = new HashSet<Cell> { cellValue };
            }
        }
    }
}
