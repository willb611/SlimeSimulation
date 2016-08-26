namespace SlimeSimulation.Configuration
{
    public class SimulationConfiguration
    {
        private static readonly double DefaultFlowAmount = 0.2;
        private static readonly double DefaultFeedbackParam = 1.1;

        public SimulationConfiguration() : this(new LatticeGraphWithFoodSourcesGenerationConfig(), DefaultFlowAmount, DefaultFeedbackParam)
        {
        }

        public SimulationConfiguration(LatticeGraphWithFoodSourcesGenerationConfig generationConfig,
            double flowAmount, double feedbackParam)
        {
            GenerationConfig = generationConfig;
            FlowAmount = flowAmount;
            FeedbackParam = feedbackParam;
        }

        public double FeedbackParam { get; private set; }
        public double FlowAmount { get; private set; }
        public LatticeGraphWithFoodSourcesGenerationConfig GenerationConfig { get; private set; }
    }
}
