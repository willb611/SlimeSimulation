using System;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;
using System.Threading.Tasks;
using System.Linq;
using NLog;
using SlimeSimulation.StdLibHelpers;
using SlimeSimulation.LinearEquations;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SimulationUpdater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly FlowCalculator _flowCalculator;
        private readonly SlimeNetworkAdaptionCalculator _slimeNetworkAdapterCalculator;
        private readonly int _flowAmount;

        public SimulationUpdater() : this(new FlowCalculator(), new SlimeNetworkAdaptionCalculator(), 1)
        {
            
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            int flowAmount)
        {
            _flowCalculator = flowCalculator;
            _slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
            _flowAmount = flowAmount;
        }

        internal Task<SimulationState> CalculateFlowAndUpdateNetwork(SimulationState state)
        {
            return Task.Run(() =>
            {
                var stateWithFlow = GetStateWithFlow(state.SlimeNetwork, _flowAmount, state.PossibleNetwork);
                return GetNextStateWithUpdatedConductivites(stateWithFlow.SlimeNetwork, stateWithFlow.FlowResult, state.PossibleNetwork);
            });
        }

        internal Task<SimulationState> CalculateFlowResultOrUpdateNetworkUsingFlowInState(SimulationState state)
        {
            return Task.Run(() =>
            {
                if (state.FlowResult == null)
                {
                    Logger.Info("[CalculateFlowResultOrUpdateNetworkUsingFlowInState] Calculating flowResult");
                    return GetStateWithFlow(state.SlimeNetwork, _flowAmount, state.PossibleNetwork);
                }
                else
                {
                    Logger.Info(
                        "[CalculateFlowResultOrUpdateNetworkUsingFlowInState] Updating simulation based on flow result pre-existing");
                    return GetNextStateWithUpdatedConductivites(state.SlimeNetwork, state.FlowResult, state.PossibleNetwork);
                }
            });
        }

        private SimulationState GetNextStateWithUpdatedConductivites(SlimeNetwork slimeNetwork, FlowResult flowResult, GraphWithFoodSources graphWithFoodSources)
        {
            var nextNetwork = _slimeNetworkAdapterCalculator.CalculateNextStep(slimeNetwork, flowResult);
            return new SimulationState(nextNetwork, true, graphWithFoodSources);
        }

        private SimulationState GetStateWithFlow(SlimeNetwork slimeNetwork, int flowAmount, GraphWithFoodSources graphWithFoodSources)
        {
            try
            {
                var flowResult = GetFlow(slimeNetwork, flowAmount);
                return new SimulationState(slimeNetwork, flowResult, graphWithFoodSources);
            }
            catch (SingularMatrixException e)
            {
                Logger.Error(e);
                throw;
            }
        }

        private FlowResult GetFlow(SlimeNetwork network, int flow)
        {
            Node source = network.FoodSources.PickRandom();
            Node sink = network.FoodSources.PickRandom();
            while (sink == source)
            {
                sink = network.FoodSources.PickRandom();
            }
            return GetFlow(network, flow, source, sink);
        }

        private FlowResult GetFlow(SlimeNetwork network, int flow, Node source, Node sink)
        {
            var flowResult = _flowCalculator.CalculateFlow(network,
              source, sink, flow);
            return flowResult;
        }

        internal Task<SimulationState> ExpandSlime(SimulationState state)
        {
            throw new NotImplementedException();
        }

    }
}
