using Sudoku;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SudokuSolver.Solver.Dynamic
{
    public abstract class SolverBase : ISolver
    {
        public IEnumerable<Cell[,]> Solve(Cell[,] puzzle)
        {
            dynamic context = new ExpandoObject();
            puzzle = (Cell[,])puzzle.Clone();
            context.Board = puzzle;
            Initialize(context);
            TrySolve(context);
            if(puzzle.AllCoordinates().All(c => puzzle[c.x,c.y] != Cell.Empty))
            {
                yield return (Cell[,])context.Board.Clone();
                yield break;
            }
            throw new NotImplementedException("Solve yielded too little information or can't handle multiple solutions");
        }

        protected abstract void Initialize(dynamic context);

        protected abstract void TrySolve(dynamic context);
    }
}
