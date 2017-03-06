using Newtonsoft.Json;

namespace SlimeSimulation.Configuration
{
    public class SlimeNetworkAdaptionCalculatorConfig
    {
        private static readonly double DefaultFeedbackParam = 1.1;
        private static readonly double DefaultTimePerSimulationStep = 0.5;

        public SlimeNetworkAdaptionCalculatorConfig() : this(DefaultFeedbackParam, DefaultTimePerSimulationStep)
        {
            
        }

        [JsonConstructor]
        public SlimeNetworkAdaptionCalculatorConfig(double feedbackParam, double timePerSimulationStep)
        {
            FeedbackParam = feedbackParam;
            TimePerSimulationStep = timePerSimulationStep;
        }


        public double FeedbackParam { get; private set; }
        public double TimePerSimulationStep { get; private set; }
    }
}
