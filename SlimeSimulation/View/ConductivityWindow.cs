using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using System.Collections.Generic;
using SlimeSimulation.Model;

namespace SlimeSimulation.View {
    class ConductivityWindow : WindowTemplate {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Edge> edges;

        public ConductivityWindow(List<Edge> edges) : base("ConductivityWindow") {
            this.edges = edges;
        }

        protected override void AddToWindow(Window window) {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            window.Add(hbox);
            DrawingArea flowResultArea = new GraphDrawingArea(edges, new ConnectivityLineWidthController(edges),
                new ConnectivityNodeHighligthController());
            hbox.Add(flowResultArea);
        }

    }
}
