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
        public static NewSimulationStarterWindowController Instance { get; private set; }

        private NewSimulationStarterAbstractWindow _startingAbstractWindow;
        private readonly SimulationControllerFactory _controllerFactory;

        public NewSimulationStarterWindowController() : this(new SimulationControllerFactory())
        {
            Instance = this;
        }
        public NewSimulationStarterWindowController(SimulationControllerFactory simulationControllerFactory)
        {
            _controllerFactory = simulationControllerFactory;
        }
        
        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Info("[OnClickCallback] Entered. Doing nothing");
        }
        
        public override void Render()
        {
            using (var gtkLifecycleController = GtkLifecycleController.Instance)
            {
                using (AbstractWindow = new NewSimulationStarterAbstractWindow("Slime simulation parameter selection", this))
                {
                    _startingAbstractWindow = (NewSimulationStarterAbstractWindow) AbstractWindow;
                    gtkLifecycleController.Display(AbstractWindow);
                }
                Logger.Debug("[Render] Finished");
            }
        }

        internal void StartSimulation(SimulationConfiguration config)
        {
            var controller = _controllerFactory.MakeSimulationController(this, config);
            Logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Application.Invoke(delegate
            {
                Logger.Debug("[StartSimulation] Invoking from main thread ");
                _startingAbstractWindow.Hide();
                controller.RunSimulation();
                controller = null; // aid gc ?
            });
        }

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            _startingAbstractWindow.Dispose();
        }
    }
}
