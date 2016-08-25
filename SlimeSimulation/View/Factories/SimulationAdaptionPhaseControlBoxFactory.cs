using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;

namespace SlimeSimulation.View.Factories
{
    class SimulationAdaptionPhaseControlBoxFactory : ISimulationControlBoxFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AbstractSimulationControlBox MakeControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Logger.Debug("[MakeControlBox] Making");
            return new SimulationAdaptionPhaseControlBox(simulationStepWindowController, parentWindow);
        }
    }
}
