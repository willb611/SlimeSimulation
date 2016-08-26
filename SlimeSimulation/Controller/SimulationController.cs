using System;
using System.Threading;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.View;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller
{
    public class SimulationController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly SimulationUpdater _simulationUpdater;
        private readonly GtkLifecycleController _gtkLifecycleController = GtkLifecycleController.Instance;
        
        private bool _simulationDoingStep;
        private SimulationState _state;
        private bool _shouldFlowResultsBeDisplayed = false;
        private WindowControllerTemplate _activeWindowController;

        private readonly ApplicationStartWindowController _applicationStartWindowController;

        public int SimulationStepsCompleted { get; internal set; }

        public SimulationController(ApplicationStartWindowController startingWindowController,
            SimulationUpdater simulationUpdater, SlimeNetwork initial,
            GraphWithFoodSources graphWithFoodSources)
        {
            _applicationStartWindowController = startingWindowController;
            _simulationUpdater = simulationUpdater;
            _state = new SimulationState(initial, initial.CoversGraph(graphWithFoodSources), graphWithFoodSources);
        }

        public SimulationState SimulationState
        {
            get
            {
                while (_simulationDoingStep)
                {
                    Thread.Sleep(50);
                }
                return _state;
            }
        }

        internal void Display(WindowTemplate window)
        {
            _gtkLifecycleController.Display(window);
        }

        public bool ShouldFlowResultsBeDisplayed
        {
            get { return _shouldFlowResultsBeDisplayed; }
            set {
                _shouldFlowResultsBeDisplayed = value;
                var slimeNetworkWindowController = _activeWindowController as SlimeNetworkWindowController;
                slimeNetworkWindowController?.ReDraw();
            }
        }

        public void RunSimulation()
        {
            try
            {
                UpdateDisplayFromState(_state);
            }
            catch (Exception e)
            {
                var error = "[RunSimulation] Unexpected exception: " + e + e.Data;
                Logger.Fatal(error);
                _applicationStartWindowController.DisplayError(error);
            }
        }

        internal void FinishSimulation()
        {
            _applicationStartWindowController.FinishSimulation(this);
        }

        internal void DoNextSimulationSteps(int numberOfSteps)
        {
            for (var stepsRunSoFar = 0; stepsRunSoFar < numberOfSteps; stepsRunSoFar++)
            {
                Logger.Debug($"[DoNextSimulationSteps] Now completed {++stepsRunSoFar} steps");
                DoNextSimulationStep()?.Wait();
            }
        }
        public void RunStepsUntilSlimeHasFullyExplored()
        {
            var stepNumber = 0;
            while (!SimulationState.HasFinishedExpanding)
            {
                DoNextSimulationStep()?.Wait();
                Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Now completed {++stepNumber} steps");
            }
            Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Finished in {stepNumber} steps");
        }

        public Task<SimulationState> DoNextSimulationStep()
        {
            if (_simulationDoingStep)
            {
                var error = "[DoNextSimulationStep] Not starting next step as it's already in progress";
                Logger.Warn(error);
                _applicationStartWindowController.DisplayError(error);
                return null;
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
                    if (_state.FlowResult == null)
                    {
                        // This step is just calculating the flow through the network, it doesn't count as a step.
                        SimulationStepsCompleted--;
                    }
                    nextState = _simulationUpdater.CalculateFlowResultOrUpdateNetworkUsingFlowInState(_state);
                }
                else
                {
                    nextState = _simulationUpdater.CalculateFlowAndUpdateNetwork(_state);
                }
                SimulationStepsCompleted++;
                UpdateControllerState(nextState);
                return nextState;
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
            Application.Invoke(delegate
            {
                Logger.Debug("[UpdateControllerState] Invoking from main thread ");
                UpdateDisplayFromState(_state);
            });
        }

        private void UpdateDisplayFromState(SimulationState state)
        {
            WindowControllerTemplate nextWindowController;
            if (!state.HasFinishedExpanding)
            {
                nextWindowController = DisplayControllerForNotFullyExpandedSlime(state.PossibleNetwork, state.SlimeNetwork);
            } else if (state.FlowResult == null || !ShouldFlowResultsBeDisplayed)
            {
                nextWindowController = DisplayControllerForNetworkConnectivity(state.SlimeNetwork, state.PossibleNetwork);
            }
            else
            {
                nextWindowController = DisplayControllerForFlowResult(state.FlowResult);
            }
            _activeWindowController = nextWindowController;
            nextWindowController.Render();
        }

        private WindowControllerTemplate DisplayControllerForNotFullyExpandedSlime(GraphWithFoodSources graphWithFoodSources, SlimeNetwork slimeNetwork)
        {
            return new SlimeNetworkWindowController(this, slimeNetwork, graphWithFoodSources,
                new SimulationGrowthPhaseControlBoxFactory());
        }

        private WindowControllerTemplate DisplayControllerForFlowResult(FlowResult flowResult)
        {
            return new FlowResultWindowController(this, flowResult);
        }

        private WindowControllerTemplate DisplayControllerForNetworkConnectivity(SlimeNetwork network, GraphWithFoodSources graphWithFoodSources)
        {
            return new SlimeNetworkWindowController(this, network, graphWithFoodSources, new SimulationAdaptionPhaseControlBoxFactory());
        }
    }
}
