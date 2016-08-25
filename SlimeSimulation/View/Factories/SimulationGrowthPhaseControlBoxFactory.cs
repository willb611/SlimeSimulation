using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Factories
{
    internal class SimulationGrowthPhaseControlBoxFactory : ISimulationControlBoxFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SimulationControlBox MakeControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Logger.Debug("[MakeControlBox] Making");
            return new SimulationGrowthPhaseControlBox(simulationStepWindowController, parentWindow);
        }
    }
}
