using System;

namespace SudokuSolver.Solver.Dynamic
{
    public partial class CustomSolver : SolverBase
    {
        private readonly Action<dynamic>[] _inits;
        private readonly Action<dynamic>[] _solves;
        public CustomSolver(Action<dynamic>[] initMethods, Action<dynamic>[] solvingMethods)
        {
            _solves = solvingMethods;
            _inits = initMethods;
        }


        protected override void Initialize(dynamic context)
        {
            foreach (var init in _inits)
                init(context);
        }

        protected override void TrySolve(dynamic context)
        {
            foreach (var solve in _solves)
                solve(context);
        }
    }
}
