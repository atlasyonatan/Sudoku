using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Solver.Dynamic
{
    public class CustomSolverBuilder
    {
        protected HashSet<Action<dynamic>> _inits = new HashSet<Action<dynamic>>();
        protected HashSet<Action<dynamic>> _solves = new HashSet<Action<dynamic>>();

        public CustomSolverBuilder AddSolveAction(params Action<dynamic>[] action)
        {
            foreach (var item in action)
                _solves.Add(item);
            return this;
        }

        public CustomSolverBuilder AddInitAction(params Action<dynamic>[] action)
        {
            foreach (var item in action)
                _inits.Add(item);
            return this;
        }

        public CustomSolverBuilder RemoveSolveAction(params Action<dynamic>[] action)
        {
            foreach (var item in action)
                _solves.Remove(item);
            return this;
        }

        public CustomSolverBuilder RemoveInitAction(params Action<dynamic>[] action)
        {
            foreach (var item in action)
                _inits.Remove(item);
            return this;
        }

        public ISolver Build() => new CustomSolver(_inits.ToArray(), _solves.ToArray());
    }
}
