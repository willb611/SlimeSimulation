using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
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

        private const double DefaultNewSlimeEdgeConnectivity = 1;
        private readonly FlowCalculator _flowCalculator;
        private readonly SlimeNetworkAdaptionCalculator _slimeNetworkAdapterCalculator;
        private readonly int _flowAmount;
        private readonly double _connectivityOfNewSlimeEdges;

        public SimulationUpdater() 
            : this(new FlowCalculator(), new SlimeNetworkAdaptionCalculator(), 1)
        {
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            int flowAmount)
            : this(flowCalculator, slimeNetworkAdapterCalculator, flowAmount, DefaultNewSlimeEdgeConnectivity)
        {
        }
        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator,
            int flowAmount, double connectivityOfNewSlimeEdges)
        {
            _flowCalculator = flowCalculator;
            _slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
            _flowAmount = flowAmount;
            _connectivityOfNewSlimeEdges = connectivityOfNewSlimeEdges;
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
            return Task.Run(() =>
            {
                var expandedNetwork = ActuallyExpandSlime(state.SlimeNetwork, state.PossibleNetwork);
                var hasFinishedExpanding = expandedNetwork.CoversGraph(state.PossibleNetwork);
                return new SimulationState(expandedNetwork, hasFinishedExpanding, state.PossibleNetwork);
            });
        }

        public SlimeNetwork ActuallyExpandSlime(SlimeNetwork slimeNetwork, GraphWithFoodSources graph)
        {
            var edgesToBeCoveredWithSlime = new HashSet<Edge>();
            var slimeEdges = new HashSet<SlimeEdge>(slimeNetwork.Edges);
            foreach (var slimeNode in slimeNetwork.Nodes)
            {
                foreach (var edge in graph.EdgesConnectedToNode(slimeNode))
                {
                    edgesToBeCoveredWithSlime.Add(edge);
                }
            }
            foreach (var slimeEdge in slimeEdges)
            {
                if (edgesToBeCoveredWithSlime.Contains(slimeEdge.Edge))
                {
                    edgesToBeCoveredWithSlime.Remove(slimeEdge.Edge);
                }
            }
            foreach (var unslimedEdge in edgesToBeCoveredWithSlime)
            {
                slimeEdges.Add(new SlimeEdge(unslimedEdge, _connectivityOfNewSlimeEdges));
            }
            return new SlimeNetwork(slimeEdges);
        }

    }
}
