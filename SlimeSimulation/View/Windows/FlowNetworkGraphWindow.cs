using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using System.Collections.Generic;
using SlimeSimulation.Model;
using System;
using SlimeSimulation.Controller;
using SlimeSimulation.Controller.WindowsComponentController;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Windows
{
    class FlowNetworkGraphWindow : GraphDrawingWindowTemplate
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<Edge> _edges;
        private readonly FlowNetworkGraphController _controller;
        
        public FlowNetworkGraphWindow(List<Edge> edges, FlowNetworkGraphController controller)
          : base("ConductivityWindow", controller)
        {
            this._edges = edges;
            this._controller = controller;
        }

        protected override void AddToWindow(Window window)
        {
            var bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            var hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            GraphDrawingArea = new GraphDrawingArea(_edges, new ConnectivityLineViewController(_edges),
              new ConnectivityNodeViewController());
            base.ListenToClicksOn(GraphDrawingArea);

            hbox.Add(GraphDrawingArea);

            var vbox = new VBox(false, 10);
            vbox.PackStart(StepNumberLabel(), false, true, 10);
            vbox.PackStart(hbox, true, true, 10);

            window.Add(vbox);
        }

        private Label StepNumberLabel()
        {
            var stepNumber = _controller.StepsSoFarInSimulation();
            return new Label("Simulation steps completed number: " + stepNumber);
        }
    }
}
