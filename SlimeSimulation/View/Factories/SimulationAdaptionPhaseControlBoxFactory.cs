using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Factories
{
    class SimulationAdaptionPhaseControlBoxFactory : ISimulationControlBoxFactory
    {
        public SimulationControlBox MakeControlBox(SimulationStepWindowController simulationStepWindowController, Window parentWindow)
        {
            return new SimulationAdaptionPhaseControlBox(simulationStepWindowController, parentWindow);
        }
    }
}
