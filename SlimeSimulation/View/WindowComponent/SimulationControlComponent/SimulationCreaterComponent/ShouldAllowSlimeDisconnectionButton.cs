using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
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
