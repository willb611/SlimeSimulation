using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    class SimulationAdaptionPhaseControlBox : AbstractSimulationControlBox
    {

        public SimulationAdaptionPhaseControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepWindowController));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepWindowController, parentWindow));
        }
    }
}
