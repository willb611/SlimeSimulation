using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using System.Collections.Generic;
using SlimeSimulation.Model;
using System;
using SlimeSimulation.Controller;

namespace SlimeSimulation.View {
    class FlowNetworkGraphWindow : WindowTemplate {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Edge> edges;
        private DrawingArea graphDrawingArea;
        private FlowNetworkGraphController controller;

        public FlowNetworkGraphWindow(List<Edge> edges, FlowNetworkGraphController controller) : base("ConductivityWindow", controller) {
            this.edges = edges;
            this.controller = controller;
        }

        protected override void Dispose(bool disposing) {
            if (disposed) {
                return;
            } else if (disposing) {
                graphDrawingArea.Dispose();
            }
            disposed = true;
            logger.Trace("[Dispose : bool] finished");
        }

        protected override void AddToWindow(Window window) {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            window.Add(hbox);
            graphDrawingArea = new GraphDrawingArea(edges, new ConnectivityLineViewController(edges),
                new ConnectivityNodeViewController());
            hbox.Add(graphDrawingArea);
        }

    }
}
