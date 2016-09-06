using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    public class StepsTakenInAdaptingSlimeDisplayComponent : Label
    {
        private readonly SimulationStepAbstractWindowController _controller;

        public StepsTakenInAdaptingSlimeDisplayComponent(SimulationStepAbstractWindowController controller) : base("")
        {
            _controller = controller;
            Text = "Simulation steps completed: " + _controller.StepsCompletedSoFarInAdaptingSlime;
        }
    }
}
