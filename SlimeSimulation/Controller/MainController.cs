using SlimeSimulation.FlowCalculation;
using SlimeSimulation.View;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.FlowCalculation.LinearEquations;
using System.Linq;
using SlimeSimulation.Controller;

namespace SlimeSimulation.View {
    class MainController {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly int flowAmount;
        private SlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator(13);
        private FlowCalculator flowCalculator = new FlowCalculator(new GaussianSolver());

        public MainController(int flowAmount) {
            this.flowAmount = flowAmount;
        }

        
        public void RunSimulation() {
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.Generate();
            using (MainView view = new MainView()) {
                logger.Info("[RunSimulation] Using before flowNetworkGraphController");
                new FlowNetworkGraphController(view, slimeNetwork.Edges).Render();
                //var initialFlow = GetFlow(slimeNetwork, flowAmount);
                //new FlowResultController(view).Render(initialFlow);
                //new ConductivityController(view).RenderConnectivity(slimeNetwork.Edges);

                // TODO figure out how to wait for the flow network controller to finish, then carry on with simulation
                // TODO IDisposable things aren't being used properly. Following log statement never entered ? 
                logger.Info("[RunSimulation] Using after flowNetworkGraphController");
            }
            logger.Info("[RunSimulation] Finished!");
        }


        private FlowResult GetFlow(SlimeNetwork network, int flow) {
            Node source = network.FoodSources.First();
            Node sink = network.FoodSources.Last();
            return GetFlow(network, flow, source, sink);
        }
        private FlowResult GetFlow(SlimeNetwork network, int flow, Node source, Node sink) {
            var flowResult = flowCalculator.CalculateFlow(network.Edges, network.Nodes,
                    source, sink, flowAmount);
            return flowResult;
        }
    }
}
