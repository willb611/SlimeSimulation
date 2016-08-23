using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Configuration
{
    public class SimulationConfiguration
    {
        private static readonly int DefaultFlowAmount = 2;
        private static readonly double DefaultFeedbackParam = 1;

        public SimulationConfiguration() : this(new LatticeSlimeNetworkGenerationConfig(), DefaultFlowAmount, DefaultFeedbackParam)
        {
        }

        public SimulationConfiguration(LatticeSlimeNetworkGenerationConfig generationConfig,
            int flowAmount, double feedbackParam)
        {
            this.GenerationConfig = generationConfig;
            this.FlowAmount = flowAmount;
            this.FeedbackParam = feedbackParam;
        }

        public double FeedbackParam { get; private set; }
        public int FlowAmount { get; private set; }
        public LatticeSlimeNetworkGenerationConfig GenerationConfig { get; private set; }
    }
}
