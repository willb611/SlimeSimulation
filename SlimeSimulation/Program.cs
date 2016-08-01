using SlimeSimulation.Model;
using SlimeSimulation.FlowCalculation.LinearEquations;
using System.Linq;
using SlimeSimulation.Controller;

namespace SlimeSimulation {
    class Program {
        static void Main(string[] args) {
            var slimeNetworkGenerator = new LatticeSlimeNetworkGenerator();
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.generate(5);

            var controller = new MainController();
            var flowCalculator = new FlowCalculator();
            Node source = slimeNetwork.FoodSources.First();
            Node sink = slimeNetwork.FoodSources.Last();
            int flowAmount = 4;
            var flowResult = flowCalculator.CalculateFlow(slimeNetwork.Edges, slimeNetwork.Nodes,
                    source, sink, flowAmount);
            controller.RenderFlowResult(flowResult);
        }
    }
}
