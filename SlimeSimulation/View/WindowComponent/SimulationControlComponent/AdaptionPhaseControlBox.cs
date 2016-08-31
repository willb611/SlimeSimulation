using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    class AdaptionPhaseControlBox : AbstractSimulationControlBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public AdaptionPhaseControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            var controlInterfaceStartingValues = simulationStepWindowController.SimulationControlInterfaceValues;
            Add(new SimulationStepButton(simulationStepWindowController, parentWindow));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepWindowController, parentWindow, controlInterfaceStartingValues.NumberOfStepsToRun));
            Add(new ShouldFlowResultsBeDisplayedControlComponent(controlInterfaceStartingValues));
            Add(new ShouldStepFromAllSourcesAtOnceControlComponent(controlInterfaceStartingValues));
        }
    }
}
