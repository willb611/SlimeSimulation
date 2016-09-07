using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    internal class GrowthPhaseControlBox : AbstractSimulationControlBox
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GrowthPhaseControlBox(SimulationStepAbstractWindowController simulationStepAbstractWindowController, Window parentWindow)
        {
            AddControls(simulationStepAbstractWindowController, parentWindow);
        }

        private void AddControls(SimulationStepAbstractWindowController simulationStepAbstractWindowController, Window parentWindow)
        {
            Add(new SimulationStepButton(simulationStepAbstractWindowController, parentWindow));
            Add(new SimulationStepUntilFullyGrownComponent(simulationStepAbstractWindowController, parentWindow));
        }
    }
}
