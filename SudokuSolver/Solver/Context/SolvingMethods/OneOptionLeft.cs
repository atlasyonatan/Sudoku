using Sudoku;
using System;
using System.Linq;

namespace SudokuSolver.Solver.Context.SolvingMethods
{
    public static class OneOptionLeft
    {
        public static void Solve(PuzzleContext context)
        {
            var board = context.Board;
            var info = context.Info;
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var (x, y) in info.AllCoordinates().Where(c => board[c.x, c.y] == Cell.Empty))
                {
                    var localInfo = info[x, y];
                    if (localInfo.Count == 1)
                    {
                        HashSetInfo.Mark(board, info, (x, y), localInfo.First());
                        changed = true;
                        context.Changed = true;
                        break;
                    }
                }
            }
        }
    }
}
