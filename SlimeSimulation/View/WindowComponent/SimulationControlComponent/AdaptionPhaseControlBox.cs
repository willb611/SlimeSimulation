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
            var container = new Table(6, 1, true);
            container.Attach(new SimulationStepButton(simulationStepAbstractWindowController, parentWindow), 0, 1, 0, 1);
            container.Attach(new SimulationStepNumberOfTimesComponent(simulationStepAbstractWindowController, parentWindow, controlInterfaceStartingValues), 0, 1, 1, 3);
            container.Attach(new ShouldFlowResultsBeDisplayedControlComponent(controlInterfaceStartingValues), 0, 1, 3, 4);
            container.Attach(new ShouldStepFromAllSourcesAtOnceControlComponent(controlInterfaceStartingValues), 0, 1, 4, 5);
            container.Attach(new SimulationSaveComponent(simulationStepAbstractWindowController.SimulationController, parentWindow), 0, 1, 5, 6);

            Add(container);
        }
    }
}
