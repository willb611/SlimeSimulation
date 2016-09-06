using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows.Templates;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent;
using SlimeSimulation.StdLibHelpers;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Windows
{
    public class ApplicationStartWindow : WindowTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationStartWindowController _windowController;
        private readonly SimulationConfiguration _defaultConfig = new SimulationConfiguration();
       
        private Button _beginSimulationComponent;
        private CheckButton _shouldAllowDisconnectionCheckButton;

        private FlowAmountControlComponent _flowAmountControlComponent;
        private LatticeGenerationControlComponent _latticeGenerationControlComponent;
        private FeedbackParameterControlComponent _slimeNetworkAdaptionComponent;
        private ErrorDisplayComponent _errorDisplayComponent;

        public ApplicationStartWindow(string windowTitle, ApplicationStartWindowController windowController)
            : base(windowTitle, windowController)
        {
            _windowController = windowController;
            Window.Resize(600, 600);
        }

        protected override void AddToWindow(Window window)
        {
            var container = MakeComponents();
            container.Add(_flowAmountControlComponent);
            container.Add(_slimeNetworkAdaptionComponent);
            container.Add(_latticeGenerationControlComponent);
            container.Add(_shouldAllowDisconnectionCheckButton);
            container.Add(_beginSimulationComponent);
            container.Add(_errorDisplayComponent);
            window.Add(container);
            window.Unmaximize();
        }

        private VBox MakeComponents()
        {
            _slimeNetworkAdaptionComponent = new FeedbackParameterControlComponent(_defaultConfig.SlimeNetworkAdaptionCalculatorConfig);
            _flowAmountControlComponent = new FlowAmountControlComponent(_defaultConfig.FlowAmount);
            _shouldAllowDisconnectionCheckButton = new ShouldAllowSlimeDisconnectionButton(_defaultConfig.ShouldAllowDisconnection);
            _beginSimulationComponent = new BeginSimulationComponent(this, _windowController);
            _errorDisplayComponent = new ErrorDisplayComponent();
            _latticeGenerationControlComponent =
                new LatticeGenerationControlComponent(_defaultConfig.GenerationConfig);
            return new VBox();
        }

        internal SimulationConfiguration GetConfigFromViews()
        {
            var slimeNetworkAdaptionConfig = _slimeNetworkAdaptionComponent.ReadConfiguration();
            _errorDisplayComponent.AddToDisplayBuffer(_slimeNetworkAdaptionComponent.Errors());
            double? flowAmount = _flowAmountControlComponent.ReadFlowAmount();
            _errorDisplayComponent.AddToDisplayBuffer(_flowAmountControlComponent.Errors());
            bool shouldAllowDisconnection = _shouldAllowDisconnectionCheckButton.Active;
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
                _flowAmountControlComponent.Dispose();
                _latticeGenerationControlComponent.Dispose();
                _shouldAllowDisconnectionCheckButton.Dispose();
                _slimeNetworkAdaptionComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        ~ApplicationStartWindow()
        {
            Dispose(false);
        }
    }
}
