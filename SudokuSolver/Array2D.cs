using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public static class Array2D
    {
        public static IEnumerable<(int x, int y)> AllCoordinates<T>(this T[,] arr) =>
            Enumerable.Range(0, arr.GetLength(0)).SelectMany(i => Enumerable.Range(0, arr.GetLength(1)).Select(j => (i, j)));

        public static IEnumerable<(int x, int y)> GetAllRelevant(int x, int y) => GetRow(y).Concat(GetColumn(x)).Concat(GetBox(x, y));

        public static IEnumerable<(int x, int y)> GetRow(int y) =>
            Enumerable.Range(0, 9).Select(i => (i, y));

        public static IEnumerable<(int x, int y)> GetColumn(int x) =>
            Enumerable.Range(0, 9).Select(i => (x, i));

        public static IEnumerable<(int x, int y)> GetBox(int x, int y)
        {
            var boxOriginX = x - x % 3;
            var boxOriginY = y - y % 3;
            return Enumerable.Range(boxOriginX, 3).SelectMany(i => Enumerable.Range(boxOriginY, 3).Select(j => (i, j)));
        }

        public static IEnumerable<IEnumerable<(int x, int y)>> AllRows() =>
            Enumerable.Range(0, 9).Select(y => GetRow(y));

        public static IEnumerable<IEnumerable<(int x, int y)>> AllColumns() =>
            Enumerable.Range(0, 9).Select(x => GetColumn(x));

        public static IEnumerable<IEnumerable<(int x, int y)>> AllBoxes() =>
            Enumerable.Range(0, 3).SelectMany(y =>
                Enumerable.Range(0, 3).Select(x =>
                    GetBox(3 * x, 3 * y)));
    }
}
