using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;
using System.Diagnostics;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    class SlimeNetworkWindowController : SimulationStepWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISet<SlimeEdge> _edges;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;

        public SlimeNetworkWindowController(SimulationController mainController, ISet<SlimeEdge> edges,
            ISimulationControlBoxFactory simulationControlBoxFactory)
          : base(mainController)
        {
            _edges = edges;
            _simulationControlBoxFactory = simulationControlBoxFactory;
        }

        public override void Render()
        {
            Logger.Debug("[RenderConnectivity] Making new window");
            using (Window = new SlimeNetworkWindow(new List<SlimeEdge>(_edges), this, _simulationControlBoxFactory))
            {
                SimulationController.Display(Window);
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
            SimulationController.ShouldFlowResultsBeDisplayed = shouldResultsBeDisplayed;
        }

        internal bool WillFlowResultsBeDisplayed
        {
            get { return SimulationController.ShouldFlowResultsBeDisplayed; }
            set
            {
                var slimeNetworkWindow = Window as SlimeNetworkWindow;
                Debug.Assert(slimeNetworkWindow != null, nameof(slimeNetworkWindow) + " != null");
                slimeNetworkWindow?.UpdateWillFlowResultsBeDisplayed(value);
            }
        }
    }
}
