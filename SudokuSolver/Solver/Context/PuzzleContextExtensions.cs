using Sudoku;
using System.Collections.Generic;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Context
{
    public static class PuzzleContextExtensions
    {
        public static PuzzleContext Clone(this PuzzleContext context)
        {
            var board = (Cell[,])context.Board.Clone();
            var info = (HashSet<Cell>[,])context.Info.Clone();
            foreach (var (x, y) in info.AllCoordinates())
                info[x, y] = new HashSet<Cell>(info[x, y]);
            return new PuzzleContext()
            {
                Board = board,
                Info = info,
                Changed = context.Changed,
            };
        }

        public static void Mark(this PuzzleContext context, int x, int y, Cell value)
        {
            //mark it on board
            context.Board[x, y] = value;
            //eliminate on info
            foreach (var (i, j) in GetAllRelevant(x, y))
                context.Info[i, j].Remove(context.Board[x,y]);
            context.Info[x, y] = new HashSet<Cell> { value };
            context.Changed = true;
        }
    }
}
