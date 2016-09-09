using NLog;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Algorithms.LinearEquations;
using SlimeSimulation.Algorithms.RouteSelection;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class NonAsyncSimulationUpdater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly FlowCalculator _flowCalculator;
        private readonly SlimeNetworkAdaptionCalculator _slimeNetworkAdapterCalculator;
        private readonly double _flowAmount;
        private readonly SlimeNetworkExplorer _slimeNetworkExplorer;
        private readonly ItemLock<IRouteSelector> _routeSelectorLock;

        public double FlowUsedWhenAdaptingNetwork => _flowAmount;

        public NonAsyncSimulationUpdater(FlowCalculator flowCalculator, double flowAmount,
            SlimeNetworkAdaptionCalculator slimeNetworkAdapterCalculator, SlimeNetworkExplorer slimeNetworkExplorer)
        {
            _flowCalculator = flowCalculator;
            _slimeNetworkAdapterCalculator = slimeNetworkAdapterCalculator;
            _flowAmount = flowAmount;
            _slimeNetworkExplorer = slimeNetworkExplorer;
            _routeSelectorLock = new ItemLock<IRouteSelector>(new EnumerateSubgraphsRouteSelector());
        }
        
        public SimulationState GetNextStateWithUpdatedConductivites(SimulationState state)
        {
            var nextNetwork = _slimeNetworkAdapterCalculator.CalculateNextStep(state.SlimeNetwork, state.FlowResult);
            return new SimulationState(nextNetwork, true, state.GraphWithFoodSources, state.StepsTakenInExploringState, state.StepsTakenInAdaptingState + 1);
        }

        public SimulationState GetStateWithFlow(SimulationState state)
        {
            var slime = state.SlimeNetwork;
            try
            {
                var flowResult = GetFlow(slime);
                return new SimulationState(slime, flowResult, state.GraphWithFoodSources, state.StepsTakenInExploringState, state.StepsTakenInAdaptingState);
            }
            catch (SingularMatrixException e)
            {
                Logger.Error(e);
                return state;
            }
        }

        private FlowResult GetFlow(SlimeNetwork network)
        {
            var routeSelector = _routeSelectorLock.Lock();
            try
            {
                var route = routeSelector.SelectRoute(network);
                return GetFlowForRouteInNetwork(route, network);
            }
            finally
            {
                _routeSelectorLock.ClearLock();
            }
        }

        public FlowResult GetFlowForRouteInNetwork(Route route, SlimeNetwork network)
        {
            return _flowCalculator.CalculateFlow(network, route, _flowAmount);
        }

        public SimulationState ExpandSlime(SimulationState state)
        {
            Logger.Debug("[TaskExpandSlime] Starting");
            var expandedNetwork = _slimeNetworkExplorer.ExpandSlimeInNetwork(state.SlimeNetwork, state.GraphWithFoodSources);
            var hasFinishedExpanding = state.HasFinishedExpanding || state.SlimeNetwork.CoversGraph(state.GraphWithFoodSources);
            return new SimulationState(expandedNetwork, hasFinishedExpanding, state.GraphWithFoodSources, state.StepsTakenInExploringState + 1, 0);
        }
    }
}
