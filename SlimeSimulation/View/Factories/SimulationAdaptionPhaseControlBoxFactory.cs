using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Factories
{
    class SimulationAdaptionPhaseControlBoxFactory : ISimulationControlBoxFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SimulationControlBox MakeControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Logger.Debug("[MakeControlBox] Making");
            return new SimulationAdaptionPhaseControlBox(simulationStepWindowController, parentWindow);
        }
    }
}
