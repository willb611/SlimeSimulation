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
        private ISet<Edge> edges;

        public FlowNetworkGraphController(MainView view, ISet<Edge> edges) {
            this.view = view;
            this.edges = edges;
        }

        public override void Render() {
            logger.Debug("[RenderConnectivity] Making new window");
            using (window = new FlowNetworkGraphWindow(new List<Edge>(edges), this)) {
                view.Display(window);
            }
        }

        public override void OnClick() {
            logger.Debug("[OnClick] Clicked!");
        }
    }
}
