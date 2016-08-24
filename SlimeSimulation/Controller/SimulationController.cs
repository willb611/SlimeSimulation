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
using SlimeSimulation.View.Factories;

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

        private readonly ApplicationStartWindowController _applicationStartWindowController;

        public int SimulationStepsCompleted { get; internal set; }
        public bool ShouldFlowResultsBeDisplayed { get; private set; }

        public SimulationController(ApplicationStartWindowController startingWindowController,
            int flowAmount, SimulationUpdater simulationUpdater, SlimeNetwork initial,
            GraphWithFoodSources graphWithFoodSources)
        {
            this._applicationStartWindowController = startingWindowController;
            this._flowAmount = flowAmount;
            this._simulationUpdater = simulationUpdater;
            _state = new SimulationState(initial, graphWithFoodSources.Equals(initial));
            ShouldFlowResultsBeDisplayed = true;
        }

        public void RunSimulation()
        {
            try
            {
                UpdateDisplayFromState(_state);
            }
            catch (Exception e)
            {
				Logger.Fatal(e, "[RunSimulation] Unexpected exception: " + e, e.Data);
            }
        }

        internal void FinishSimulation()
        {
            _applicationStartWindowController.FinishSimulation(this);
        }


        internal void ToggleAreFlowResultsDisplayed(bool shouldResultsBeDisplayed)
        {
            ShouldFlowResultsBeDisplayed = shouldResultsBeDisplayed;
        }

        internal void DoNextSimulationSteps(int numberOfSteps)
        {
            for (var stepsRunSoFar = 0; stepsRunSoFar < numberOfSteps; stepsRunSoFar++)
            {
                DoNextSimulationStep();
            }
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
                if (!_state.HasFinishedExpanding)
                {
                    nextState = _simulationUpdater.ExpandSlime(_state);
                } else if (ShouldFlowResultsBeDisplayed)
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
                _simulationDoingStep = false;
            }
            catch (Exception e)
            {
                Logger.Error(e, "[UpdateControllerState] Error: ");
            }
        }

        public void UpdateDisplay()
        {
            Gtk.Application.Invoke(delegate
            {
                Logger.Debug("[UpdateControllerState] Invoking from main thread ");
                UpdateDisplayFromState(_state);
            });
        }

        private void UpdateDisplayFromState(SimulationState state)
        {
            if (!state.HasFinishedExpanding)
            {
                DisplayNotFullyExpandedSlime(state.SlimeNetwork);
            } else if (state.FlowResult == null || !ShouldFlowResultsBeDisplayed)
            {
                DisplayConnectivityInNetwork(state.SlimeNetwork);
            }
            else
            {
                DisplayFlowResult(state.FlowResult);
            }
        }

        private void DisplayNotFullyExpandedSlime(SlimeNetwork slimeNetwork)
        {
            throw new NotImplementedException();
        }

        private void DisplayFlowResult(FlowResult flowResult)
        {
            new FlowResultWindowController(this, _gtkLifecycleController, flowResult).Render();
        }

        private void DisplayConnectivityInNetwork(SlimeNetwork network)
        {
            new SlimeNetworkWindowController(this, _gtkLifecycleController, network.Edges, new SimulationAdaptionPhaseControlBoxFactory()).Render();
        }
    }
}
