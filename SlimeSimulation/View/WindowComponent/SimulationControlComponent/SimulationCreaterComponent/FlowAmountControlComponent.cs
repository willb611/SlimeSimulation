using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent
{
    public class FlowAmountControlComponent : HBox
    {
        private TextView _flowAmountTextView;
        public TextView TextView => _flowAmountTextView;

        public FlowAmountControlComponent(double flowAmount) : base()
        {
            _flowAmountTextView = new TextView();
            _flowAmountTextView.Buffer.Text = flowAmount.ToString();
            Add(DescriptionLabel());
            Add(_flowAmountTextView);
        }

        private Label DescriptionLabel()
        {
            return new Label("Flow through system per iteration");
        }
    }
}
