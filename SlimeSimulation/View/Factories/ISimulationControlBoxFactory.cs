using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;

namespace SlimeSimulation.View.Factories
{
    public interface ISimulationControlBoxFactory
    {
        AbstractSimulationControlBox MakeControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow);
    }
}
