using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.LinearEquations
{
    public interface ILinearEquationSolver
    {
        double[] FindX(double[][] a, double[] b);
    }
}
