using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.Factories;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class NewSimulationStarterWindowController : AbstractSimulationControllerStarter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationStartWindowController _applicationStartWindowController;
        private NewSimulationStarterWindow _newSimulationStarterWindow;
        private readonly SimulationControllerFactory _controllerFactory;

        public NewSimulationStarterWindowController(ApplicationStartWindowController applicationStartWindowController)
            : this(applicationStartWindowController, new SimulationControllerFactory())
        {
        }
        public NewSimulationStarterWindowController(ApplicationStartWindowController applicationStartWindowController, 
            SimulationControllerFactory simulationControllerFactory)
        {
            _controllerFactory = simulationControllerFactory;
            _applicationStartWindowController = applicationStartWindowController;
        }
        
        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Info("[OnClickCallback] Entered. Doing nothing");
        }
        
        public override void Render()
        {
            using (AbstractWindow = new NewSimulationStarterWindow("Slime simulation parameter selection", this))
            {
                _newSimulationStarterWindow = (NewSimulationStarterWindow) AbstractWindow;
                GtkLifecycleController.Instance.Display(AbstractWindow);
            }
        }

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

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            _newSimulationStarterWindow.Dispose();
            _applicationStartWindowController.Display();
        }
    }
}
