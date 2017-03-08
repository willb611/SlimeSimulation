using Gtk;
using NLog;
using SlimeSimulation.Controller.Factories;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class NewSimulationFromFileDescriptionWindowController : AbstractSimulationControllerStarter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationStartWindowController _applicationStartWindowController;
        private LoadFromDescriptionWindow _loadFromDescriptionWindow;
        private readonly SimulationControllerFactory _controllerFactory;

        public NewSimulationFromFileDescriptionWindowController(ApplicationStartWindowController applicationStartWindowController)
        {
            _applicationStartWindowController = applicationStartWindowController;
        }

        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Info("[OnClickCallback] Entered!");
        }

        public override void Render()
        {
            using (AbstractWindow = new LoadFromDescriptionWindow("Slime simulation parameter selection", this))
            {
                _loadFromDescriptionWindow = (LoadFromDescriptionWindow)AbstractWindow;
                GtkLifecycleController.Instance.Display(AbstractWindow);
            }
        }
    /*
        internal void StartSimulation(SimulationConfiguration config)
        {
            var controller = _controllerFactory.MakeSimulationController(this, config);
            Logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Application.Invoke(delegate
            {
                Logger.Debug("[StartSimulation] Invoking from main thread ");
                _newSimulationStarterWindow.Hide();
                controller.RunSimulation();
                controller = null; // aid gc ?
            });
        }
        */
        public override void OnWindowClose()
        {
            base.OnWindowClose();
            _loadFromDescriptionWindow.Dispose();
            _applicationStartWindowController.Display();
        }
    }
}
