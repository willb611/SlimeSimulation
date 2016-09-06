using System;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    internal class SimulationStepUntilFullyGrownComponent : HBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Window _parentWindow;
        private readonly SimulationStepAbstractWindowController _simulationStepAbstractWindowController;

        public SimulationStepUntilFullyGrownComponent(SimulationStepAbstractWindowController simulationStepAbstractWindowController, Window parentWindow)
        {
            this._simulationStepAbstractWindowController = simulationStepAbstractWindowController;
            this._parentWindow = parentWindow;

            var doStepsButton = new Button(new Label("Run until slime has finished expanding"));
            doStepsButton.Clicked += DoStepsButtonOnClicked;
            
            Add(doStepsButton);
        }

        private void DoStepsButtonOnClicked(object sender, EventArgs eventArgs)
        {
            _simulationStepAbstractWindowController.RunStepsUntilSlimeHasFullyExplored();
        }
    }
}
