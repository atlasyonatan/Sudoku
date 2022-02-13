using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver
{
    public abstract class SolverBase<TInfo> : ISolver
    {
        public IEnumerable<Cell[,]> Solve(Cell[,] puzzle)
        {
            var context = new SolverContext<TInfo>();
            context.Board = (Cell[,])puzzle.Clone();
            Initialize(context);
            Solve(context);
            if(context.Board.AllCoordinates().All(c => context.Board[c.x,c.y] != Cell.Empty))
            {
                yield return (Cell[,])context.Board.Clone();
                yield break;
            }
            throw new NotImplementedException("Solve yielded too little information or can't handle multiple solutions");
        }

        protected abstract void Initialize(SolverContext<TInfo> context);

        protected abstract void Solve(SolverContext<TInfo> context);
    }
}
