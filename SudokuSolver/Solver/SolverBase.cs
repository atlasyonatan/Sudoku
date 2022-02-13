using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver
{
    public abstract class SolverBase<TInfo> : ISolver
    {
        protected Cell[,] _board;
        protected TInfo[,] _info;
        public IEnumerable<Cell[,]> Solve(Cell[,] puzzle)
        {
            _board = (Cell[,])puzzle.Clone();
            UpdateInfo();
            Solve();
            if(_board.AllCoordinates().All(c => _board[c.x,c.y] != Cell.Empty))
            {
                yield return _board;
                yield break;
            }
            throw new NotImplementedException("Solve yielded too little information or can't handle multiple solutions");
        }

        protected abstract void UpdateInfo();

        protected abstract void Solve();
    }
}
