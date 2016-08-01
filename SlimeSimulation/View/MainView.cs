using System;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;

namespace SlimeSimulation.View {
    public class MainView : IDisposable {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainView() {
            logger.Info("Entered constructor");
            Application.Init();
        }

        public void Dispose() {
            logger.Info("[Dispose] Quitting application");
            Application.Quit();
        }

        internal void renderFlowResult(FlowResult flowResult) {
            logger.Debug("Rendering FlowResult");
            var flowWindow = new FlowResultWindow(flowResult);
            flowWindow.Display();
            Application.Run();
        }

        internal void renderConnectivity(ISet<Edge> edges) {
            var window = new ConductivityWindow(new List<Edge>(edges));
            window.Display();
            Application.Run();
        }
    }
}
