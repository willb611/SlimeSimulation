using NLog;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller {
    class FlowResultController {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MainView view;

        public FlowResultController(MainView view) {
            this.view = view;
        }

        internal void RenderFlowResult(FlowResult flowResult) {
            logger.Debug("Rendering FlowResult");
            var flowWindow = new FlowResultWindow(flowResult, this);
            view.Display(flowWindow);
        }

        public void OnClick(FlowResult result) {
            result.Validate();
        }
    }
}
