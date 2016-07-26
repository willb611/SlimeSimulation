using SlimeSimulation.Model;
using SlimeSimulation.View;
using SlimeSimulation.FlowCalculation;
using System.Linq;

namespace SlimeSimulation {
    class Program {
        static void Main(string[] args) {
            var slimeNetworkGenerator = new SlimeNetworkGenerator();
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.generate();
            using (MainView mainView = new MainView()) {

                var flowCalculator = new FlowCalculator();
                Node source = slimeNetwork.FoodSources.First();
                Node sink = slimeNetwork.FoodSources.Last();
                int flowAmount = 15;
                FlowResult flowResult = flowCalculator.CalculateFlow(slimeNetwork.Edges, slimeNetwork.Loops,
                    source, sink, flowAmount);
                mainView.renderFlowResult(flowResult);
            }
        }
    }
}
