using System.Collections.Generic;
using Gtk;
using SlimeSimulation.Configuration;
using SlimeSimulation.StdLibHelpers;
using System;

namespace SlimeSimulation.View.WindowComponent.SimulationConfigurationComponent
{
    public class FeedbackParameterControlComponent : HBox
    {
        private const string DescriptionString = "Feedback parameter for updating slime simulation at each step";
        private List<string> _errors;
        private readonly TextView _feedbackParamTextView;

        public FeedbackParameterControlComponent(double defaultFeedbackParam)
        {
            Label description = new Label(DescriptionString);
            TextView textView = new TextView();
            textView.Buffer.Text = defaultFeedbackParam.ToString();
            _feedbackParamTextView = textView;
            Add(description);
            Add(textView);
        }

        public double? ReadFeedbackParameter()
        {
            _errors = new List<string>();
            var feedbackParameter = _feedbackParamTextView.ExtractDoubleFromView();
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
