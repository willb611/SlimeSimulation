using Gtk;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    internal class FlowAmountLabelComponent : Label
    {
        private readonly double _flowAmount;
        public FlowAmountLabelComponent(double flowAmount) : base("When updating network in simulation steps, flow pushed through network is: " + flowAmount)
        {
            _flowAmount = flowAmount;
        }
    }
}
