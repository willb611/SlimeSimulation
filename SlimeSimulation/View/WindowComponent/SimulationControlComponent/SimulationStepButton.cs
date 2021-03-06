using System;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    internal class SimulationStepButton : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly SimulationStepAbstractWindowController _controller;
        private readonly SimulationControlInterfaceValues _simulationControlInterfaceValues;
        private readonly Window _parentWindow;

        public SimulationStepButton(SimulationStepAbstractWindowController controller, Window parentWindow) : base(new Label("Next Simulation Step"))
        {
            _controller = controller;
            _simulationControlInterfaceValues = controller.SimulationControlInterfaceValues;
            _parentWindow = parentWindow;
            Clicked += DoSimulationStepOnClicked;
        }

        private void DoSimulationStepOnClicked(object sender, EventArgs eventArgs)
        {
            if (_simulationControlInterfaceValues.ShouldFlowResultsBeDisplayed && _simulationControlInterfaceValues.ShouldStepFromAllSourcesAtOnce)
            {
                MessageDialog messageDialog = new MessageDialog(_parentWindow, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.OkCancel,
                    "Flow results are set to be displayed, running a step as the average of flow results from each slime food node will disable flow results being displayed. Continue?");
                messageDialog.Title = "Ok to disable showing flow results?";
                ResponseType response = (ResponseType)messageDialog.Run();
                if (response == ResponseType.DeleteEvent || response == ResponseType.Cancel)
                {
                    messageDialog.Destroy();
                    Logger.Debug("[DoSimulationStepOnClicked] Returning as user was not ok with skipping flow result windows");
                    return;
                }
                Logger.Debug("[DoSimulationStepOnClicked] Skip flow results was not enabled, but user confirmed ok to disable flow results");
                _simulationControlInterfaceValues.ShouldFlowResultsBeDisplayed = false;
            }
            _controller.OnStepCompleted();
        }
    }
}
