using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver.Context
{
    public class CustomSolverBuilder
    {
        protected HashSet<Action<PuzzleContext>> _inits = new();
        protected HashSet<Action<PuzzleContext>> _solves = new();

        public CustomSolverBuilder AddSolveAction(params Action<PuzzleContext>[] action)
        {
            foreach (var item in action)
                _solves.Add(item);
            return this;
        }

        public CustomSolverBuilder AddInitAction(params Action<PuzzleContext>[] action)
        {
            foreach (var item in action)
                _inits.Add(item);
            return this;
        }

        public CustomSolverBuilder RemoveSolveAction(params Action<PuzzleContext>[] action)
        {
            foreach (var item in action)
                _solves.Remove(item);
            return this;
        }

        public CustomSolverBuilder RemoveInitAction(params Action<PuzzleContext>[] action)
        {
            foreach (var item in action)
                _inits.Remove(item);
            return this;
        }

        public ISolver Build() => new CustomSolver(_inits.ToArray(), _solves.ToArray());
    }
}
