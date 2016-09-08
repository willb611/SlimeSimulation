using System;
using Newtonsoft.Json;
using SlimeSimulation.Algorithms.FlowCalculation;

namespace SlimeSimulation.Model.Simulation
{
    public class SimulationState
    {
        public SlimeNetwork SlimeNetwork { get; private set; }
        public FlowResult FlowResult { get; private set; }
        public bool HasFinishedExpanding { get; private set; }
        public GraphWithFoodSources GraphWithFoodSources { get; internal set; }
        public SimulationStepsTaken SimulationStepsTaken { get; private set; }

        public int StepsTakenInExploringState
        {
            get { return SimulationStepsTaken.StepsTakenInExploringState; }
            internal set { SimulationStepsTaken.StepsTakenInExploringState = value; }
        }

        public int StepsTakenInAdaptingState
        {
            get { return SimulationStepsTaken.StepsTakenInAdaptingState; }
            internal set { SimulationStepsTaken.StepsTakenInAdaptingState = value; }
        }

        public SimulationState(SlimeNetwork slimeNetwork, bool hasFinishedExpanding, GraphWithFoodSources graphWithFoodSources) : this(slimeNetwork, hasFinishedExpanding, graphWithFoodSources, 0, 0)
        {
        }

        public SimulationState(SlimeNetwork slimeNetwork, FlowResult flowResult,
            GraphWithFoodSources graphWithFoodSources,
            int stepsTakenInExploringState, int stepsTakenInAdaptingState) : this(slimeNetwork, true, graphWithFoodSources, stepsTakenInExploringState, stepsTakenInAdaptingState)
        {
            FlowResult = flowResult;
        }

        [JsonConstructor]
        public SimulationState(SlimeNetwork slimeNetwork, FlowResult flowResult,
            GraphWithFoodSources graphWithFoodSources,
            int stepsTakenInExploringState, int stepsTakenInAdaptingState, bool hasFinishedExpanding) : this(slimeNetwork, flowResult, graphWithFoodSources,
                stepsTakenInExploringState, stepsTakenInAdaptingState)
        {
            HasFinishedExpanding = hasFinishedExpanding;
        }

        public SimulationState(SlimeNetwork slimeNetwork, bool hasFinishedExpanding,
            GraphWithFoodSources graphWithFoodSources,
            int stepsTakenInExploringState, int stepsTakenInAdaptingState)
        {
            if (slimeNetwork == null)
            {
                throw new ArgumentNullException(nameof(slimeNetwork));
            } else if (graphWithFoodSources == null)
            {
                throw new ArgumentNullException(nameof(graphWithFoodSources));
            }
            SimulationStepsTaken =
                new SimulationStepsTaken.Builder().WithAdaptingSteps(stepsTakenInAdaptingState)
                    .WithExploringSteps(stepsTakenInExploringState)
                    .Build();
            SlimeNetwork = slimeNetwork;
            HasFinishedExpanding = hasFinishedExpanding;
            this.GraphWithFoodSources = graphWithFoodSources;
        }

        public override string ToString()
        {
            return base.ToString() + ", hash: " + GetHashCode();
        }
    }
}
