using SlimeSimulation.Model;
using SlimeSimulation.View;
using SlimeSimulation.FlowCalculation;
using System.Linq;

namespace SlimeSimulation {
    class Program {
        static void Main(string[] args) {
            var slimeNetworkGenerator = new LatticeSlimeNetworkGenerator();
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.generate();
            // Given edge with node, not foodNode. So in slimeNetworkGenerator an edge is not being updated with the new foodsource
            using (MainView mainView = new MainView()) {

                var flowCalculator = new FlowCalculator();
                Node source = slimeNetwork.FoodSources.First();
                Node sink = slimeNetwork.FoodSources.Last();
                int flowAmount = 4;
                FlowResult flowResult = flowCalculator.CalculateFlow(slimeNetwork.Edges, slimeNetwork.Loops,
                    source, sink, flowAmount);
                mainView.renderFlowResult(flowResult);
            }
        }
    }
}
