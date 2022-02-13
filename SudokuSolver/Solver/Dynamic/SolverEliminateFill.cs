using System;

namespace SudokuSolver.Solver.Dynamic
{
    public class SolverEliminateFill : CustomSolver
    {
        public SolverEliminateFill() : base(
            new Action<dynamic>[]
            {
                HashSetInfo.Init
            },
            new Action<dynamic>[]
            {
                OneOptionLeft.Solve,
                GroupsFill.Solve
            })
        { }
    }
}
