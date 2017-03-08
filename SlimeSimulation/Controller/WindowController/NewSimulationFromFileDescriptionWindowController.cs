using System;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
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
            : this(applicationStartWindowController, new SimulationControllerFactory())
        {
        }
        public NewSimulationFromFileDescriptionWindowController(ApplicationStartWindowController applicationStartWindowController,
            SimulationControllerFactory simulationControllerFactory)
        {
            _applicationStartWindowController = applicationStartWindowController;
            _controllerFactory = simulationControllerFactory;
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
        
        internal void StartSimulationWithDescriptionFromFile(SimulationConfiguration config)
        {
            SimulationController controller;
            try
            {
                controller = _controllerFactory.MakeSimulationController(this, config);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                _loadFromDescriptionWindow.DisplayError(e);
                return;
            }
            Logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Application.Invoke(delegate
            {
                Logger.Debug("[StartSimulation] Invoking from main thread ");
                _loadFromDescriptionWindow.Hide();
                controller.RunSimulation();
                controller = null; // aid gc ?
            });
        }

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            _loadFromDescriptionWindow.Dispose();
            _applicationStartWindowController.Display();
        }
    }
}
