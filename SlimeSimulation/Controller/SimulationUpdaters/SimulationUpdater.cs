using System;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;
using System.Threading.Tasks;
using System.Linq;
using NLog;
using SlimeSimulation.FlowCalculation.LinearEquations;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    internal class SimulationUpdater
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private FlowCalculator flowCalculator;
        private SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator;

        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator)
        {
            this.flowCalculator = flowCalculator;
            this.slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
        }

        internal Task<SimulationState> GetNextState(SimulationState state, int flowAmount)
        {
            if (state.FlowResult == null)
            {
                logger.Info("[GetNextState] Calculating flowResult");
                return GetStateWithFlow(state.SlimeNetwork, flowAmount);
            }
            else
            {
                logger.Info("[GetNextState] Updating simulation based on flow result pre-existing");
                return GetNextStateWithUpdatedConductivites(state.SlimeNetwork, state.FlowResult);
            }
        }

        private Task<SimulationState> GetNextStateWithUpdatedConductivites(SlimeNetwork slimeNetwork, FlowResult flowResult)
        {
            return Task<SimulationState>.Run(() =>
            {
                var nextNetwork = slimeNetworkAdapterCalculator.CalculateNextStep(slimeNetwork, flowResult);
                return new SimulationState(nextNetwork);
            });
        }

        private Task<SimulationState> GetStateWithFlow(SlimeNetwork slimeNetwork, int flowAmount)
        {
            return Task<SimulationState>.Run(() =>
            {
                var flowResult = GetFlow(slimeNetwork, flowAmount);
                return new SimulationState(slimeNetwork, flowResult);
            });
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
            var flowResult = flowCalculator.CalculateFlow(network.Edges, network.Nodes,
              source, sink, flow);
            return flowResult;
        }
    }
}
