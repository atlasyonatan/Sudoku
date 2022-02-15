using Sudoku;
using System.Linq;

namespace SudokuSolver.Solver
{
    public static class Puzzle
    {
        public static bool IsSolved(this Cell[,] puzzle) =>
            puzzle.AllCoordinates().All(c => puzzle[c.x, c.y] != Cell.Empty);
    }
}
