using Sudoku;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver.Context
{
    public abstract class SolverBase : ISolver
    {
        public IEnumerable<Cell[,]> Solve(Cell[,] puzzle)
        {
            var puzzles = new Stack<Cell[,]>();
            puzzles.Push((Cell[,])puzzle.Clone());
            while (puzzles.TryPop(out var board))
            {
                var info = HashSetInfo.GetInfo(board);
                var context = new PuzzleContext()
                {
                    Board = board,
                    Info = info,
                    Changed = false
                };
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
                        var newBoard = (Cell[,])board.Clone();
                        newBoard[x, y] = option;
                        puzzles.Push(newBoard);
                    }
                }
            }
            yield break;
        }

        protected abstract void Initialize(PuzzleContext context);

        protected abstract void InnerSolve(PuzzleContext context);
    }
}
