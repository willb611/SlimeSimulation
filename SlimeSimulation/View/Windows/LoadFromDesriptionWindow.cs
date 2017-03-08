using System;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.View.Windows.Templates;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.WindowComponent.SimulationCreationComponent;

namespace SlimeSimulation.View.Windows
{
    public class LoadFromDescriptionWindow : AbstractWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly NewSimulationFromFileDescriptionWindowController _windowController;
        private readonly SimulationConfiguration _defaultConfig = new SimulationConfiguration();

        private Button _beginSimulationComponent;
        
        private ErrorDisplayComponent _errorDisplayComponent;
        private SimulationUpdateParameterComponent _simulationUpdateParameterComponent;

        public LoadFromDescriptionWindow(string windowTitle, NewSimulationFromFileDescriptionWindowController windowController)
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
            Table container = new Table(10, 1, false);
            _simulationUpdateParameterComponent = new SimulationUpdateParameterComponent(_defaultConfig);
            //_fileToLoadFromInputComponent = new FileToLoadFromInputComponent();
            //_beginSimulationComponent = new NewSimulationFromDescriptionComponent(this, _windowController);
            _errorDisplayComponent = new ErrorDisplayComponent();

            container.Attach(_simulationUpdateParameterComponent, 0, 1, 0, 3);
            //container.Attach(_fileToLoadFromInputComponent, 0, 1, 3, 4);
            //container.Attach(_beginSimulationComponent, 0, 1, 4, 5);
            container.Attach(new Label("Test"), 0, 10, 4, 5);
            container.Attach(_errorDisplayComponent, 0, 1, 5, 6);
            return container;
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
                    // Get input file ? 
                    if (true) // if input ok?
                    {
                        var config = new GraphWithFoodSourceGenerationConfig(null,
                            GraphGeneratorFactory.GenerateFromFileType);
                        return new SimulationConfiguration(config, flowAmount.Value,
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
                _simulationUpdateParameterComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
