using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using static SudokuSolver.Array2D;

namespace SudokuSolver.Solver
{
    public class SolverEliminateFill : SolverBase<HashSet<Cell>>
    {
        private readonly Func<IEnumerable<IEnumerable<(int x, int y)>>>[] GroupsGetMethods = new Func<IEnumerable<IEnumerable<(int x, int y)>>>[] { AllRows, AllColumns, AllBoxes };
        protected override void UpdateInfo()
        {
            _info = new HashSet<Cell>[_board.GetLength(0), _board.GetLength(1)];
            foreach (var c in _info.AllCoordinates())
                _info[c.x, c.y] = Enumerable.Range(1, 9).Cast<Cell>().ToHashSet();

            foreach (var c1 in _board.AllCoordinates().Where(c => _board[c.x, c.y] != Cell.Empty))
            {
                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                    _info[c2.x, c2.y].Remove(_board[c1.x, c1.y]);
                _info[c1.x, c1.y] = new HashSet<Cell> { _board[c1.x, c1.y] };
            }
        }

        protected override void Solve()
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                var innerChanged = true;
                while (innerChanged)
                {
                    innerChanged = false;
                    foreach (var c1 in _info.AllCoordinates().Where(c => _board[c.x, c.y] == Cell.Empty))
                    {
                        var localInfo = _info[c1.x, c1.y];
                        var count = localInfo.Count();
                        switch (count)
                        {
                            case 0:
                                throw new Exception($"Cell can't have no avalible values (x={c1.x},y={c1.y})");
                            case 1:
                                //mark it on board
                                _board[c1.x, c1.y] = localInfo.First();
                                //eliminate on info
                                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                                    _info[c2.x, c2.y].Remove(_board[c1.x, c1.y]);
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
                        var knownValues = cellGroup.Select(c => _board[c.x, c.y]).Where(cell => cell != Cell.Empty).ToHashSet();
                        var groupings = cellGroup.SelectMany(coordinate => _info[coordinate.x, coordinate.y]
                                .Where(cell => !knownValues.Contains(cell))
                                .Select(cell => (coordiante: coordinate, cell: cell)))
                            .GroupBy(ci => ci.cell)
                            .ToArray();
                        foreach (var grouping in groupings.Where(g => g.Count() == 1))
                        {
                            var cellInfo = grouping.First();
                            _info[cellInfo.coordiante.x, cellInfo.coordiante.y] = new HashSet<Cell> { cellInfo.cell };
                            changed = true;
                        }
                    }
                }
            }
        }
    }
}
