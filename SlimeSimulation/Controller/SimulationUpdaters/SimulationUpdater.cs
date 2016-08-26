using System;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class SimulationUpdater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly FlowCalculator _flowCalculator;
        private readonly SlimeNetworkAdaptionCalculator _slimeNetworkAdapterCalculator;
        private readonly int _flowAmount;
        private readonly SlimeNetworkExplorer _slimeNetworkExplorer;

        public SimulationUpdater() 
            : this(new FlowCalculator(), new SlimeNetworkAdaptionCalculator(), 1)
        {
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            int flowAmount)
            : this(flowCalculator, slimeNetworkAdapterCalculator, flowAmount, new SlimeNetworkExplorer())
        {
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            int flowAmount, SlimeNetworkExplorer slimeNetworkExplorer)
        {
            _flowCalculator = flowCalculator;
            _slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
            _flowAmount = flowAmount;
            _slimeNetworkExplorer = slimeNetworkExplorer;
        }


        internal Task<SimulationState> CalculateFlowAndUpdateNetwork(SimulationState state)
        {
            return Task.Run(() =>
            {
                var stateWithFlow = GetStateWithFlow(state.SlimeNetwork, _flowAmount, state.PossibleNetwork);
                if (stateWithFlow.FlowResult != null)
                {
                    return GetNextStateWithUpdatedConductivites(stateWithFlow.SlimeNetwork, stateWithFlow.FlowResult,
                        state.PossibleNetwork);
                }
                else
                {
                    return stateWithFlow;
                }
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
                Logger.Info(
                    "[CalculateFlowResultOrUpdateNetworkUsingFlowInState] Updating simulation based on flow result pre-existing");
                return GetNextStateWithUpdatedConductivites(state.SlimeNetwork, state.FlowResult, state.PossibleNetwork);
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
                return new SimulationState(slimeNetwork, true, graphWithFoodSources);
            }
        }

        private FlowResult GetFlow(SlimeNetwork network, int flow)
        {
            Node source = network.FoodSources.PickRandom();
            Node sink = network.FoodSources.PickRandom();
            while (InvalidSourceSink(source, sink, network))
            {
                sink = network.FoodSources.PickRandom();
            }
            return GetFlow(network, flow, source, sink);
        }

        private bool InvalidSourceSink(Node source, Node sink, SlimeNetwork network)
        {
            if (source == sink)
            {
                return true;
            }
            else
            {
                return !network.RouteExistsBetween(source, sink);
            }
        }

        private FlowResult GetFlow(SlimeNetwork network, int flow, Node source, Node sink)
        {
            var flowResult = _flowCalculator.CalculateFlow(network,
              source, sink, flow);
            return flowResult;
        }

        internal Task<SimulationState> ExpandSlime(SimulationState state)
        {
            return Task.Run(() =>
            {
                var expandedNetwork = _slimeNetworkExplorer.ExpandSlimeInNetwork(state.SlimeNetwork, state.PossibleNetwork);
                var hasFinishedExpanding = state.HasFinishedExpanding || state.SlimeNetwork.CoversGraph(state.PossibleNetwork);
                return new SimulationState(expandedNetwork, hasFinishedExpanding, state.PossibleNetwork);
            });
        }
    }
}
