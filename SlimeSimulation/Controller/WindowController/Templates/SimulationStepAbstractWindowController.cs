using NLog;
using SlimeSimulation.Configuration;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class SimulationStepAbstractWindowController : AbstractWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public readonly SimulationController SimulationController;
        public readonly SimulationControlInterfaceValues SimulationControlInterfaceValues;

        internal bool IsSlimeAllowedToDisconnect => SimulationController.IsSlimeAllowedToDisconnect;
        public double FlowUsedInAdaptingNetwork => SimulationController.FlowUsedWhenAdaptingNetwork;
        public double FeedbackUsedWhenAdaptingNetwork => SimulationController.FeedbackUsedWhenAdaptingNetwork;

        public int StepsCompletedSoFarInAdaptingSlime => SimulationController.StepsTakenInAdaptingState;
        public int StepsTakenForSlimeToExplore => SimulationController.StepsTakenForSlimeToExplore;

        public SimulationStepAbstractWindowController(SimulationController simulationController)
        {
            SimulationController = simulationController;
            SimulationControlInterfaceValues = simulationController.SimulationControlBoxConfig;
        }

        public void OnStepCompleted()
        {
            Logger.Debug("[OnStepCompleted] Entered");
            AbstractWindow.Dispose();
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
            Logger.Debug("[RunNumberOfSteps] Entered");
            AbstractWindow.Dispose();
            SimulationController.AsyncDoNextSimulationSteps(numberOfSteps);
            SimulationController.UpdateDisplay();
        }

        public void RunStepsUntilSlimeHasFullyExplored()
        {
            Logger.Debug("[RunStepsUntilSlimeHasFullyExplored] Entered");
            AbstractWindow.Dispose();
            SimulationController.AsyncRunStepsUntilSlimeHasFullyExplored();
            SimulationController.UpdateDisplay();
        }

        public void UpdateConfiguration(SimulationConfiguration simulationConfiguration)
        {
            SimulationController.Configuration = simulationConfiguration;
        }

        public void Hide()
        {
            AbstractWindow.Hide();
        }
    }
}
