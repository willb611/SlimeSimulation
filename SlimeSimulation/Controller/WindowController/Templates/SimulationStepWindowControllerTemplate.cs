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
        internal bool StepWithoutShowingFlowResult => !SimulationController.ShouldFlowResultsBeDisplayed;

        internal bool IsSlimeAllowedToDisconnect => SimulationController.IsSlimeAllowedToDisconnect;
        public double FlowUsedInAdaptingNetwork => SimulationController.FlowUsedWhenAdaptingNetwork;
        public double FeedbackUsedWhenAdaptingNetwork => SimulationController.FeedbackUsedWhenAdaptingNetwork;

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
