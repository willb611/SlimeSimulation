using System;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.Windows
{
    internal class ShouldFlowResultsBeDisplayedControlComponent : CheckButton
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SimulationControlInterfaceValues _simulationControlBoxConfig;

        public ShouldFlowResultsBeDisplayedControlComponent(SimulationControlInterfaceValues simulationControlBoxConfig) 
            : base("Should simulation step result (flow graph) be displayed?")
        {
            _simulationControlBoxConfig = simulationControlBoxConfig;
            Setup();
        }

        private void Setup()
        {
            Active = _simulationControlBoxConfig.ShouldFlowResultsBeDisplayed;
            Toggled += delegate(object sender, EventArgs args)
            {
                _simulationControlBoxConfig.ShouldFlowResultsBeDisplayed = !_simulationControlBoxConfig.ShouldFlowResultsBeDisplayed;
            };
            Logger.Debug("[ShouldSimulationStepResultsBeDisplayedInput] Setting initial value to: " + _simulationControlBoxConfig.ShouldFlowResultsBeDisplayed);
        }
    }
}
