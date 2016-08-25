using NLog;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class SimulationStepWindowController : WindowControllerTemplate
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected SimulationController SimulationController;

        public SimulationStepWindowController(SimulationController simulationController)
        {
            SimulationController = simulationController;
        }

        public void OnStepCompleted()
        {
            _logger.Debug("[OnStepCompleted] Entered");
            SimulationController.DoNextSimulationStep();
            SimulationController.UpdateDisplay();
            _logger.Debug("[OnStepCompleted] SimulationController.DoNextSimulationStep(); finished");
            base.OnWindowClose();
            _logger.Debug("[OnStepCompleted] base.OnWindowClose(); finished");
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
    }
}
