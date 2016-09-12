using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    class AdaptionPhaseControlBox : AbstractSimulationControlBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public AdaptionPhaseControlBox(SimulationStepAbstractWindowController simulationStepAbstractWindowController, Window parentWindow)
        {
            AddControls(simulationStepAbstractWindowController, parentWindow);
        }

        private void AddControls(SimulationStepAbstractWindowController simulationStepAbstractWindowController, Window parentWindow)
        {
            var controlInterfaceStartingValues = simulationStepAbstractWindowController.SimulationControlInterfaceValues;
            Add(new SimulationStepButton(simulationStepAbstractWindowController, parentWindow));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepAbstractWindowController, parentWindow, controlInterfaceStartingValues));
            Add(new ShouldFlowResultsBeDisplayedControlComponent(controlInterfaceStartingValues));
            Add(new ShouldStepFromAllSourcesAtOnceControlComponent(controlInterfaceStartingValues));
            Add(new SimulationSaveComponent(simulationStepAbstractWindowController.SimulationController, parentWindow));
        }
    }
}
