using System;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreation
{
    public class CreateNewSimulationComponent : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private NewSimulationStarterAbstractWindow _enclosingWindow;
        private NewSimulationStarterWindowController _enclosingWindowController;

        public CreateNewSimulationComponent(NewSimulationStarterAbstractWindow enclosingWindow,
            NewSimulationStarterWindowController controller)
            : base (new Label("Begin simulation")) {
            _enclosingWindow = enclosingWindow;
            _enclosingWindowController = controller;
            Clicked += CreateNewSimulationComponent_Clicked;
        }

        private void CreateNewSimulationComponent_Clicked(object sender, EventArgs e)
        {
            Logger.Debug("[BeginSimulationComponent_Clicked] Entered");
            var config = _enclosingWindow.GetConfigFromViews();
            if (config == null)
            {
                Logger.Info("[BeginSimulationComponent_Clicked] Not starting simulation due to invalid parameters");
            }
            else
            {
                _enclosingWindowController.StartSimulation(config);
            }
        }
    }
}
