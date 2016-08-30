using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent;
using SlimeSimulation.View.Windows;

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
            Add(new SimulationStepButton(simulationStepWindowController));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepWindowController, parentWindow, controlInterfaceStartingValues.NumberOfStepsToRun));
            Add(new ShouldFlowResultsBeDisplayedControlComponent(controlInterfaceStartingValues));
            Add(new ShouldStepFromAllSourcesAtOnceControlComponent(controlInterfaceStartingValues));
        }
    }
}
