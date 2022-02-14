using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Dynamic
{
    public static class GroupsFill
    {
        private static readonly Func<IEnumerable<IEnumerable<(int x, int y)>>>[] GroupsGetMethods = new Func<IEnumerable<IEnumerable<(int x, int y)>>>[] { AllRows, AllColumns, AllBoxes };
        
        //check columns, rows, boxes for musts
        public static void Solve(dynamic context)
        {
            var info = (HashSet<Cell>[,])context.HashSetInfo;
            var board = (Cell[,])context.Board;
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var groupsGetMethod in GroupsGetMethods)
                {
                    var groups = groupsGetMethod();
                    foreach (var cellGroup in groups.Select(g => g.ToArray()))
                    {
                        var knownValues = cellGroup.Select(c => board[c.x, c.y]).Where(cell => cell != Cell.Empty).ToHashSet();
                        var groupings = cellGroup.SelectMany(coordinate => info[coordinate.x, coordinate.y]
                                .Where(cell => !knownValues.Contains(cell))
                                .Select(cell => (Coordinate: coordinate, Value: cell)))
                            .GroupBy(ci => ci.Value)
                            .ToArray();
                        foreach (var grouping in groupings.Where(g => g.Count() == 1))
                        {
                            var (coordinate, value) = grouping.First();
                            HashSetInfo.Mark(board, info, coordinate, value);
                            changed = true;
                            context.Changed = true;
                        }
                    }
                }
            }
        }
    }
}
