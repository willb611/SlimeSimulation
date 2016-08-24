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

        public SimulationUpdater(FlowCalculator flowCalculator, SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator)
        {
            this._flowCalculator = flowCalculator;
            this._slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
        }

        internal Task<SimulationState> CalculateFlowAndUpdateNetwork(SimulationState state, int flowAmount)
        {
            return Task<SimulationState>.Run(() =>
            {
                var stateWithFlow = GetStateWithFlow(state.SlimeNetwork, flowAmount);
                return GetNextStateWithUpdatedConductivites(stateWithFlow.SlimeNetwork, stateWithFlow.FlowResult);
            });
        }

        internal Task<SimulationState> CalculateFlowResultOrUpdateNetworkUsingFlowInState(SimulationState state,
            int flowAmount)
        {
            return Task<SimulationState>.Run(() =>
            {
                if (state.FlowResult == null)
                {
                    Logger.Info("[CalculateFlowResultOrUpdateNetworkUsingFlowInState] Calculating flowResult");
                    return GetStateWithFlow(state.SlimeNetwork, flowAmount);
                }
                else
                {
                    Logger.Info(
                        "[CalculateFlowResultOrUpdateNetworkUsingFlowInState] Updating simulation based on flow result pre-existing");
                    return GetNextStateWithUpdatedConductivites(state.SlimeNetwork, state.FlowResult);
                }
            });
        }

        private SimulationState GetNextStateWithUpdatedConductivites(SlimeNetwork slimeNetwork, FlowResult flowResult)
        {
            var nextNetwork = _slimeNetworkAdapterCalculator.CalculateNextStep(slimeNetwork, flowResult);
            return new SimulationState(nextNetwork);
        }

        private SimulationState GetStateWithFlow(SlimeNetwork slimeNetwork, int flowAmount)
        {
            try
            {
                var flowResult = GetFlow(slimeNetwork, flowAmount);
                return new SimulationState(slimeNetwork, flowResult);
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
    }
}
