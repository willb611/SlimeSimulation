using System;
using System.Collections.Generic;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationUpdateParameters;

namespace SlimeSimulation.View.Windows
{
    public class SimulationUpdateParameterComponent : VBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed;

        private readonly CheckButton _shouldAllowDisconnectionCheckButton;
        private readonly FlowAmountControlComponent _flowAmountControlComponent;
        private readonly FeedbackParameterControlComponent _slimeNetworkAdaptionComponent;

        public bool ShouldAllowDisconnection => _shouldAllowDisconnectionCheckButton.Active;

        public SimulationUpdateParameterComponent(SimulationConfiguration defaultConfiguration)
        {
            _slimeNetworkAdaptionComponent = new FeedbackParameterControlComponent(defaultConfiguration.SlimeNetworkAdaptionCalculatorConfig);
            _flowAmountControlComponent = new FlowAmountControlComponent(defaultConfiguration.FlowAmount);
            _shouldAllowDisconnectionCheckButton = new ShouldAllowSlimeDisconnectionButton(defaultConfiguration.ShouldAllowDisconnection);

            Add(_slimeNetworkAdaptionComponent);
            Add(_flowAmountControlComponent);
            Add(_shouldAllowDisconnectionCheckButton);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Logger.Debug("[Dispose] Overriden method called from within " + this);
        }

        protected void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                base.Dispose();
                _flowAmountControlComponent.Dispose();
                _shouldAllowDisconnectionCheckButton.Dispose();
                _slimeNetworkAdaptionComponent.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        ~SimulationUpdateParameterComponent()
        {
            Dispose(false);
        }

        public SlimeNetworkAdaptionCalculatorConfig ReadSlimeNetworkAdaptionConfiguration()
        {
            return _slimeNetworkAdaptionComponent.ReadConfiguration();
        }

        public double? ReadFlowAmountConfiguration()
        {
            return _flowAmountControlComponent.ReadFlowAmount();
        }

        public List<string> Errors()
        {
            var errors = new List<string>();
            errors.AddRange(_flowAmountControlComponent.Errors());
            errors.AddRange(_slimeNetworkAdaptionComponent.Errors());
            return errors;
        }
    }
}
