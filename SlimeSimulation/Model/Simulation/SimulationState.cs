using SlimeSimulation.FlowCalculation;
using System;

namespace SlimeSimulation.Model.Simulation
{
    public class SimulationState
    {
        public SlimeNetwork SlimeNetwork { get; private set; }
        public FlowResult FlowResult { get; private set; }
        public bool HasFinishedExpanding { get; private set; }
        public GraphWithFoodSources PossibleNetwork { get; internal set; }

        public SimulationState(SlimeNetwork network, bool hasFinishedExpanding, GraphWithFoodSources graphWithFoodSources)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }
            SlimeNetwork = network;
            HasFinishedExpanding = hasFinishedExpanding;
            PossibleNetwork = graphWithFoodSources;
        }

        public SimulationState(SlimeNetwork network, FlowResult flowResult, GraphWithFoodSources graphWithFoodSources) : this(network, true, graphWithFoodSources)
        {
            FlowResult = flowResult;
        }

        public override string ToString()
        {
            return base.ToString() + ", hash: " + GetHashCode();
        }
    }
}
