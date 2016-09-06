using System;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.Model;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class SlimeNetworkWindowController : SimulationStepAbstractWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SlimeNetwork _slimeNetwork;
        private readonly GraphWithFoodSources _graphWithFoodSources;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;
        
        public SlimeNetworkWindowController(SimulationController mainController, SlimeNetwork slimeNetwork, GraphWithFoodSources graphWithFoodSources,
            ISimulationControlBoxFactory simulationControlBoxFactory)
          : base(mainController)
        {
            if (mainController == null)
            {
                throw new ArgumentNullException(nameof(mainController));
            } else if (slimeNetwork == null)
            {
                throw new ArgumentNullException(nameof(slimeNetwork));
            } else if (graphWithFoodSources == null)
            {
                throw new ArgumentNullException(nameof(graphWithFoodSources));
            } else if (simulationControlBoxFactory == null)
            {
                throw new ArgumentNullException(nameof(simulationControlBoxFactory));
            }
            _slimeNetwork = slimeNetwork;
            Logger.Debug("[constructor] Given slimeNetwork: {0}", slimeNetwork);
            _graphWithFoodSources = graphWithFoodSources;
            _simulationControlBoxFactory = simulationControlBoxFactory;
        }

        public override void Render()
        {
            Logger.Debug("[Render] Making new window");
            using (AbstractWindow = new SlimeNetworkWindow(_slimeNetwork, _graphWithFoodSources, this, _simulationControlBoxFactory))
            {
                SimulationController.Display(AbstractWindow);
            }
        }
        
        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClickCallback] Clicked!");
            var area = widget as GraphDrawingArea;
            area?.InvertEdgeDrawing();
        }
        
        public void ReDraw()
        {
            AbstractWindow.Display();
        }
    }
}
