using NLog;
using SlimeSimulation.Model;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller.WindowController
{
    class SlimeNetworkWindowController : SimulationStepWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GtkLifecycleController _gtkLifecycleController;
        private readonly ISet<SlimeEdge> _edges;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;

        public SlimeNetworkWindowController(SimulationController mainController, GtkLifecycleController gtkLifecycleController, ISet<SlimeEdge> edges,
            ISimulationControlBoxFactory simulationControlBoxFactory)
          : base(mainController)
        {
            this._gtkLifecycleController = gtkLifecycleController;
            this._edges = edges;
            _simulationControlBoxFactory = simulationControlBoxFactory;
        }

        public override void Render()
        {
            Logger.Debug("[RenderConnectivity] Making new window");
            using (Window = new SlimeNetworkWindow(new List<SlimeEdge>(_edges), this, _simulationControlBoxFactory))
            {
                _gtkLifecycleController.Display(Window);
            }
        }
        
        public override void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClick] Clicked!");
            var area = widget as GraphDrawingArea;
            area?.InvertEdgeDrawing();
        }

        internal int StepsSoFarInSimulation()
        {
            return SimulationController.SimulationStepsCompleted;
        }

        internal void ToggleAreFlowResultsDisplayed(bool shouldResultsBeDisplayed)
        {
            SimulationController.ToggleAreFlowResultsDisplayed(shouldResultsBeDisplayed);
        }

        internal bool WillFlowResultsBeDisplayed()
        {
            return SimulationController.ShouldFlowResultsBeDisplayed;
        }
    }
}
