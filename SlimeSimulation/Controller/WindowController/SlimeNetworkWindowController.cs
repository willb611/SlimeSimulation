using System.Collections.Generic;
using System.Diagnostics;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.Model;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class SlimeNetworkWindowController : SimulationStepWindowControllerTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SlimeNetwork _slimeNetwork;
        private readonly GraphWithFoodSources _graphWithFoodSources;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;

        internal bool WillFlowResultsBeDisplayed => SimulationController.ShouldFlowResultsBeDisplayed;

        public SlimeNetworkWindowController(SimulationController mainController, SlimeNetwork slimeNetwork, GraphWithFoodSources graphWithFoodSources,
            ISimulationControlBoxFactory simulationControlBoxFactory)
          : base(mainController)
        {
            _slimeNetwork = slimeNetwork;
            _graphWithFoodSources = graphWithFoodSources;
            _simulationControlBoxFactory = simulationControlBoxFactory;
        }

        public override void Render()
        {
            Logger.Debug("[Render] Making new window");
            using (Window = new SlimeNetworkWindow(_slimeNetwork, _graphWithFoodSources, this, _simulationControlBoxFactory))
            {
                SimulationController.Display(Window);
            }
        }
        
        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClickCallback] Clicked!");
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
        
        public void ReDraw()
        {
            Window.Display();
        }
    }
}
