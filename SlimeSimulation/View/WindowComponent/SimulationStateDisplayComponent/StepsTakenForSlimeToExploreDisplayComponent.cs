using Gtk;
using SlimeSimulation.Controller.WindowController.Templates;

namespace SlimeSimulation.View.Windows
{
    internal class StepsTakenForSlimeToExploreDisplayComponent : Label
    {
        private SimulationStepWindowControllerTemplate _controller;
        public StepsTakenForSlimeToExploreDisplayComponent(SimulationStepWindowControllerTemplate controller) : base("")
        {
            _controller = controller;
            Text = "Steps taken for slime to explore fully: " + controller.StepsTakenForSlimeToExplore;
        }
    }
}
