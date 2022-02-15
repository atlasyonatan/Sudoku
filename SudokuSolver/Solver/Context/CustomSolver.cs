using System;

namespace SudokuSolver.Solver.Context
{
    public partial class CustomSolver : SolverBase
    {
        private readonly Action<PuzzleContext>[] _inits = Array.Empty<Action<PuzzleContext>>();
        private readonly Action<PuzzleContext>[] _solves = Array.Empty<Action<PuzzleContext>>();

        public CustomSolver()
        {
        }
        public CustomSolver(Action<PuzzleContext>[] initMethods, Action<PuzzleContext>[] solvingMethods)
        {
            _solves = solvingMethods;
            _inits = initMethods;
        }


        protected override void Initialize(PuzzleContext context)
        {
            foreach (var init in _inits)
                init(context);
        }

        protected override void InnerSolve(PuzzleContext context)
        {
            context.Changed = true;
            while (context.Changed)
            {
                context.Changed = false;
                foreach (var solve in _solves)
                    solve(context);
            }
        }
    }
}
