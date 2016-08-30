using System;
using SlimeSimulation.FlowCalculation;

namespace SlimeSimulation.Model.Simulation
{
    public class SimulationState
    {
        public SlimeNetwork SlimeNetwork { get; private set; }
        public FlowResult FlowResult { get; private set; }
        public bool HasFinishedExpanding { get; private set; }
        public GraphWithFoodSources PossibleNetwork { get; internal set; }
        public int StepsTakenInExploringState { get; internal set; }
        public int StepsTakenInAdaptingState { get; internal set; }

        public SimulationState(SlimeNetwork network, bool hasFinishedExpanding, GraphWithFoodSources graphWithFoodSources) : this(network, hasFinishedExpanding, graphWithFoodSources, 0, 0)
        {
        }
        public SimulationState(SlimeNetwork network, bool hasFinishedExpanding,
            GraphWithFoodSources graphWithFoodSources,
            int stepsTakenInExploringState, int stepsTakenInAdaptingState)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }
            StepsTakenInAdaptingState = stepsTakenInAdaptingState;
            StepsTakenInExploringState = stepsTakenInExploringState;
            SlimeNetwork = network;
            HasFinishedExpanding = hasFinishedExpanding;
            PossibleNetwork = graphWithFoodSources;
        }

        public SimulationState(SlimeNetwork network, FlowResult flowResult,
            GraphWithFoodSources graphWithFoodSources,
            int stepsTakenInExploringState, int stepsTakenInAdaptingState) : this(network, true, graphWithFoodSources, stepsTakenInExploringState, stepsTakenInAdaptingState)
        {
            FlowResult = flowResult;
        }

        public override string ToString()
        {
            return base.ToString() + ", hash: " + GetHashCode();
        }
    }
}
