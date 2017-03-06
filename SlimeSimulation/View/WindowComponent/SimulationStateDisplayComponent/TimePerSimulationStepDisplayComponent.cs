using Gtk; 

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    internal class TimePerSimulationStepDisplayComponent : Label
    {
        private double timePerSimulationStep;

        public TimePerSimulationStepDisplayComponent(double timePerSimulationStep) 
            : base("When adapting network, each step counts as " + timePerSimulationStep + " units of time.")
        {
            this.timePerSimulationStep = timePerSimulationStep;
        }
    }
}
