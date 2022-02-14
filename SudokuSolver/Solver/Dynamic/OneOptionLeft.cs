using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver.Dynamic
{
    public static class OneOptionLeft
    {
        public static void Solve(dynamic context)
        {
            var board = (Cell[,])context.Board;
            var info = (HashSet<Cell>[,])context.HashSetInfo;
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var c1 in info.AllCoordinates().Where(c => board[c.x, c.y] == Cell.Empty))
                {
                    var localInfo = info[c1.x, c1.y];
                    var count = localInfo.Count;
                    switch (count)
                    {
                        case 0:
                            throw new Exception($"Cell can't have 0 avalible values (x={c1.x},y={c1.y})");
                        case 1:
                            HashSetInfo.Mark(board, info, c1, localInfo.First());
                            changed = true;
                            context.Changed = true;
                            break;
                    }
                }
            }
        }
    }
}
