using Gtk; 

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    internal class TimePerSimulationStepDisplayComponent : Label
    {
        private double timePerSimulationStep;

        public TimePerSimulationStepDisplayComponent(double timePerSimulationStep) : base("When updating network in simulation steps, each step counts as " + timePerSimulationStep + " timeSteps ")
        {
            this.timePerSimulationStep = timePerSimulationStep;
        }
    }
}
