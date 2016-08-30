using System;
using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class SimulationStepWindowControllerTemplate : WindowControllerTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected SimulationController SimulationController;

        public SimulationControlInterfaceValues SimulationControlInterfaceValues;
        public int StepsCompletedSoFarInAdaptingSlime => SimulationController.StepsTakenInAdaptingState;
        public int StepsTakenForSlimeToExplore => SimulationController.StepsTakenForSlimeToExplore;

        public SimulationStepWindowControllerTemplate(SimulationController simulationController)
        {
            SimulationController = simulationController;
            SimulationControlInterfaceValues = simulationController.SimulationControlBoxConfig;
        }


        public void OnStepCompleted()
        {
            Logger.Debug("[OnStepCompleted] Entered");
            Window.Dispose();
            SimulationController.DoNextSimulationStep();
            SimulationController.UpdateDisplay();
            Logger.Debug("[OnStepCompleted] SimulationController.DoNextSimulationStep(); finished");
            Logger.Debug("[OnStepCompleted] base.OnWindowClose(); finished");
        }

        internal bool IsSlimeAllowedToDisconnect()
        {
            return SimulationController.IsSlimeAllowedToDisconnect();
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

        public double FlowUsedInAdaptingNetwork()
        {
            return SimulationController.FlowUsedWhenAdaptingNetwork();
        }

        public double FeedbackUsedWhenAdaptingNetwork()
        {
            return SimulationController.FeedbackUsedWhenAdaptingNetwork();
        }
    }
}
