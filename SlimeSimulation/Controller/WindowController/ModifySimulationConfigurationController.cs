using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class ModifySimulationConfigurationController : AbstractWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SimulationController _simulationController;

        public ModifySimulationConfigurationController(SimulationController simulationController)
        {
            _simulationController = simulationController;
        }

        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClick] Clicked!");
        }

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            _simulationController.UpdateDisplay();
        }

        public override void Render()
        {
            using (AbstractWindow = new ModifySimulationConfigurationWindow(this, _simulationController.Configuration))
            {
                GtkLifecycleController.Instance.Display(AbstractWindow);
            }
        }

        public void ContinueSimulationWithConfiguration(SimulationConfiguration simulationConfiguration)
        {
            _simulationController.Configuration = simulationConfiguration;
            Dispose();
            _simulationController.UpdateDisplay();
        }

        public void Display
            (string toString)
        {
            
        }

        public void DisplayError(string errorMessage)
        {
            _simulationController.DisplayError(errorMessage);
        }
    }
}
