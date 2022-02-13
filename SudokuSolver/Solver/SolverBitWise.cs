using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver
{
    public static class SolverBitWise
    {
        public static IEnumerable<Cell[,]> Solve(Cell[,] board)
        {
            board = (Cell[,])board.Clone();
            int[,] optionsArr = new int[9, 9];

            //0b0 = available
            //0b1 = eliminated
            void HandleKnownDigit(int x, int y)
            {
                var digit = (int)board[x, y];
                var mask = 1 << digit;
                foreach (var (a, b) in GetBox(x, y).Concat(GetColumn(x, y)).Concat(GetRow(x, y)))
                    optionsArr[a, b] |= mask;
                optionsArr[x, y] = ~mask;
            }

            //handle known digits from initial board
            foreach (var p in AllCoordinates(board).Where(p => board[p.x, p.y] != Cell.Empty))
                HandleKnownDigit(p.x, p.y);

            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var p in AllCoordinates(board).Where(p => board[p.x, p.y] == Cell.Empty))
                {
                    var options = DigitOptions(optionsArr[p.x, p.y]).ToArray();
                    if(options.Length == 0)
                        yield break; //found conflict. no solutions
                    if (options.Length > 1)
                        continue;
                    var option = options[0];
                    board[p.x, p.y] = (Cell)option;
                    HandleKnownDigit(p.x, p.y);
                    changed = true;
                } 
            }

            var leftovers = AllCoordinates(board).Where(p => board[p.x, p.y] != Cell.Empty).ToArray();
            if (leftovers.Length == 0)
            {
                yield return board;
                yield break;
            }

            //todo: handle degrees of freedom
            throw new NotImplementedException("todo: handle degrees of freedom");
        }

        public static IEnumerable<(int x, int y)> AllCoordinates<T>(T[,] arr) =>
            Enumerable.Range(0, arr.GetLength(0)).SelectMany(i => Enumerable.Range(0, arr.GetLength(1)).Select(j => (i, j)));


        public static readonly int Mask = ((int)Math.Pow(2, 9)-1) << 1;

        public static IEnumerable<int> DigitOptions(int options) =>
            Enumerable.Range(1, 9).Where(i => (~options & Mask) == (1 << i)); //(~(options | ~Mask) ^ (1 << i)) == 0);

        public static IEnumerable<(int x, int y)> GetRow(int x, int y)
        {
            for (int i = 0; i < 9; i++)
                yield return (x, i);
        }

        public static IEnumerable<(int x, int y)> GetColumn(int x, int y)
        {
            for (int i = 0; i < 9; i++)
                yield return (i, y);
        }

        public static IEnumerable<(int x, int y)> GetBox(int x, int y)
        {
            var boxOriginX = x - x % 3;
            var boxOriginY = y - y % 3;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    yield return (boxOriginX + i, boxOriginY + j);
        }
    }
}
