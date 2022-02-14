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
                    TrySolve(context);
                }
                catch
                {
                    continue;
                }
                if (IsSolved(board))
                    yield return board;
                else
                {
                    //guess
                    var info = context is IDictionary<string, object> dictionary 
                        && dictionary.TryGetValue("HashSetInfo", out var obj) 
                        && obj is HashSet<Cell>[,] infoObj
                        ? infoObj
                        : HashSetInfo.GetInfo(board);
                    var guessInfo = info.AllCoordinates()
                            .Select(c => (Coordinate: c, Info: info[c.x, c.y], Count: info[c.x, c.y].Count()))
                            .Where(ci => ci.Count > 1)
                            .OrderBy(ci => ci.Count)
                            .First();
                    var c = guessInfo.Coordinate;
                    foreach (var option in guessInfo.Info)
                    {
                        var newBoard = (Cell[,])board.Clone();
                        newBoard[c.x, c.y] = option;
                        puzzles.Push(newBoard);
                    }
                }
            }
            yield break;
        }

        protected abstract void Initialize(dynamic context);

        protected abstract void TrySolve(dynamic context);

        public static bool IsSolved(Cell[,] puzzle) =>
            puzzle.AllCoordinates().All(c => puzzle[c.x, c.y] != Cell.Empty);
    }
}
