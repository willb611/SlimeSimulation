using Gtk;

namespace SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent
{
    public class ShouldAllowSlimeDisconnectionButton : CheckButton
    {
        private const string description = "Should the slime be allowed to disconnect from parts of the graph?";

        public ShouldAllowSlimeDisconnectionButton(bool shouldAllowDisconnectionInitial) : base (description)
        {
            Active = shouldAllowDisconnectionInitial;
        }
    }
}
