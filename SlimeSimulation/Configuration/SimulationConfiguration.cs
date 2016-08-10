using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Configuration
{
    public class SimulationConfiguration
    {
        private static readonly int DEFAULT_FLOW_AMOUNT = 2;
        private static readonly double DEFAULT_FEEDBACK_PARAM = 1;

        public SimulationConfiguration() : this(new LatticeSlimeNetworkGenerationConfig(), DEFAULT_FLOW_AMOUNT, DEFAULT_FEEDBACK_PARAM)
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
