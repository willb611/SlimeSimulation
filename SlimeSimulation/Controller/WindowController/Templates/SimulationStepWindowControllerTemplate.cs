using NLog;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class SimulationStepWindowControllerTemplate : WindowControllerTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected SimulationController SimulationController;

        public SimulationStepWindowControllerTemplate(SimulationController simulationController)
        {
            SimulationController = simulationController;
        }

        public void OnStepCompleted()
        {
            Logger.Debug("[OnStepCompleted] Entered");
            SimulationController.DoNextSimulationStep();
            SimulationController.UpdateDisplay();
            Logger.Debug("[OnStepCompleted] SimulationController.DoNextSimulationStep(); finished");
            base.OnWindowClose();
            Logger.Debug("[OnStepCompleted] base.OnWindowClose(); finished");
        }

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            SimulationController.FinishSimulation();
        }

        internal void RunNumberOfSteps(int numberOfSteps)
        {
            Window.Dispose();
            SimulationController.DoNextSimulationSteps(numberOfSteps);
            SimulationController.UpdateDisplay();
        }

        internal bool StepWithoutShowingFlowResult()
        {
            return !SimulationController.ShouldFlowResultsBeDisplayed;
        }

        public void DisableShowingFlowResults()
        {
            SimulationController.ShouldFlowResultsBeDisplayed = false;
        }

        public void RunStepsUntilSlimeHasFullyExplored()
        {
            Window.Dispose();
            Logger.Debug("[RunStepsUntilSlimeHasFullyExplored] Starting");
            var stepNumber = 0;
            while (!SimulationController.SimulationState.HasFinishedExpanding)
            {
                SimulationController.DoNextSimulationStep();
                Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Now completed {++stepNumber} steps");
            }
            Logger.Debug($"[RunStepsUntilSlimeHasFullyExplored] Finished in {stepNumber} steps, updating display");
            SimulationController.UpdateDisplay();
        }
    }
}
