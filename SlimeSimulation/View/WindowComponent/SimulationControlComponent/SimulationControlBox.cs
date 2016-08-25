using Gtk;

namespace SlimeSimulation.View.WindowComponent
{
    public abstract class SimulationControlBox : VBox
    {

        public SimulationControlBox() : this(true, 10)
        {
        }
        public SimulationControlBox(bool homogeneous, int spacing) : base(homogeneous, spacing)
        {
        }
    }
}
