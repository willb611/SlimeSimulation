using Gtk;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    class SimulationConfigurationDisplay : HBox
    {

        public SimulationConfigurationDisplay(SimulationConfiguration simulationConfiguration)
        {
            Add(new Label("Simulation configuration parameters:"));
            Add(InitialParameters(simulationConfiguration));
        }

        private VBox InitialParameters(SimulationConfiguration simulationConfiguration)
        {
            var box = new VBox();
            box.Add(new IsSlimeAllowedToDisconnectLabel(simulationConfiguration.ShouldAllowDisconnection));
            box.Add(new FeedbackRateDisplayComponent(simulationConfiguration.SlimeNetworkAdaptionCalculatorConfig.FeedbackParam));
            box.Add(new TimePerSimulationStepDisplayComponent(simulationConfiguration.SlimeNetworkAdaptionCalculatorConfig.TimePerSimulationStep));
            box.Add(new FlowAmountLabelComponent(simulationConfiguration.FlowAmount));
            return box;
        }
    }
}

