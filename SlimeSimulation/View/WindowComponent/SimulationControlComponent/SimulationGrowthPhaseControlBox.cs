using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    internal class SimulationGrowthPhaseControlBox : AbstractSimulationControlBox
    {
        public SimulationGrowthPhaseControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepWindowController));
            Add(new SimulationStepUntilFullyGrownComponent(simulationStepWindowController, parentWindow));
        }
    }
}
