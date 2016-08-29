using System;
using System.Collections.Generic;
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
        private readonly double _flowAmount;
        private readonly SlimeNetworkExplorer _slimeNetworkExplorer;

        public SimulationUpdater() 
            : this(new FlowCalculator(), new SlimeNetworkAdaptionCalculator(), 1)
        {
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            double flowAmount)
            : this(flowCalculator, slimeNetworkAdapterCalculator, flowAmount, new SlimeNetworkExplorer())
        {
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            double flowAmount, SlimeNetworkExplorer slimeNetworkExplorer)
        {
            _flowCalculator = flowCalculator;
            _slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
            _flowAmount = flowAmount;
            _slimeNetworkExplorer = slimeNetworkExplorer;
        }

        public Task<SimulationState> TaskUpdateNetworkUsingFlowInState(SimulationState state)
        {
            return Task.Run(() =>
            {
                if (state.FlowResult == null)
                {
                    throw new ArgumentException("Given null flow in state");
                }
                else
                {
                    return GetNextStateWithUpdatedConductivites(state.SlimeNetwork, state.FlowResult, state.PossibleNetwork);
                }
            });
        }

        internal Task<SimulationState> TaskCalculateFlowAndUpdateNetwork(SimulationState state)
        {
            return Task.Run(() =>
            {
                var stateWithFlow = GetStateWithFlow(state);
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

        public Task<SimulationState> TaskCalculateFlow(SimulationState state)
        {
            return Task.Run(() => GetStateWithFlow(state));
        }

        private SimulationState GetNextStateWithUpdatedConductivites(SlimeNetwork slimeNetwork, FlowResult flowResult, GraphWithFoodSources graphWithFoodSources)
        {
            var nextNetwork = _slimeNetworkAdapterCalculator.CalculateNextStep(slimeNetwork, flowResult);
            return new SimulationState(nextNetwork, true, graphWithFoodSources);
        }

        private SimulationState GetStateWithFlow(SimulationState state)
        {
            return GetStateWithFlow(state.SlimeNetwork, _flowAmount, state.PossibleNetwork);
        }
        private SimulationState GetStateWithFlow(SlimeNetwork slimeNetwork, double flowAmount, GraphWithFoodSources graphWithFoodSources)
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

        private FlowResult GetFlow(SlimeNetwork network, double flow)
        {
            Node source = SelectSource(network);
            Node sink = SelectSink(network);
            int iterations = 0;
            while (InvalidSourceSink(source, sink, network))
            {
                source = SelectSource(network);
                sink = SelectSink(network);
                iterations++;
            }
            Logger.Info($"[GetFlow] Took {iterations} attempts to find a valid source and sink combination");
            return GetFlow(network, flow, source, sink);
        }

        private Node SelectSink(SlimeNetwork network)
        {
            return network.FoodSources.PickRandom();
        }
        
        private Node SelectSource(SlimeNetwork network)
        {
            return AdvanceAndGetFoodSourceEnumerator(network).Current;
        }
        private IEnumerator<FoodSourceNode> _foodSourceEnumerator;
        private IEnumerator<FoodSourceNode> AdvanceAndGetFoodSourceEnumerator(SlimeNetwork network)
        {
            while (_foodSourceEnumerator == null || !_foodSourceEnumerator.MoveNext())
            {
                _foodSourceEnumerator = network.FoodSources.GetEnumerator();
                Logger.Debug("[AdvanceAndGetFoodSourceEnumerator] Entered method");
            }
            return _foodSourceEnumerator;
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
        
        private FlowResult GetFlow(SlimeNetwork network, double flow, Node source, Node sink)
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
