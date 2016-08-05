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
        LatticeSlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator();
        
        public void RunSimulation() {
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.Generate(13);
            var flowCalculator = new FlowCalculator(new GaussianSolver());
            Node source = slimeNetwork.FoodSources.First();
            Node sink = slimeNetwork.FoodSources.Last();
            int flowAmount = 4;
            var flowResult = flowCalculator.CalculateFlow(slimeNetwork.Edges, slimeNetwork.Nodes,
                    source, sink, flowAmount);
            using (MainView view = new MainView()) {
                new FlowResultController(view).RenderFlowResult(flowResult);
                //new ConductivityController(view).RenderConnectivity(slimeNetwork.Edges);
            }
        }
    }
}
