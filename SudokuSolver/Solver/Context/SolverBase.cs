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
                var context = new PuzzleContext()
                {
                    Board = board,
                    Info = HashSetInfo.GetInfo(board),
                    Changed = false
                };
                //dynamic context = new ExpandoObject();
                //context.Board = board;
                Initialize(context);
                try
                {
                    InnerSolve(context);
                }
                catch
                {

                }
                var info = context.Info;
                //var info = context is IDictionary<string, object> dictionary
                //        && dictionary.TryGetValue("HashSetInfo", out var obj)
                //        && obj is HashSet<Cell>[,] infoObj
                //        ? infoObj
                //        : HashSetInfo.GetInfo(board);
                if (info.AllCoordinates().Any(c => info[c.x, c.y].Count == 0))
                    continue;
                if (IsSolved(board))
                    yield return board;
                else
                {
                    //guess
                    var ((x, y), options, _) = info.AllCoordinates()
                            .Where(c => board[c.x, c.y] == Cell.Empty)
                            .Select(c => (Coordinate: c, Info: info[c.x, c.y], info[c.x, c.y].Count))
                            .OrderBy(ci => ci.Count)
                            .First();
                    foreach (var option in options)
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

        public static bool IsSolved(Cell[,] puzzle) =>
            puzzle.AllCoordinates().All(c => puzzle[c.x, c.y] != Cell.Empty);
    }
}
