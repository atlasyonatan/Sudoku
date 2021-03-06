using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Context.SolvingMethods
{
    public static class GroupsFill
    {
        private static readonly Func<IEnumerable<IEnumerable<(int x, int y)>>>[] GroupsGetMethods = new Func<IEnumerable<IEnumerable<(int x, int y)>>>[] { AllRows, AllColumns, AllBoxes };

        //check columns, rows, boxes for musts
        public static void Solve(PuzzleContext context)
        {
            var board = context.Board;
            var info = context.Info;
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
                            var ((x,y), value) = grouping.First();
                            context.Mark(x, y, value);
                            changed = true;
                        }
                    }
                }
            }
        }
    }
}
