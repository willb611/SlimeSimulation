using NLog;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller {
    class FlowResultController : WindowController {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MainView view;
        private FlowResultWindow window;

        public FlowResultController(MainView view) {
            this.view = view;
        }

        internal void Render(FlowResult flowResult) {
            logger.Debug("Rendering FlowResult");
            using (window = new FlowResultWindow(flowResult, this)) {
                view.Display(window);
            }
        }

        public void OnClick(FlowResult result) {
            result.Validate();
        }

        public void OnQuit() {
            logger.Info("[OnQuit] Window closed!");
            if (window != null) {
                logger.Debug("[OnQuit] About to dispose of window");
                window.Dispose();
                logger.Debug("[OnQuit] Finished disposing of window");
            }
        }
    }
}
