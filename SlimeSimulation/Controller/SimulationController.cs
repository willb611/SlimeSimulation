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
using SlimeSimulation.StdLibHelpers;
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

        private readonly ItemLock<SimulationState> _protectedState = new ItemLock<SimulationState>();
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
            _protectedState.Lock();
            _protectedState.SetAndClearLock(new SimulationState(initial, initial.CoversGraph(graphWithFoodSources), graphWithFoodSources));
        }

        internal void Display(WindowTemplate window)
        {
            _gtkLifecycleController.Display(window);
        }

        private SimulationState GetSimulationState()
        {
            return _protectedState.Get();
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
                UpdateDisplayFromState(_protectedState.Get());
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
                DoNextSimulationStepAsync();
            }
        }

        public void RunStepsUntilSlimeHasFullyExplored()
        {
            var stepNumber = 0;
            while (!GetSimulationState().HasFinishedExpanding)
            {
                DoNextSimulationStepAsync();
                Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Now completed {++stepNumber} steps");
            }
            Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Finished in {stepNumber} steps");
        }

        public void DoNextSimulationStepAsync()
        {
            var state = _protectedState.Lock();
            Logger.Debug("[DoNextSimulationStep] Stepping");
            Task<SimulationState> nextState;
            if (!state.HasFinishedExpanding)
            {
                nextState = _simulationUpdater.ExpandSlime(state);
            }
            else if (ShouldFlowResultsBeDisplayed)
            {
                if (state.FlowResult == null)
                {
                    // This step is just calculating the flow through the network, it doesn't count as a step.
                    SimulationStepsCompleted--;
                }
                nextState = _simulationUpdater.CalculateFlowResultOrUpdateNetworkUsingFlowInState(state);
            }
            else
            {
                nextState = _simulationUpdater.CalculateFlowAndUpdateNetwork(state);
            }
            SimulationStepsCompleted++;
            UpdateControllerState(nextState);
        }

        private async void UpdateControllerState(Task<SimulationState> stateParam)
        {
            _protectedState.SetAndClearLock(await stateParam);
        }

        public void UpdateDisplay()
        {
            Application.Invoke(delegate
            {
                Logger.Debug("[UpdateControllerState] Invoking from main thread ");
                UpdateDisplayFromState(_protectedState.Get());
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
