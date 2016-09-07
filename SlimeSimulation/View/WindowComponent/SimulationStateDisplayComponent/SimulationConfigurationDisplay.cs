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
            box.Add(new FlowAmountLabelComponent(simulationConfiguration.FlowAmount));
            box.Add(new FeedbackRateDisplayComponent(simulationConfiguration.SlimeNetworkAdaptionCalculatorConfig.FeedbackParam));
            return box;
        }
    }
}

