using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    class SimulationInitialConfigurationDisplayComponent : HBox
    {
        private readonly SimulationStepWindowControllerTemplate _simulationStepWindowController;

        public SimulationInitialConfigurationDisplayComponent(
            SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            _simulationStepWindowController = simulationStepWindowController;
            Add(new Label("Initial simulation parameters:"));
            Add(InitialParameters(simulationStepWindowController));
        }

        private VBox InitialParameters(SimulationStepWindowControllerTemplate simulationStepWindowController)
        {
            var box = new VBox();
            box.Add(new IsSlimeAllowedToDisconnectLabel(simulationStepWindowController.IsSlimeAllowedToDisconnect));
            box.Add(new FlowAmountLabelComponent(simulationStepWindowController.FlowUsedInAdaptingNetwork));
            box.Add(new FeedbackRateDisplayComponent(simulationStepWindowController.FeedbackUsedWhenAdaptingNetwork));
            return box;
        }
    }
}

