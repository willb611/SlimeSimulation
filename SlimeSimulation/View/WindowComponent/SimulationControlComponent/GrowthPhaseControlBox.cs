using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    internal class GrowthPhaseControlBox : AbstractSimulationControlBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GrowthPhaseControlBox(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            AddControls(simulationStepWindowController, parentWindow);
        }

        private void AddControls(SimulationStepWindowControllerTemplate simulationStepWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepWindowController, parentWindow));
            Add(new SimulationStepUntilFullyGrownComponent(simulationStepWindowController, parentWindow));
        }
    }
}
