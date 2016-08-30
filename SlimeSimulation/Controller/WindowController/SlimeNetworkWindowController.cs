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
                Logger.Debug("[Render] displaying using controller");
                SimulationController.Display(Window);
                Logger.Debug("[Render] SimulationController.Display(Window); has finihsed");
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
            Window.Display();
        }
    }
}
