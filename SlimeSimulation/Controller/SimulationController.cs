using System;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Simulation;
using SlimeSimulation.View;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller
{
    public class SimulationController : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static bool _disposed = false;
        
        private readonly AsyncSimulationUpdater _asyncSimulationUpdater;
        private readonly GtkLifecycleController _gtkLifecycleController;

        private readonly ItemLock<SimulationState> _protectedState;
        private AbstractWindowController _activeAbstractWindowController;

        private readonly AbstractSimulationControllerStarter _startingController;
        private SimulationConfiguration _config;

        public SimulationControlInterfaceValues SimulationControlBoxConfig { get; }
        public int StepsTakenInAdaptingState => GetSimulationState().StepsTakenInAdaptingState;
        public int StepsTakenForSlimeToExplore => GetSimulationState().StepsTakenInExploringState;

        public bool IsSlimeAllowedToDisconnect => _config.ShouldAllowDisconnection;
        public double FlowUsedWhenAdaptingNetwork => _asyncSimulationUpdater.FlowUsedWhenAdaptingNetwork;
        public double FeedbackUsedWhenAdaptingNetwork => _asyncSimulationUpdater.FeedbackUsedWhenAdaptingNetwork;

        private readonly SimulationSavingController _simulationSavingController;
        public string LastAttemptedSaveLocation => _simulationSavingController.LastAttemptedSaveLocation;

        public SimulationConfiguration Configuration
        {
            get { return _config; }
            set { _config = value; }
        }

        public bool ShouldFlowResultsBeDisplayed
        {
            get { return SimulationControlBoxConfig.ShouldFlowResultsBeDisplayed; }
            set { SimulationControlBoxConfig.ShouldFlowResultsBeDisplayed = value; }
        }


        public SimulationController(AbstractSimulationControllerStarter startingController,
            GtkLifecycleController gtkLifecycleController, AsyncSimulationUpdater simulationUpdater,
            SimulationSave simulationSave)
        {
            SimulationControlBoxConfig = simulationSave.SimulationControlInterfaceValues;
            _config = simulationSave.SimulationConfiguration;
            _gtkLifecycleController = gtkLifecycleController;
            _startingController = startingController;
            _asyncSimulationUpdater = simulationUpdater;
            _protectedState = new ItemLock<SimulationState>(simulationSave.SimulationState);
            _simulationSavingController = new SimulationSavingController();
        }

        internal void Display(AbstractWindow abstractWindow)
        {
            Logger.Debug("[Display] About to display {0}", abstractWindow);
            _gtkLifecycleController.Display(abstractWindow);
        }

        // Visible for testing
        internal SimulationState GetSimulationState()
        {
            return _protectedState.Get();
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
                _startingController.DisplayError(error);
            }
        }

        internal void FinishSimulation()
        {
            _startingController.RegainControlFromFinishedSimulation(this);
        }

        internal void AsyncDoNextSimulationSteps(int numberOfSteps)
        {
            Logger.Info($"[AsyncDoNextSimulationSteps] Running {numberOfSteps} steps");
            for (var stepsRunSoFar = 0; stepsRunSoFar < numberOfSteps; stepsRunSoFar++)
            {
                AsyncDoNextSimulationStep();
                Logger.Info($"[DoNextSimulationSteps] Now completed {stepsRunSoFar}/{numberOfSteps} steps");
            }
            Logger.Info($"[AsyncDoNextSimulationSteps] Completed {numberOfSteps} steps");
        }

        internal void AsyncDoNextSimulationStepsSavingEveryNSteps(int numberOfSteps, int intervalOfStepsToSaveSimulationAt)
        {
            int stepsRunSoFar;
            for (stepsRunSoFar = 0; stepsRunSoFar < numberOfSteps; stepsRunSoFar += intervalOfStepsToSaveSimulationAt)
            {
                Logger.Debug("[AsyncDoNextSimulationStepsSavingEveryNSteps] So far ran {0} steps out of {1}",
                    numberOfSteps, stepsRunSoFar);
                AsyncDoNextSimulationSteps(intervalOfStepsToSaveSimulationAt);
                SaveSimulation();
            }
            if (stepsRunSoFar < numberOfSteps)
            {
                AsyncDoNextSimulationSteps(numberOfSteps - stepsRunSoFar);
            }
            UpdateDisplay();
        }

        public void AsyncRunStepsUntilSlimeHasFullyExplored()
        {
            int stepNumber;
            for (stepNumber = 0; !GetSimulationState().HasFinishedExpanding; stepNumber++)
            {
                AsyncDoNextSimulationStep();
                Logger.Debug($"[AsyncRunStepsUntilSlimeHasFullyExplored] Now started {stepNumber} steps");
            }
            Logger.Debug($"[AsyncRunStepsUntilSlimeHasFullyExplored] Started all {stepNumber} steps");
        }

        public void AsyncDoNextSimulationStep()
        {
            var state = _protectedState.Lock();
            Logger.Debug("[DoNextSimulationStep] Stepping");
            Task<SimulationState> nextState;
            if (!state.HasFinishedExpanding)
            {
                nextState = _asyncSimulationUpdater.TaskExpandSlime(state);
            }
            else if (ShouldFlowResultsBeDisplayed)
            {
                if (state.FlowResult == null)
                {
                    nextState = _asyncSimulationUpdater.TaskCalculateFlow(state);
                }
                else
                {
                    nextState = _asyncSimulationUpdater.TaskUpdateNetworkUsingFlowInState(state);
                }
            }
            else if (SimulationControlBoxConfig.ShouldStepFromAllSourcesAtOnce)
            {
                nextState = _asyncSimulationUpdater.TaskCalculateFlowFromAllSourcesAndUpdateNetwork(state);
            } else 
            {
                nextState = _asyncSimulationUpdater.TaskCalculateFlowAndUpdateNetwork(state);
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
            AbstractWindowController nextAbstractWindowController;
            if (!state.HasFinishedExpanding)
            {
                nextAbstractWindowController = DisplayControllerForNotFullyExpandedSlime(state.GraphWithFoodSources, state.SlimeNetwork);
            } else if (state.FlowResult == null || !ShouldFlowResultsBeDisplayed)
            {
                nextAbstractWindowController = DisplayControllerForNetworkConnectivity(state.SlimeNetwork, state.GraphWithFoodSources);
            }
            else
            {
                nextAbstractWindowController = DisplayControllerForFlowResult(state.FlowResult);
            }
            Logger.Debug($"[UpdateDisplayFromState] Found nextWindow controller {nextAbstractWindowController}");
            _activeAbstractWindowController?.Dispose();
            _activeAbstractWindowController = nextAbstractWindowController;
            nextAbstractWindowController.Render();
        }

        private AbstractWindowController DisplayControllerForNotFullyExpandedSlime(GraphWithFoodSources graphWithFoodSources, SlimeNetwork slimeNetwork)
        {
            return new SlimeNetworkWindowController(this, slimeNetwork, graphWithFoodSources,
                new SimulationGrowthPhaseControlBoxFactory());
        }

        private AbstractWindowController DisplayControllerForFlowResult(FlowResult flowResult)
        {
            return new FlowResultWindowController(this, flowResult);
        }

        private AbstractWindowController DisplayControllerForNetworkConnectivity(SlimeNetwork network, GraphWithFoodSources graphWithFoodSources)
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
                _activeAbstractWindowController.Dispose();
                _activeAbstractWindowController = null;
                _protectedState.Lock();
                _protectedState.SetAndClearLock(null);
            }
            _disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }

        public void DisplayError(string errorMessage)
        {
            _startingController.DisplayError(errorMessage);
        }

        public Exception SaveSimulation()
        {
            return _simulationSavingController.SaveSimulation(new SimulationSave(GetSimulationState(), SimulationControlBoxConfig, Configuration));
        }
    }
}
