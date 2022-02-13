using System;
using System.IO;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"D:\Repos\Sudoku\SudokuSolver\Puzzles\SudokuEasy1.txt";
            var puzzleString = File.ReadAllText(filePath);
            var puzzle = Serialize.FromBoardString(puzzleString);
            Console.WriteLine("Puzzle:");
            Console.WriteLine(Serialize.ToBoardString(puzzle));
            Console.WriteLine();
            var solutions = SolverHashSet.Solve(puzzle);
            foreach (var solution in solutions)
            {
                Console.WriteLine(Serialize.ToBoardString(solution));
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        
    }
}
