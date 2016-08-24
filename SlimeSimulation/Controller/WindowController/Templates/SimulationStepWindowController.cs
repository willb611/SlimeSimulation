using System;
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

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            SimulationController.FinishSimulation();
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

        internal void RunNumberOfSteps(int numberOfSteps)
        {
            base.OnWindowClose();
            SimulationController.DoNextSimulationSteps(numberOfSteps);
            SimulationController.UpdateDisplay();
        }

        internal bool StepWithoutShowingFlowResult()
        {
            return !SimulationController.ShouldFlowResultsBeDisplayed;
        }
    }
}
