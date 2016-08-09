using SlimeSimulation.Controller;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.FlowCalculation.LinearEquations;

namespace SlimeSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var flowAmount = 3;
            var feedbackParameter = 1.8;
            var flowCalculator = new FlowCalculator(new LupDecompositionSolver());
            SimulationUpdater simulationUpdater = new SimulationUpdater(flowCalculator, new SlimeNetworkAdaptionCalculator(feedbackParameter));
            var controller = new MainController(flowAmount, simulationUpdater);
            controller.RunSimulation();
        }
    }
}
