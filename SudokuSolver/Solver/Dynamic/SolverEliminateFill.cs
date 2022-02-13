using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver.Dynamic
{
    public class SolverEliminateFill : SolverBase
    {
        private static readonly Func<IEnumerable<IEnumerable<(int x, int y)>>>[] GroupsGetMethods = new Func<IEnumerable<IEnumerable<(int x, int y)>>>[] { AllRows, AllColumns, AllBoxes };

        protected override void Initialize(dynamic context)
        {
            HashSetInfo.Init(context);
        }

        protected override void TrySolve(dynamic context)
        {
            var info = (HashSet<Cell>[,])context.HashSetInfo;
            var board = (Cell[,])context.Board;
            var changed = true;
            while (changed)
            {
                changed = false;
                var innerChanged = true;
                while (innerChanged)
                {
                    innerChanged = false;
                    foreach (var c1 in info.AllCoordinates().Where(c => board[c.x, c.y] == Cell.Empty))
                    {
                        var localInfo = info[c1.x, c1.y];
                        var count = localInfo.Count();
                        switch (count)
                        {
                            case 0:
                                throw new Exception($"Cell can't have 0 avalible values (x={c1.x},y={c1.y})");
                            case 1:
                                //mark it on board
                                board[c1.x, c1.y] = localInfo.First();
                                //eliminate on info
                                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                                    info[c2.x, c2.y].Remove(board[c1.x, c1.y]);
                                innerChanged = true;
                                break;
                        }
                    }
                }

                //check columns, rows, boxes for musts
                foreach (var groupsGetMethod in GroupsGetMethods)
                {
                    var groups = groupsGetMethod();
                    foreach (var cellGroup in groups.Select(g => g.ToArray()))
                    {
                        var knownValues = cellGroup.Select(c => board[c.x, c.y]).Where(cell => cell != Cell.Empty).ToHashSet();
                        var groupings = cellGroup.SelectMany(coordinate => info[coordinate.x, coordinate.y]
                                .Where(cell => !knownValues.Contains(cell))
                                .Select(cell => (coordiante: coordinate, cell: cell)))
                            .GroupBy(ci => ci.cell)
                            .ToArray();
                        foreach (var grouping in groupings.Where(g => g.Count() == 1))
                        {
                            var cellInfo = grouping.First();
                            info[cellInfo.coordiante.x, cellInfo.coordiante.y] = new HashSet<Cell> { cellInfo.cell };
                            changed = true;
                        }
                    }
                }
            }
        }
    }
}
