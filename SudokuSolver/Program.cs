using Sudoku;
using SudokuSolver.Solver;
using SudokuSolver.Solver.Dynamic;
using System;
using System.Diagnostics;
using System.IO;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //link to generator website: https://qqwing.com/generate.html
            var filePath = @"D:\Repos\Sudoku\SudokuSolver\Puzzles\Sudoku1.txt";
            var puzzleString = File.ReadAllText(filePath);
            var puzzle = Serialize.FromBoardString(puzzleString);
            Console.WriteLine("Puzzle:");
            Console.WriteLine(Serialize.ToBoardString(puzzle));
            Console.WriteLine();
            var sw = new Stopwatch();
            sw.Start();
            ISolver solver = new CustomSolver(Array.Empty<Action<dynamic>>(), Array.Empty<Action<dynamic>>());
            var solutions = solver.Solve(puzzle);
            foreach (var solution in solutions)
            {
                sw.Stop();
                Console.WriteLine($"Solution: ({sw.Elapsed:c})");
                Console.WriteLine(Serialize.ToBoardString(solution));
                Console.WriteLine();
                sw.Restart();
            }
            sw.Stop();
            Console.ReadLine();
        }
    }
}
