using SlimeSimulation.Model;
using SlimeSimulation.View;
using SlimeSimulation.FlowCalculation.LinearEquations;
using SlimeSimulation.FlowCalculation;
using System.Linq;

namespace SlimeSimulation {
    class Program {
        static void Main(string[] args) {
            var slimeNetworkGenerator = new LatticeSlimeNetworkGenerator();
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.generate(5);
            using (MainView mainView = new MainView()) {

                var flowCalculator = new FlowCalculator();
                Node source = slimeNetwork.FoodSources.First();
                Node sink = slimeNetwork.FoodSources.Last();
                int flowAmount = 4;
                var flowResult = flowCalculator.CalculateFlow(slimeNetwork.Edges, slimeNetwork.Nodes,
                    source, sink, flowAmount);
                mainView.renderFlowResult(flowResult);
                //mainView.renderConnectivity(slimeNetwork.Edges);
            }
        }
    }
}
