using Gtk;
using SlimeSimulation.Configuration;
using SlimeSimulation.StdLibHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.WindowComponent.SimulationControlComponent.SimulationCreaterComponent
{
    public class FeedbackParameterControlComponent : HBox
    {
        private const string descriptionString = "Feedback parameter for updating slime simulation at each step";
        private List<string> _errors;
        private TextView _feedbackParamTextView;

        public FeedbackParameterControlComponent(SlimeNetworkAdaptionCalculatorConfig defaultAdaptorConfig)
        {
            Label description = new Label(descriptionString);
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
                _errors.Add(descriptionString);
                return null;
            } else
            {
                var adaptionConfig = new SlimeNetworkAdaptionCalculatorConfig(feedbackParameter.Value);
                return adaptionConfig;
            }
        }

        internal List<string> Errors()
        {
            return _errors;
        }
    }
}
