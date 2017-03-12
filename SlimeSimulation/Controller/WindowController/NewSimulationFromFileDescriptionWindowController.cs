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
        private NewSimulationFromFileDescriptionWindow _newSimulationFromFileDescriptionWindow;
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
            using (AbstractWindow = new NewSimulationFromFileDescriptionWindow("Slime simulation parameter selection", this))
            {
                _newSimulationFromFileDescriptionWindow = (NewSimulationFromFileDescriptionWindow)AbstractWindow;
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
                _newSimulationFromFileDescriptionWindow.ShowErrorMessage("Unable to find/load configuration due to an exception: " + e.Message);
                return;
            }
            Logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Application.Invoke(delegate
            {
                Logger.Debug("[StartSimulation] Invoking from main thread ");
                _newSimulationFromFileDescriptionWindow.Hide();
                controller.RunSimulation();
                controller = null; // aid gc ?
            });
        }

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            _newSimulationFromFileDescriptionWindow.Dispose();
            _applicationStartWindowController.Display();
        }
    }
}
