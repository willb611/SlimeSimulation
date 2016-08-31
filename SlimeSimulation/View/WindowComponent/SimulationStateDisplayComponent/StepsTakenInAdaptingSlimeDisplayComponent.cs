using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    public class StepsTakenInAdaptingSlimeDisplayComponent : Label
    {
        private readonly SimulationStepWindowControllerTemplate _controller;

        public StepsTakenInAdaptingSlimeDisplayComponent(SimulationStepWindowControllerTemplate controller) : base("")
        {
            _controller = controller;
            Text = "Simulation steps completed: " + _controller.StepsCompletedSoFarInAdaptingSlime;
        }
    }
}