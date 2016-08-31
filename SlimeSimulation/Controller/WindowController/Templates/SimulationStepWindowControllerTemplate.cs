using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class SimulationStepWindowControllerTemplate : WindowControllerTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public readonly SimulationController SimulationController;
        public readonly SimulationControlInterfaceValues SimulationControlInterfaceValues;

        internal bool IsSlimeAllowedToDisconnect => SimulationController.IsSlimeAllowedToDisconnect;
        public double FlowUsedInAdaptingNetwork => SimulationController.FlowUsedWhenAdaptingNetwork;
        public double FeedbackUsedWhenAdaptingNetwork => SimulationController.FeedbackUsedWhenAdaptingNetwork;

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
            SimulationController.AsyncDoNextSimulationStep();
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
            SimulationController.AsyncDoNextSimulationSteps(numberOfSteps);
            SimulationController.UpdateDisplay();
        }

        public void RunStepsUntilSlimeHasFullyExplored()
        {
            Window.Dispose();
            Logger.Debug("[RunStepsUntilSlimeHasFullyExplored] Starting");
            SimulationController.AsyncRunStepsUntilSlimeHasFullyExplored();
            SimulationController.UpdateDisplay();
        }
    }
}
