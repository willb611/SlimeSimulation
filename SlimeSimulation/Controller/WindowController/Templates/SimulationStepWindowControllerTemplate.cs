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
            Window.Dispose();
            Logger.Debug("[OnStepCompleted] Entered");
            SimulationController.DoNextSimulationStepAsync();
            SimulationController.UpdateDisplay();
            Logger.Debug("[OnStepCompleted] SimulationController.DoNextSimulationStep(); finished");
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
            SimulationController.RunStepsUntilSlimeHasFullyExplored();
            SimulationController.UpdateDisplay();
        }
    }
}
