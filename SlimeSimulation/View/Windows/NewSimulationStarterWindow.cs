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
       
        private Button _beginSimulationComponent;

        private LatticeGenerationControlComponent _latticeGenerationControlComponent;
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
            var container = MakeComponentsAndReturnContainerForThem();
            container.Add(_simulationUpdateParameterComponent);
            container.Add(_latticeGenerationControlComponent);
            container.Add(_beginSimulationComponent);
            container.Add(_errorDisplayComponent);
            window.Add(container);
            window.Unmaximize();
        }

        private VBox MakeComponentsAndReturnContainerForThem()
        {
            _simulationUpdateParameterComponent = new SimulationUpdateParameterComponent(_defaultConfig);
            _beginSimulationComponent = new CreateNewSimulationComponent(this, _windowController);
            _errorDisplayComponent = new ErrorDisplayComponent();
            _latticeGenerationControlComponent =
                new LatticeGenerationControlComponent(_defaultConfig.GenerationConfig);
            return new VBox();
        }

        internal SimulationConfiguration GetConfigFromViews()
        {
            var slimeNetworkAdaptionConfig = _simulationUpdateParameterComponent.ReadSlimeNetworkAdaptionCalculatorConfiguration();
            double? flowAmount = _simulationUpdateParameterComponent.ReadFlowAmountConfiguration();
            bool shouldAllowDisconnection = _simulationUpdateParameterComponent.ShouldAllowDisconnection;
            _errorDisplayComponent.AddToDisplayBuffer(_simulationUpdateParameterComponent.Errors());
            if (slimeNetworkAdaptionConfig != null && flowAmount.HasValue)
            {
                try
                {
                    var generationConfig = _latticeGenerationControlComponent.ReadGenerationConfig();
                    _errorDisplayComponent.AddToDisplayBuffer(_latticeGenerationControlComponent.Errors());
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
            _errorDisplayComponent.UpdateDisplayFromBuffer();
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
                _beginSimulationComponent.Dispose();
                _errorDisplayComponent.Dispose();
                _latticeGenerationControlComponent.Dispose();
                _simulationUpdateParameterComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
