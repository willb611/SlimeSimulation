using System;
using System.Collections.Generic;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent;

namespace SlimeSimulation.View.WindowComponent
{
    public class SimulationUpdateParameterComponent : Table
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed;

        private readonly CheckButton _shouldAllowDisconnectionCheckButton;
        private readonly SlimeNetworkAdaptionCalculatorComponent _slimeNetworkAdaptionComponent;
        private readonly FlowAmountControlComponent _flowAmountControlComponent;

        public bool ShouldAllowDisconnection => _shouldAllowDisconnectionCheckButton.Active;

        public SimulationUpdateParameterComponent(SimulationConfiguration defaultConfiguration) : base(4, 1, false)
        {
            _slimeNetworkAdaptionComponent = new SlimeNetworkAdaptionCalculatorComponent(defaultConfiguration.SlimeNetworkAdaptionCalculatorConfig);
            _flowAmountControlComponent = new FlowAmountControlComponent(defaultConfiguration.FlowAmount);
            _shouldAllowDisconnectionCheckButton = new ShouldAllowSlimeDisconnectionButton(defaultConfiguration.ShouldAllowDisconnection);

            Attach(_slimeNetworkAdaptionComponent, 0, 1, 0, 2);
            Attach(_flowAmountControlComponent, 0, 1, 2, 3);
            Attach(_shouldAllowDisconnectionCheckButton, 0, 1, 3, 4);
        }

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Logger.Debug("[Dispose] Overriden method called from within " + this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {                return;
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

        public SlimeNetworkAdaptionCalculatorConfig ReadSlimeNetworkAdaptionCalculatorConfiguration()
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
