using System.Collections.Generic;
using Gtk;
using SlimeSimulation.Configuration;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationUpdateParameters
{
    public class FeedbackParameterControlComponent : HBox
    {
        private const string DescriptionString = "Feedback parameter for updating slime simulation at each step";
        private List<string> _errors;
        private readonly TextView _feedbackParamTextView;

        public FeedbackParameterControlComponent(SlimeNetworkAdaptionCalculatorConfig defaultAdaptorConfig)
        {
            Label description = new Label(DescriptionString);
            TextView textView = new TextView();
            textView.Buffer.Text = defaultAdaptorConfig.FeedbackParam.ToString();
            _feedbackParamTextView = textView;
            Add(description);
            Add(textView);
        }

        public SlimeNetworkAdaptionCalculatorConfig ReadConfiguration()
        {
            _errors = new List<string>();
            var feedbackParameter = _feedbackParamTextView.ExtractDoubleFromView();
            if (feedbackParameter.HasValue)
            {
                var adaptionConfig = new SlimeNetworkAdaptionCalculatorConfig(feedbackParameter.Value);
                return adaptionConfig;
            } else
            {
                _errors.Add("Invalid value for: " + DescriptionString);
                return null;
            }
        }

        internal List<string> Errors()
        {
            return _errors;
        }
    }
}
