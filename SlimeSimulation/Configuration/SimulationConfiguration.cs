namespace SlimeSimulation.Configuration
{
    public class SimulationConfiguration
    {
        private static readonly int DefaultFlowAmount = 2;
        private static readonly double DefaultFeedbackParam = 1;

        public SimulationConfiguration() : this(new LatticeGraphWithFoodSourcesGenerationConfig(), DefaultFlowAmount, DefaultFeedbackParam)
        {
        }

        public SimulationConfiguration(LatticeGraphWithFoodSourcesGenerationConfig generationConfig,
            int flowAmount, double feedbackParam)
        {
            GenerationConfig = generationConfig;
            FlowAmount = flowAmount;
            FeedbackParam = feedbackParam;
        }

        public double FeedbackParam { get; private set; }
        public int FlowAmount { get; private set; }
        public LatticeGraphWithFoodSourcesGenerationConfig GenerationConfig { get; private set; }
    }
}
