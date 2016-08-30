using System;
using System.Threading;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.StdLibHelpers;
using SlimeSimulation.View;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller
{
    public class SimulationController : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static bool _disposed = false;
        
        private readonly SimulationUpdater _simulationUpdater;
        private readonly GtkLifecycleController _gtkLifecycleController;

        private readonly ItemLock<SimulationState> _protectedState = new ItemLock<SimulationState>();
        private bool _shouldFlowResultsBeDisplayed = false;
        private WindowControllerTemplate _activeWindowController;

        private readonly ApplicationStartWindowController _applicationStartWindowController;
        private readonly SimulationConfiguration _config;

        public SimulationControlBoxConfig SimulationControlBoxConfig { get; } = new SimulationControlBoxConfig();
        public int SimulationStepsCompleted { get; internal set; }

        public SimulationController(ApplicationStartWindowController applicationStartWindowController, SimulationConfiguration config,
            GtkLifecycleController gtkLifecycleController, SimulationState initialState, SimulationUpdater simulationUpdater)
        {
            _config = config;
            _gtkLifecycleController = gtkLifecycleController;
            _applicationStartWindowController = applicationStartWindowController;
            _simulationUpdater = simulationUpdater;
            _protectedState.Lock();
            _protectedState.SetAndClearLock(initialState);
        }

        internal void Display(WindowTemplate window)
        {
            Logger.Debug("[Display] About to display {0}", window);
            _gtkLifecycleController.Display(window);
        }

        // Visible for testing
        internal SimulationState GetSimulationState()
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
            SimulationControlBoxConfig.NumberOfStepsToRun = numberOfSteps;
            Logger.Info($"[DoNextSimulationSteps] Running {numberOfSteps} steps");
            for (var stepsRunSoFar = 0; stepsRunSoFar < numberOfSteps; stepsRunSoFar++)
            {
                DoNextSimulationStep();
                Logger.Debug($"[DoNextSimulationSteps] Now completed {stepsRunSoFar} steps");
            }
            Logger.Info($"[DoNextSimulationSteps] Completed {numberOfSteps} steps");
        }

        public void RunStepsUntilSlimeHasFullyExplored()
        {
            var stepNumber = 0;
            while (!GetSimulationState().HasFinishedExpanding)
            {
                DoNextSimulationStep();
                Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Now completed {++stepNumber} steps");
            }
            Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Finished in {stepNumber} steps");
        }

        public void DoNextSimulationStep()
        {
            var state = _protectedState.Lock();
            Logger.Debug("[DoNextSimulationStep] Stepping");
            Task<SimulationState> nextState;
            if (!state.HasFinishedExpanding)
            {
                nextState = _simulationUpdater.TaskExpandSlime(state);
                SimulationStepsCompleted++;
            }
            else if (ShouldFlowResultsBeDisplayed)
            {
                if (state.FlowResult == null)
                {
                    nextState = _simulationUpdater.TaskCalculateFlow(state);
                }
                else
                {
                    nextState = _simulationUpdater.TaskUpdateNetworkUsingFlowInState(state);
                    SimulationStepsCompleted++;
                }
            }
            else
            {
                nextState = _simulationUpdater.TaskCalculateFlowAndUpdateNetwork(state);
                SimulationStepsCompleted++;
            }
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
                Logger.Debug("[UpdateDisplay] Invoking from main thread ");
                UpdateDisplayFromState(_protectedState.Get());
            });
        }

        private void UpdateDisplayFromState(SimulationState state)
        {
            Logger.Debug("[UpdateDisplayFromState] Entered");
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
            Logger.Debug($"[UpdateDisplayFromState] Found nextWindow controller {nextWindowController}");
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

        ~SimulationController()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Logger.Debug("[Dispose] Overriden method called from within " + this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _activeWindowController.Dispose();
                _activeWindowController = null;
                _protectedState.Lock();
                _protectedState.SetAndClearLock(null);
            }
            _disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        public bool IsSlimeAllowedToDisconnect()
        {
            return _config.ShouldAllowDisconnection;
        }

        public double FlowUsedWhenAdaptingNetwork()
        {
            return _simulationUpdater.FlowUsedWhenAdaptingNetwork();
        }

        public double FeedbackUsedWhenAdaptingNetwork()
        {
            return _simulationUpdater.FeedbackUsedWhenAdaptingNetwork();
        }
    }
}
