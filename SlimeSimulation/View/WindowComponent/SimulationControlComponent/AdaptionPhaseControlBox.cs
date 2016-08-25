using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    class AdaptionPhaseControlBox : AbstractSimulationControlBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ShouldFlowResultsBeDisplayedControlComponent _shouldFlowResultsBeDisplayed;

        public AdaptionPhaseControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepWindowController));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepWindowController, parentWindow));
            TryToAddShouldFlowResultsBeDisplayedComponent(simulationStepWindowController);
        }

        private void TryToAddShouldFlowResultsBeDisplayedComponent(SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            var slimeWindowController = simulationStepWindowController as SlimeNetworkWindowController;
            if (slimeWindowController == null)
            {
                Logger.Warn(
                    $"[AddControls] Expected {nameof(simulationStepWindowController)} to be of type {nameof(slimeWindowController)} but wasn't");
            }
            else
            {
                _shouldFlowResultsBeDisplayed = new ShouldFlowResultsBeDisplayedControlComponent(slimeWindowController);
                Add(_shouldFlowResultsBeDisplayed);
            }
        }

        public override void ReDraw()
        {
            _shouldFlowResultsBeDisplayed?.ReDraw();
        }
    }
}
