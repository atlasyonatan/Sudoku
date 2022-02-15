using Sudoku;
using System.Collections.Generic;

namespace SudokuSolver.Solver.Context
{
    public class PuzzleContext
    {
        public Cell[,] Board { get; set; }
        public HashSet<Cell>[,] Info { get; set; }
        public bool Changed { get; set; }
    }
}
