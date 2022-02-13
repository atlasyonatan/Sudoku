using Sudoku;
using System.Collections.Generic;

namespace SudokuSolver.Solver
{
    public interface ISolver
    {
        IEnumerable<Cell[,]> Solve(Cell[,] puzzle);
    }
}
