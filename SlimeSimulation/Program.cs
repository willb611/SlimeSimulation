using SlimeSimulation.Controller;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;

namespace SlimeSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            //var flowCalculator = new FlowCalculator(new LupDecompositionSolver());
            //SlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator(13, 0.05);
            //var initial = slimeNetworkGenerator.Generate();
            //SimulationUpdater simulationUpdater = new SimulationUpdater(flowCalculator, new SlimeNetworkAdaptionCalculator(feedbackParameter));
            //var controller = new SimulationController(flowAmount, simulationUpdater, initial);
            //controller.RunSimulation();
            var applicationStarter = new ApplicationStartController();
            applicationStarter.Render();
        }
    }
}
