using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public static class SolverHashSet
    {
        public static IEnumerable<Cell[,]> Solve(Cell[,] board)
        {
            board = (Cell[,])board.Clone();
            var width = board.GetLength(0);
            var length = board.GetLength(1);
            HashSet<Cell>[,] info = new HashSet<Cell>[width, length];
            foreach (var c in AllCoordinates(info))
                info[c.x, c.y] = Enumerable.Range(1, 9).Cast<Cell>().ToHashSet();

            foreach (var c1 in AllCoordinates(board).Where(c => board[c.x, c.y] != Cell.Empty))
            {
                foreach (var c2 in GetAllRelevant(c1.x, c1.y))
                    info[c2.x, c2.y].Remove(board[c1.x, c1.y]);
                info[c1.x, c1.y] = new HashSet<Cell> { board[c1.x, c1.y] };
            }

            var groupsGetMethods = new Func<IEnumerable<IEnumerable<(int x, int y)>>>[] { AllRows, AllColumns, AllBoxes };
            var changed = true;
            while (changed)
            {
                changed = false;
                var innerChanged = true;
                while (innerChanged)
                {
                    innerChanged = false;
                    foreach (var c1 in AllCoordinates(info).Where(c => board[c.x, c.y] == Cell.Empty))
                    {
                        var localInfo = info[c1.x, c1.y];
                        var count = localInfo.Count();
                        switch (count)
                        {
                            case 0:
                                yield break;
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
                foreach (var groupsGetMethod in groupsGetMethods)
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
            //if all known => finish.
            if (AllCoordinates(board).All(c => board[c.x, c.y] != Cell.Empty))
            {
                yield return board;
                yield break;
            }
            //else there are multiple solutions => find them
            throw new NotImplementedException("Solver can't handle multiple solutions yet");
        }

        public static IEnumerable<(int x, int y)> AllCoordinates<T>(T[,] arr) =>
            Enumerable.Range(0, arr.GetLength(0)).SelectMany(i => Enumerable.Range(0, arr.GetLength(1)).Select(j => (i, j)));

        public static IEnumerable<(int x, int y)> GetAllRelevant(int x, int y) => GetRow(x, y).Concat(GetColumn(x, y)).Concat(GetBox(x, y));

        public static IEnumerable<(int x, int y)> GetRow(int x, int y) =>
            Enumerable.Range(0, 9).Select(i => (i, y));

        public static IEnumerable<(int x, int y)> GetColumn(int x, int y) =>
            Enumerable.Range(0, 9).Select(i => (x, i));

        public static IEnumerable<(int x, int y)> GetBox(int x, int y)
        {
            var boxOriginX = x - x % 3;
            var boxOriginY = y - y % 3;
            return Enumerable.Range(boxOriginX, 3).SelectMany(i => Enumerable.Range(boxOriginY, 3).Select(j => (i, j)));
        }

        public static IEnumerable<IEnumerable<(int x, int y)>> AllRows() =>
            Enumerable.Range(0, 9).Select(y => GetRow(0, y));

        public static IEnumerable<IEnumerable<(int x, int y)>> AllColumns() =>
            Enumerable.Range(0, 9).Select(x => GetColumn(x, 0));

        public static IEnumerable<IEnumerable<(int x, int y)>> AllBoxes() =>
            Enumerable.Range(0, 3).SelectMany(y =>
                Enumerable.Range(0, 3).Select(x =>
                    GetBox(3 * x, 3 * y)));
    }
}
