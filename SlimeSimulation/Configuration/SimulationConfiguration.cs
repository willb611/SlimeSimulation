namespace SlimeSimulation.Configuration
{
    public class SimulationConfiguration
    {
        private static readonly double DefaultFlowAmount = 0.2;
        private static readonly bool DefaultShouldAllowDisconnection = true;

        public SimulationConfiguration() : this(new LatticeGraphWithFoodSourcesGenerationConfig(), DefaultFlowAmount, new SlimeNetworkAdaptionCalculatorConfig())
        {
        }

        public SimulationConfiguration(LatticeGraphWithFoodSourcesGenerationConfig generationConfig,
            double flowAmount, SlimeNetworkAdaptionCalculatorConfig slimeNetworkAdaptionCalculatorConfig) : this(generationConfig, flowAmount,
                slimeNetworkAdaptionCalculatorConfig, DefaultShouldAllowDisconnection)
        {
        }

        public SimulationConfiguration(LatticeGraphWithFoodSourcesGenerationConfig generationConfig,
            double flowAmount, SlimeNetworkAdaptionCalculatorConfig slimeNetworkAdaptionCalculatorConfig, bool shouldAllowDisconnection)
        {
            ShouldAllowDisconnection = shouldAllowDisconnection;
            GenerationConfig = generationConfig;
            FlowAmount = flowAmount;
            SlimeNetworkAdaptionCalculatorConfig = slimeNetworkAdaptionCalculatorConfig;
        }

        public SlimeNetworkAdaptionCalculatorConfig SlimeNetworkAdaptionCalculatorConfig { get; private set; }
        public double FlowAmount { get; private set; }
        public bool ShouldAllowDisconnection { get; private set; }
        public LatticeGraphWithFoodSourcesGenerationConfig GenerationConfig { get; private set; }
    }
}
