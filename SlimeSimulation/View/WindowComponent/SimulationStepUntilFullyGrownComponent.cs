using System;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    internal class SimulationStepUntilFullyGrownComponent : HBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Window _parentWindow;
        private readonly SimulationStepWindowControllerTemplate _simulationStepWindowController;

        public SimulationStepUntilFullyGrownComponent(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            this._simulationStepWindowController = simulationStepWindowController;
            this._parentWindow = parentWindow;

            var doStepsButton = new Button(new Label("Run until slime has finished expanding"));
            doStepsButton.Clicked += DoStepsButtonOnClicked;
            
            Add(doStepsButton);
        }

        private void DoStepsButtonOnClicked(object sender, EventArgs eventArgs)
        {
            _simulationStepWindowController.RunStepsUntilSlimeHasFullyExplored();
        }
    }
}
