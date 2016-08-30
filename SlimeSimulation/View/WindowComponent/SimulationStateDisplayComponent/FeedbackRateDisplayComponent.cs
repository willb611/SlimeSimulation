using Gtk;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    internal class FeedbackRateDisplayComponent : Label
    {
        public FeedbackRateDisplayComponent(double feedbackUsedWhenAdaptingNetwork) : base("")
        {
            Text = "When updating network in simulation steps, feedback parameter used is: " + feedbackUsedWhenAdaptingNetwork;
        }
    }
}
