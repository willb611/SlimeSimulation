using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    class SimulationInitialConfigurationDisplayComponent : VBox
    {
        private readonly SimulationStepWindowControllerTemplate _simulationStepWindowController;

        public SimulationInitialConfigurationDisplayComponent(
            SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            _simulationStepWindowController = simulationStepWindowController;
            AddComponents(simulationStepWindowController);
        }

        private void AddComponents(SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            Add(new IsSlimeAllowedToDisconnectLabel(simulationStepWindowController.IsSlimeAllowedToDisconnect()));
        }
    }
}

