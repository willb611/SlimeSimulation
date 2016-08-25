using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent
{
    class SimulationAdaptionPhaseControlBox : SimulationControlBox
    {

        public SimulationAdaptionPhaseControlBox(SimulationStepWindowController simulationStepWindowController, Window parentWindow) : base(true, 10)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowController simulationStepWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepWindowController));
            Add(new SimulationStepNumberOfTimesComponent(simulationStepWindowController, parentWindow));
        }
    }
}
