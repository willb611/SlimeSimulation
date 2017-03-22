using System;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows.Templates;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.WindowComponent.SimulationCreationComponent;

namespace SlimeSimulation.View.Windows
{
    public class NewSimulationStarterWindow : AbstractWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly NewSimulationStarterWindowController _windowController;
        private readonly SimulationConfiguration _defaultConfig = new SimulationConfiguration();
       
        private Button _beginSimulationButton;

        private GraphGenerationControlComponent _graphGenerationControlComponent;
        private ErrorDisplayComponent _errorDisplayComponent;
        private SimulationUpdateParameterComponent _simulationUpdateParameterComponent;

        public NewSimulationStarterWindow(string windowTitle, NewSimulationStarterWindowController windowController)
            : base(windowTitle, windowController)
        {
            _windowController = windowController;
            Window.Resize(600, 600);
        }

        protected override void AddToWindow(Window window)
        {
            var container = MakeContainerWithComponents();
            window.Add(container);
            window.Unmaximize();
        }

        private Container MakeContainerWithComponents()
        {
            Table container = new Table(11, 1, false);
            _simulationUpdateParameterComponent = new SimulationUpdateParameterComponent(_defaultConfig);
            _graphGenerationControlComponent = new GraphGenerationControlComponent(_defaultConfig.GenerationConfig);
            _beginSimulationButton = new CreateNewSimulationComponent(this, _windowController);
            _errorDisplayComponent = new ErrorDisplayComponent();

            container.Attach(_simulationUpdateParameterComponent, 0, 1, 0, 3);
            container.Attach(_graphGenerationControlComponent, 0, 1, 4, 8);
            container.Attach(_beginSimulationButton, 0, 1, 9, 10);
            container.Attach(_errorDisplayComponent, 0, 1, 10, 11);
            return container;
        }

        internal SimulationConfiguration GetConfigFromViewsOrDisplayErrors()
        {
            var slimeNetworkAdaptionConfig = _simulationUpdateParameterComponent.ReadSlimeNetworkAdaptionCalculatorConfiguration();
            double? flowAmount = _simulationUpdateParameterComponent.ReadFlowAmountConfiguration();
            bool shouldAllowDisconnection = _simulationUpdateParameterComponent.ShouldAllowDisconnection;
            _errorDisplayComponent.AddToDisplayBuffer(_simulationUpdateParameterComponent.Errors());
            if (slimeNetworkAdaptionConfig != null && flowAmount.HasValue)
            {
                try
                {
                    var generationConfig = _graphGenerationControlComponent.ReadGenerationConfig();
                    _errorDisplayComponent.AddToDisplayBuffer(_graphGenerationControlComponent.Errors());
                    if (generationConfig != null)
                    {
                        return new SimulationConfiguration(generationConfig, flowAmount.Value,
                            slimeNetworkAdaptionConfig, shouldAllowDisconnection);
                    }
                }
                catch (ArgumentException e)
                {
                    string errorMsg = "Invalid parameter: " + e.Message;
                    Logger.Info(errorMsg);
                    _errorDisplayComponent.AddToDisplayBuffer(errorMsg);
                }
            }
            _errorDisplayComponent.DisplayErrorsFromBufferThenClearIt();
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                base.Dispose(true);
                _beginSimulationButton.Dispose();
                _errorDisplayComponent.Dispose();
                _graphGenerationControlComponent.Dispose();
                _simulationUpdateParameterComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
