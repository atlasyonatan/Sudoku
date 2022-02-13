using Sudoku;
using System.Dynamic;

namespace SudokuSolver.Solver
{
    public class SolverContext<TInfo> : ExpandoObject
    {
        public Cell[,] Board;
        public TInfo[,] Info;
    }

    public class SolverContext
}
