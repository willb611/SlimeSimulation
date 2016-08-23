using NLog;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    abstract class SimulationStepWindowController : WindowController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected SimulationController SimulationController;

        public SimulationStepWindowController(SimulationController simulationController)
        {
            this.SimulationController = simulationController;
        }

        public override void OnQuit() // called when window is closed.
        {
            base.OnQuit();
            SimulationController.FinishSimulation();
        }

        public void OnStepCompleted()
        {
            _logger.Debug("[OnStepCompleted] Entered");
            SimulationController.DoNextSimulationStep();
            _logger.Debug("[OnStepCompleted] SimulationController.DoNextSimulationStep(); finished");
            base.OnQuit();
            _logger.Debug("[OnStepCompleted] base.OnQuit(); finished");
        }
    }
}
