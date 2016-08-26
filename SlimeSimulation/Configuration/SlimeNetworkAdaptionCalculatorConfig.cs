using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Configuration
{
    public class SlimeNetworkAdaptionCalculatorConfig
    {
        private static readonly double DefaultFeedbackParam = 1.1;

        public SlimeNetworkAdaptionCalculatorConfig() : this(DefaultFeedbackParam)
        {
            
        }

        public SlimeNetworkAdaptionCalculatorConfig(double feedbackParam)
        {
            FeedbackParam = feedbackParam;
        }


        public double FeedbackParam { get; private set; }
    }
}
