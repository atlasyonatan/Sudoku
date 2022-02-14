using Sudoku;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SudokuSolver.Solver.Dynamic
{
    public abstract class SolverBase : ISolver
    {
        public IEnumerable<Cell[,]> Solve(Cell[,] puzzle)
        {
            var puzzles = new Stack<Cell[,]>();
            puzzles.Push((Cell[,])puzzle.Clone());
            while (puzzles.TryPop(out var board))
            {
                dynamic context = new ExpandoObject();
                context.Board = board;
                Initialize(context);
                try
                {
                    InnerSolve(context);
                }
                catch
                {
                    
                }
                var info = context is IDictionary<string, object> dictionary
                        && dictionary.TryGetValue("HashSetInfo", out var obj)
                        && obj is HashSet<Cell>[,] infoObj
                        ? infoObj
                        : HashSetInfo.GetInfo(board);
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

        protected abstract void Initialize(dynamic context);

        protected abstract void InnerSolve(dynamic context);

        public static bool IsSolved(Cell[,] puzzle) =>
            puzzle.AllCoordinates().All(c => puzzle[c.x, c.y] != Cell.Empty);
    }
}
