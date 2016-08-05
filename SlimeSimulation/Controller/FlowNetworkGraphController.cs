using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller {
    class FlowNetworkGraphController : WindowController {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MainView view;
        private FlowNetworkGraphWindow window;

        public FlowNetworkGraphController(MainView view) {
            this.view = view;
        }

        public void OnQuit() {
            logger.Info("[OnQuit] Window closed!");
            if (window != null) {
                logger.Debug("[OnQuit] About to dispose of window");
                window.Dispose();
                logger.Debug("[OnQuit] Finished disposing of window");
            }
        }

        public void RenderConnectivity(ISet<Edge> edges) {
            logger.Info("[RenderConnectivity] Making new window");
            using (window = new FlowNetworkGraphWindow(new List<Edge>(edges), this)) {
                logger.Trace("[RenderConnectivity] About to display");
                view.Display(window);
                logger.Trace("[RenderConnectivity] Finished view.Display");
            }
            logger.Trace("[RenderConnectivity] finished using");
        }
    }
}
