using Gtk;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent
{
    public abstract class AbstractSimulationControlBox : VBox
    {

        public AbstractSimulationControlBox() : this(true, 10)
        {
        }
        public AbstractSimulationControlBox(bool homogeneous, int spacing) : base(homogeneous, spacing)
        {
        }

        public abstract void ReDraw();
    }
}
