using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller
{
    class FlowNetworkGraphController : SimulationStepWindowController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private GtkLifecycleController gtkLifecycleController;
        private ISet<Edge> edges;

        public FlowNetworkGraphController(SimulationController mainController, GtkLifecycleController view, ISet<Edge> edges)
          : base(mainController)
        {
            this.gtkLifecycleController = view;
            this.edges = edges;
        }

        public override void Render()
        {
            logger.Debug("[RenderConnectivity] Making new window");
            using (window = new FlowNetworkGraphWindow(new List<Edge>(edges), this))
            {
                gtkLifecycleController.Display(window);
            }
        }
        
        public override void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args)
        {
            logger.Debug("[OnClick] Clicked!");
            GraphDrawingArea area = widget as GraphDrawingArea;
            if (area != null) { 
                area.InvertEdgeDrawing();
            }
        }
    }
}
