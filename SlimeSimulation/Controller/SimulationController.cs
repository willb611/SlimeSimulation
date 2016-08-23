using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using System;
using System.Collections.Generic;
using NLog;
using SlimeSimulation.LinearEquations;
using System.Linq;
using System.Threading.Tasks;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.View;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation.Controller
{
    public class SimulationController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly int _flowAmount;
        private readonly SimulationUpdater _simulationUpdater;
        private readonly GtkLifecycleController _gtkLifecycleController = GtkLifecycleController.Instance;

        private bool _simulationDoingStep = false;
        private SimulationState _state;

        private readonly ApplicationStartController _startingController;

        public int SimulationStepsCompleted { get; internal set; }
        public bool ShouldFlowResultsBeDisplayed { get; private set; }

        public SimulationController(ApplicationStartController startingController,
            int flowAmount, SimulationUpdater simulationUpdater, SlimeNetwork initial)
        {
            this._startingController = startingController;
            this._flowAmount = flowAmount;
            this._simulationUpdater = simulationUpdater;
            _state = new SimulationState(initial);
            ShouldFlowResultsBeDisplayed = true;
        }

        public void RunSimulation()
        {
            try
            {
                Logger.Debug("[RunSimulation] Using before ");
                UpdateDisplayFromState(_state);
                Logger.Debug("[RunSimulation] Using after ");
            }
            catch (Exception e)
            {
				Logger.Fatal(e, "[RunSimulation] Unexpected exception: " + e, e.Data);
            }
        }

        internal void FinishSimulation()
        {
            _startingController.FinishSimulation(this);
        }


        internal void ToggleAreFlowResultsDisplayed(bool shouldResultsBeDisplayed)
        {
            ShouldFlowResultsBeDisplayed = shouldResultsBeDisplayed;
        }

        internal void DoNextSimulationSteps(int numberOfSteps)
        {
            _simulationDoingStep = true;
            Task<SimulationState> taskForState = null;
            var mostRecentState = _state;
            for (var stepsRunSoFar = 0; stepsRunSoFar < numberOfSteps; stepsRunSoFar++)
            {
                if (taskForState != null)
                {
                    taskForState.Wait();
                    mostRecentState = taskForState.Result;
                }
                taskForState = _simulationUpdater.CalculateFlowAndUpdateNetwork(mostRecentState, _flowAmount);
            }
            SimulationStepsCompleted += numberOfSteps;
            UpdateControllerState(taskForState);
        }

        public void DoNextSimulationStep()
        {
            if (_simulationDoingStep)
            {
                Logger.Debug("[DoNextSimulationStep] Not starting next step as it's already in progress");
            }
            else
            {
                Logger.Debug("[DoNextSimulationStep] Stepping");
                _simulationDoingStep = true;
                Task<SimulationState> nextState;
                if (ShouldFlowResultsBeDisplayed)
                {
                    if (_state.FlowResult != null)
                    {
                        SimulationStepsCompleted++;
                    }
                    nextState = _simulationUpdater.CalculateFlowResultOrUpdateNetworkUsingFlowInState(_state, _flowAmount);
                }
                else
                {
                    nextState = _simulationUpdater.CalculateFlowAndUpdateNetwork(_state, _flowAmount);
                    SimulationStepsCompleted++;
                }
                UpdateControllerState(nextState);
            }
        }

        private async void UpdateControllerState(Task<SimulationState> stateParam)
        {
            try
            {
                _state = await stateParam;
                Logger.Debug("[UpdateControllerState] State: {0} about to update view", _state);
                Gtk.Application.Invoke(delegate
                {
                    Logger.Debug("[UpdateControllerState] Invoking from main thread ");
                    UpdateDisplayFromState(_state);
                });
                _simulationDoingStep = false;
            }
            catch (Exception e)
            {
                Logger.Error(e, "[UpdateControllerState] Error: ");
            }
        }

        private void UpdateDisplayFromState(SimulationState state)
        {
            if (state.FlowResult == null || !ShouldFlowResultsBeDisplayed)
            {
                DisplayConnectivityInNetwork(state.SlimeNetwork);
            }
            else
            {
                DisplayFlowResult(state.FlowResult);
            }
        }

        private void DisplayFlowResult(FlowResult flowResult)
        {
            new FlowResultWindowController(this, _gtkLifecycleController, flowResult).Render();
        }

        private void DisplayConnectivityInNetwork(SlimeNetwork network)
        {
            new SlimeNetworkWindowController(this, _gtkLifecycleController, network.Edges).Render();
        }
    }
}
