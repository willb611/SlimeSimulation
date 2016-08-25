namespace SlimeSimulation.LinearEquations
{
    public interface ILinearEquationSolver
    {
        double[] FindX(double[][] a, double[] b);
    }
}
