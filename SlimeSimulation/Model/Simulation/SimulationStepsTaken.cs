using System;
using Newtonsoft.Json;

namespace SlimeSimulation.Model.Simulation
{
    public class SimulationStepsTaken
    {
        public int StepsTakenInExploringState { get; internal set; }
        public int StepsTakenInAdaptingState { get; internal set; }

        [JsonConstructor]
        private SimulationStepsTaken(int stepsTakenInExploringState, int stepsTakenInAdaptingState)
        {
            StepsTakenInExploringState = stepsTakenInExploringState;
            StepsTakenInAdaptingState = stepsTakenInAdaptingState;
        }

        public class Builder
        {
            private int _stepsTakenInExploringState, _stepsTakenInAdaptingState;
            private bool _hasExploringSteps, _hasAdaptingSteps;

            public Builder WithAdaptingSteps(int stepsTakenInAdaptingState)
            {
                _stepsTakenInAdaptingState = stepsTakenInAdaptingState;
                _hasAdaptingSteps = true;
                return this;
            }

            public Builder WithExploringSteps(int stepsTakenInExploringState)
            {
                _stepsTakenInExploringState = stepsTakenInExploringState;
                _hasExploringSteps = true;
                return this;
            }

            public SimulationStepsTaken Build()
            {
                if (!_hasAdaptingSteps)
                {
                    throw new ApplicationException("Missing steps taken in adapting state");
                } else if (!_hasExploringSteps)
                {
                    throw new ApplicationException("Missing steps taken in exploring state");
                }
                else
                {
                    return new SimulationStepsTaken(_stepsTakenInExploringState, _stepsTakenInAdaptingState);
                }
            }
        }
    }
}
