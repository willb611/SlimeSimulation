using Gtk;

namespace SlimeSimulation.View.WindowComponent
{
    internal class FlowAmountLabelComponent : Label
    {
        private readonly double _flowAmount;
        public FlowAmountLabelComponent(double flowAmount) : base("Network with amount of flow: " + flowAmount)
        {
            _flowAmount = flowAmount;
        }
    }
}
