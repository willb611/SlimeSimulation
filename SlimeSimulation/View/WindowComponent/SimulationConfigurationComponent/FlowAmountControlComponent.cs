using System.Collections.Generic;
using Gtk;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent
{
    public class FlowAmountControlComponent : HBox
    {
        private TextView _flowAmountTextView;
        private List<string> _errors;
        private string descriptionString = "Flow through system per iteration";

        public FlowAmountControlComponent(double flowAmount) : base()
        {
            _flowAmountTextView = new TextView();
            _flowAmountTextView.Buffer.Text = flowAmount.ToString();
            Add(DescriptionLabel());
            Add(_flowAmountTextView);
        }

        private Label DescriptionLabel()
        {
            return new Label(descriptionString);
        }

        public double? ReadFlowAmount()
        {
            _errors = new List<string>();
            var valueRead = _flowAmountTextView.ExtractDoubleFromView();
            if (valueRead.HasValue)
            {
                return valueRead;
            } else
            {
                _errors.Add("Invalid value for: " + descriptionString);
                return null;
            }
        }

        internal List<string> Errors()
        {
            return _errors;
        }
    }
}
