using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Factories
{
    public interface ISimulationControlBoxFactory
    {
        SimulationControlBox MakeControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow);
    }
}
