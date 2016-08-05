using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller {
    class ConductivityController {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MainView view;

        public ConductivityController(MainView view) {
            this.view = view;
        }

        public void RenderConnectivity(ISet<Edge> edges) {
            var window = new ConductivityWindow(new List<Edge>(edges));
            view.Display(window);
        }
    }
}
