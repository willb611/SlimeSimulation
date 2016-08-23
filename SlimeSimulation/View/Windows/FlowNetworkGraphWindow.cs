using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using System.Collections.Generic;
using SlimeSimulation.Model;
using System;
using SlimeSimulation.Controller;
using SlimeSimulation.Controller.WindowsComponentController;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation.View.Windows
{
    class FlowNetworkGraphWindow : GraphDrawingWindowTemplate
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<Edge> _edges;
        private FlowNetworkGraphController _controller;
        
        public FlowNetworkGraphWindow(List<Edge> edges, FlowNetworkGraphController controller)
          : base("ConductivityWindow", controller)
        {
            this._edges = edges;
            this._controller = controller;
        }

        protected override void AddToWindow(Window window)
        {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            window.Add(hbox);
            GraphDrawingArea = new GraphDrawingArea(_edges, new ConnectivityLineViewController(_edges),
              new ConnectivityNodeViewController());
            base.ListenToClicksOn(GraphDrawingArea);

            hbox.Add(GraphDrawingArea);
        }

    }
}
