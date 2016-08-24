using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace SlimeSimulation.View.WindowComponent
{
    abstract class SimulationControlBox : VBox
    {

        public SimulationControlBox(bool homogeneous, int spacing) : base(homogeneous, spacing)
        {
        }
    }
}
