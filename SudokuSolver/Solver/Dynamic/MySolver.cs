using System;

namespace SudokuSolver.Solver.Dynamic
{
    public class MySolver : CustomSolver
    {
        public MySolver() : base(
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
