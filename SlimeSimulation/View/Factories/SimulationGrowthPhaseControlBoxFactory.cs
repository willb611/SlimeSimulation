using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;

namespace SlimeSimulation.View.Factories
{
    internal class SimulationGrowthPhaseControlBoxFactory : ISimulationControlBoxFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AbstractSimulationControlBox MakeControlBox(SimulationStepAbstractWindowController simulationStepAbstractWindowController, Window parentWindow)
        {
            Logger.Debug("[MakeControlBox] Making");
            return new GrowthPhaseControlBox(simulationStepAbstractWindowController, parentWindow);
        }
    }
}
