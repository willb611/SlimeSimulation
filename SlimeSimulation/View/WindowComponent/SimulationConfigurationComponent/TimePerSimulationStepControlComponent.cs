using System.Collections.Generic;
using Gtk;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent
{
    public class TimePerSimulationStepControlComponent : HBox
    {

        private const string DescriptionString = "Time per simulation step. Should be a multiple, probably less than 1.";
        private List<string> _errors;
        private readonly TextView _timePerSimulationStepTextView;


        public TimePerSimulationStepControlComponent(double defaultTimePerSimulationStep)
        {
            Label description = new Label(DescriptionString);
            TextView textView = new TextView();
            textView.Buffer.Text = defaultTimePerSimulationStep.ToString();
            _timePerSimulationStepTextView = textView;
            Add(description);
            Add(textView);
        }

        public double? ReadFeedbackParameter()
        {
            _errors = new List<string>();
            var feedbackParameter = _timePerSimulationStepTextView.ExtractDoubleFromView();
            if (!feedbackParameter.HasValue)
            {
                _errors.Add("Invalid value for: " + DescriptionString);
            }
            return feedbackParameter;
        }

        internal List<string> Errors()
        {
            return _errors;
        }
    }
}
