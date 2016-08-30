using Gtk;

namespace SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent
{
    internal class IsSlimeAllowedToDisconnectLabel : Label
    {
        private bool _isSlimeAllowedToDisconnect;
        public IsSlimeAllowedToDisconnectLabel(bool isSlimeAllowedToDisconnect)
        {
            _isSlimeAllowedToDisconnect = isSlimeAllowedToDisconnect;
            if (isSlimeAllowedToDisconnect)
            {
                base.Text = "Slime can become disconnected";
            }
            else
            {
                base.Text = "Slime will not become disconnected";
            }
        }
    }
}
