using Sudoku;
using SudokuSolver.Solver;
using SudokuSolver.Solver.Context;
using SudokuSolver.Solver.Context.SolvingMethods;
using System;
using System.Diagnostics;
using System.IO;

namespace SudokuSolver
{
    class Program
    {
        static void Main()
        {
            ///link to generator website: https://qqwing.com/generate.html
            var filePath = @"D:\Repos\Sudoku\SudokuSolver\Puzzles\Sudoku1.txt";
            var puzzleString = File.ReadAllText(filePath);
            var puzzle = Serialize.FromBoardString(puzzleString);
            //var filePath = @"D:\Repos\Sudoku\SudokuSolver\Puzzles\Omer.txt";
            //var puzzleString = File.ReadAllText(filePath);
            //var puzzle = FromOmerString(puzzleString);

            Console.WriteLine("Puzzle:");
            Console.WriteLine(Serialize.ToBoardString(puzzle));
            Console.WriteLine();
            var sw = new Stopwatch();
            var solver = CreateSolver();
            sw.Start();
            var solutions = solver.Solve(puzzle);
            var noSolutions = true;
            foreach (var solution in solutions)
            {
                sw.Stop();
                noSolutions = false;
                Console.WriteLine($"Solution: ({sw.Elapsed:c})");
                Console.WriteLine(Serialize.ToBoardString(solution));
                Console.WriteLine();
                sw.Restart();
            }
            sw.Stop();
            if (noSolutions)
                Console.WriteLine($"No solutions. ({sw.Elapsed:c})");
            Console.ReadLine();
        }

        private static ISolver CreateSolver() => new CustomSolverBuilder()
                .AddSolveAction(OneOptionLeft.Solve)
                .AddSolveAction(GroupsFill.Solve)
                .Build();

        //private static Cell[,] FromOmerString(string s)
        //{
        //    var board = new Cell[9, 9];
        //    var jagged = s.Split('\n').Select(row => row.Select(c => c - '0').ToArray()).ToArray();
        //    foreach (var (x,y) in board.AllCoordinates())
        //        board[x,y] = jagged[y][x] == 0 ? Cell.Empty : (Cell)jagged[y][x];
        //    return board;
        //}
    }
}
