using SlimeSimulation.FlowCalculation;
using SlimeSimulation.View;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using NLog;

namespace SlimeSimulation.Controller {
    class MainController {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainController() {
        }

        internal void RenderFlowResult(FlowResult flowResult) {
            logger.Debug("Rendering FlowResult");
            var flowWindow = new FlowResultWindow(flowResult);
            Display(flowWindow);
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
