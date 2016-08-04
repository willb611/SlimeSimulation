using SlimeSimulation.FlowCalculation;
using SlimeSimulation.View;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.FlowCalculation.LinearEquations;
using System.Linq;

namespace SlimeSimulation.Controller {
    class MainController {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        LatticeSlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator();
        
        public void RunSimulation() {
            SlimeNetwork slimeNetwork = slimeNetworkGenerator.Generate(3);
            var flowCalculator = new FlowCalculator();
            Node source = slimeNetwork.FoodSources.First();
            Node sink = slimeNetwork.FoodSources.Last();
            int flowAmount = 4;
            var flowResult = flowCalculator.CalculateFlow(slimeNetwork.Edges, slimeNetwork.Nodes,
                    source, sink, flowAmount);
            RenderFlowResult(flowResult);
        }

        internal void RenderFlowResult(FlowResult flowResult) {
            logger.Debug("Rendering FlowResult");
            var flowWindow = new FlowResultWindow(flowResult, this);
            Display(flowWindow);
        }

        public void OnClick(FlowResult result) {
            result.ValidateFlowResult();
        }

        internal void RenderConnectivity(ISet<Edge> edges) {
            var window = new ConductivityWindow(new List<Edge>(edges));
            Display(window);
        }

        private void Display(WindowTemplate windowTemplate) {
            using (MainView view = new MainView()) {
                view.Display(windowTemplate);
            }
        }
    }
}
