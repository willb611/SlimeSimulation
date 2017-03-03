using System;
using System.Text;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.View.Windows
{
    public class ModifySimulationConfigurationWindow : AbstractWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ModifySimulationConfigurationController _controller;
        private readonly SimulationConfiguration _previousConfiguration;

        private SimulationUpdateParameterComponent _simulationUpdateParameterComponent;

        public ModifySimulationConfigurationWindow(ModifySimulationConfigurationController controller,
            SimulationConfiguration simulationConfiguration) 
            : base("Modify the configuration", controller)
        {
            _controller = controller;
            _previousConfiguration = simulationConfiguration;
        }

        protected override void AddToWindow(Window window)
        {
            _simulationUpdateParameterComponent =
                new SimulationUpdateParameterComponent(_previousConfiguration);
            var button = new Button("Update configuration");
            button.Clicked += ButtonOnClicked;

            var container = new VBox {PreviousConfigurationDisplay(), _simulationUpdateParameterComponent, button};
            window.Add(container);
        }

        private Widget PreviousConfigurationDisplay()
        {
            return new HBox
            {
                new Label("Previous configuration"),
                new SimulationConfigurationDisplay(_previousConfiguration)
            };
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            var shouldAllowDisconnection = _simulationUpdateParameterComponent.ShouldAllowDisconnection;
            var flowAmount = _simulationUpdateParameterComponent.ReadFlowAmountConfiguration();
            var slimeNetworkAdaptionCalculatorConfig = _simulationUpdateParameterComponent.ReadSlimeNetworkAdaptionCalculatorConfiguration();
            if (flowAmount.HasValue && slimeNetworkAdaptionCalculatorConfig != null)
            {
                var config = new SimulationConfiguration(_previousConfiguration.GenerationConfig, flowAmount.Value,
                    slimeNetworkAdaptionCalculatorConfig, shouldAllowDisconnection);
                _controller.ContinueSimulationWithConfiguration(config);
            }
            else
            {
                Logger.Debug("[ButtonOnClicked] Not progresing due to invalid values");
                StringBuilder errorMessage = new StringBuilder();
                foreach (var error in _simulationUpdateParameterComponent.Errors())
                {
                    errorMessage.Append(error).Append(Environment.NewLine);
                }
                _controller.DisplayError(errorMessage.ToString());
            }
        }
    }
}
