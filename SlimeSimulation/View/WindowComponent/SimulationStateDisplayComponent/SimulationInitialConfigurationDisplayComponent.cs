using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    class SimulationInitialConfigurationDisplayComponent : HBox
    {
        private readonly SimulationStepAbstractWindowController _simulationStepAbstractWindowController;

        public SimulationInitialConfigurationDisplayComponent(
            SimulationStepAbstractWindowController simulationStepAbstractWindowController)
        {
            _simulationStepAbstractWindowController = simulationStepAbstractWindowController;
            Add(new Label("Initial simulation parameters:"));
            Add(InitialParameters(simulationStepAbstractWindowController));
        }

        private VBox InitialParameters(SimulationStepAbstractWindowController simulationStepAbstractWindowController)
        {
            var box = new VBox();
            box.Add(new IsSlimeAllowedToDisconnectLabel(simulationStepAbstractWindowController.IsSlimeAllowedToDisconnect));
            box.Add(new FlowAmountLabelComponent(simulationStepAbstractWindowController.FlowUsedInAdaptingNetwork));
            box.Add(new FeedbackRateDisplayComponent(simulationStepAbstractWindowController.FeedbackUsedWhenAdaptingNetwork));
            return box;
        }
    }
}

