using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    internal class SimulationGrowthPhaseControlBox : SimulationControlBox
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
