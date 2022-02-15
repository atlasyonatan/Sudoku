using Sudoku;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver.Context
{
    public abstract class SolverBase : ISolver
    {
        public IEnumerable<Cell[,]> Solve(Cell[,] puzzle)
        {
            var puzzles = new Stack<PuzzleContext>();
            puzzles.Push(new PuzzleContext()
            {
                Board = (Cell[,])puzzle.Clone(),
                Info = HashSetInfo.GetInfo(puzzle),
                Changed = false
            });
            while (puzzles.TryPop(out var context))
            {
                var board = context.Board;
                var info = context.Info;
                Initialize(context);
                InnerSolve(context);
                if (info.AllCoordinates().Any(c => info[c.x, c.y].Count == 0))
                    continue;
                if (board.IsSolved())
                    yield return board;
                else
                {
                    //guess
                    var ((x, y), guessOptions) = info.AllCoordinates()
                            .Where(c => board[c.x, c.y] == Cell.Empty)
                            .Select(c => (Coordinate: c, Info: info[c.x, c.y]))
                            .OrderBy(ci => ci.Info.Count)
                            .First();
                    foreach (var option in guessOptions)
                    {
                        var nContext = context.Clone();
                        nContext.Mark(x, y, option);
                        puzzles.Push(nContext);
                    }
                }
            }
            yield break;
        }

        protected abstract void Initialize(PuzzleContext context);

        protected abstract void InnerSolve(PuzzleContext context);
    }
}
