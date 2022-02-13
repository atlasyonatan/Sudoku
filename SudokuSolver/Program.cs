﻿using Sudoku;
using SudokuSolver.Solver.Dynamic;
using System;
using System.IO;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //link to generator website: https://qqwing.com/generate.html
            var filePath = @"D:\Repos\Sudoku\SudokuSolver\Puzzles\SudokuEasy1.txt";
            var puzzleString = File.ReadAllText(filePath);
            var puzzle = Serialize.FromBoardString(puzzleString);
            Console.WriteLine("Puzzle:");
            Console.WriteLine(Serialize.ToBoardString(puzzle));
            Console.WriteLine();
            var solutions = new SolverEliminateFill().Solve(puzzle);
            foreach (var solution in solutions)
            {
                Console.WriteLine(Serialize.ToBoardString(solution));
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
