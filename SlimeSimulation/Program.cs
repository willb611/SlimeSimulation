using SlimeSimulation.Controller;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationStarter = new ApplicationStartController();
            applicationStarter.Render();
        }
    }
}
