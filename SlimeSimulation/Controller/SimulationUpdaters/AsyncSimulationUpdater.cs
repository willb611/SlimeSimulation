using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Algorithms.LinearEquations;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.StdLibHelpers;

namespace SlimeSimulation.Controller.SimulationUpdaters
{
    public class AsyncSimulationUpdater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly SlimeNetworkAdaptionCalculator _slimeNetworkAdapterCalculator;
        private readonly NonAsyncSimulationUpdater _nonAsyncSimulationUpdater;

        public double FlowUsedWhenAdaptingNetwork => _nonAsyncSimulationUpdater.FlowUsedWhenAdaptingNetwork;
        public double FeedbackUsedWhenAdaptingNetwork => _slimeNetworkAdapterCalculator.FeedbackUsedWhenUpdatingNetwork;

        public AsyncSimulationUpdater() 
            : this(new SimulationConfiguration())
        {
        }

        public AsyncSimulationUpdater(SimulationConfiguration simulationConfiguration)
        {
            _slimeNetworkAdapterCalculator = new SlimeNetworkAdaptionCalculator(simulationConfiguration.SlimeNetworkAdaptionCalculatorConfig);
            _nonAsyncSimulationUpdater = new NonAsyncSimulationUpdater(new FlowCalculator(new LupDecompositionSolver()), simulationConfiguration.FlowAmount,
                _slimeNetworkAdapterCalculator, new SlimeNetworkExplorer());
        }

        public virtual Task<SimulationState> TaskUpdateNetworkUsingFlowInState(SimulationState state)
        {
            return Task.Run(() =>
            {
                if (state.FlowResult == null)
                {
                    throw new ArgumentException("Given null flow in state");
                }
                else
                {
                    return _nonAsyncSimulationUpdater.GetNextStateWithUpdatedConductivites(state);
                }
            });
        }

        internal virtual Task<SimulationState> TaskCalculateFlowAndUpdateNetwork(SimulationState state)
        {
            return Task.Run(() =>
            {
                var stateWithFlow = _nonAsyncSimulationUpdater.GetStateWithFlow(state);
                if (stateWithFlow.FlowResult == null)
                {
                    throw new ArgumentException("Given null flow in state");
                }
                else
                {
                    return _nonAsyncSimulationUpdater.GetNextStateWithUpdatedConductivites(stateWithFlow);
                }
            });
        }

        public virtual Task<SimulationState> TaskCalculateFlow(SimulationState state)
        {
            return Task.Run(() => _nonAsyncSimulationUpdater.GetStateWithFlow(state));
        }

        private Task<FlowResult> TaskCalculateFlowForRoute(SimulationState state, Route route)
        {
            return Task.Run(() => _nonAsyncSimulationUpdater.GetFlowForRouteInNetwork(route, state.SlimeNetwork));
        }

        internal virtual Task<SimulationState> TaskExpandSlime(SimulationState state)
        {
            return Task.Run(() => _nonAsyncSimulationUpdater.ExpandSlime(state));
        }
        
        public Task<SimulationState> TaskCalculateFlowFromAllSourcesAndUpdateNetwork(SimulationState state)
        {
            return Task.Run((async () =>
            {
                var flowResults = new List<FlowResult>();
                var tasks = GetAsyncTasksToPushFlowFromEachSourceNodeInSlime(state);
                Logger.Info("[TaskCalculateFlowFromAllSourcesAndUpdateNetwork] Starting. Will await completion of {0} flow results", tasks.Count);
                foreach (var task in tasks)
                {
                    var flowResultFromTask = await task;
                    flowResults.Add(flowResultFromTask);
                }
                var updatedSlime = _slimeNetworkAdapterCalculator.CalculateNextStep(state.SlimeNetwork, flowResults);
                return new SimulationState(updatedSlime, true, state.GraphWithFoodSources, state.StepsTakenInExploringState, state.StepsTakenInAdaptingState + 1);
            }));
        }

        private List<Task<FlowResult>> GetAsyncTasksToPushFlowFromEachSourceNodeInSlime(SimulationState state)
        {
            var slime = state.SlimeNetwork;
            var enumerator = slime.FoodSources.GetEnumerator();
            var tasks = new List<Task<FlowResult>>();
            while (enumerator.MoveNext())
            {
                var source = enumerator.Current;
                var connectedPossibleSinks = Nodes.GetFoodSourcesConnectedToNodeInGraph(source, slime).ToList();
                if (connectedPossibleSinks.Any())
                {
                    var sink = connectedPossibleSinks.PickRandom();
                    tasks.Add(TaskCalculateFlowForRoute(state, new Route(source, sink)));
                }
                else
                {
                    Logger.Warn(
                        "[TaskCalculateFlowFromAllSourcesAndUpdateNetwork] Found no food sources connected to {0} in graph", source);
                }
            }
            enumerator.Dispose();
            return tasks;
        }
    }
}

