using System;
using System.Collections.Generic;
using Gtk;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.Model;
using SlimeSimulation.Model.Generation;
using SlimeSimulation.StdLibHelpers;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    public class ApplicationStartWindowController : WindowControllerTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static ApplicationStartWindowController Instance { get; private set; }

        public ApplicationStartWindowController()
        {
            Instance = this;
        }

        private ApplicationStartWindow _startingWindow;
        
        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Info("[OnClickCallback] Entered. Doing nothing");
        }
        
        public override void Render()
        {
            using (var gtkLifecycleController = new GtkLifecycleController())
            {
                using (Window = new ApplicationStartWindow("Slime simulation parameter selection", this))
                {
                    Logger.Debug("[Render] Made window");
                    _startingWindow = (ApplicationStartWindow) Window;
                    Logger.Debug("[Render] Display with main view");
                    gtkLifecycleController.Display(Window);
                    Logger.Debug("[Render] Left main GTK loop ? ");
                }
                Logger.Debug("[Render] Finished");
            }
        }

        internal void StartSimulation(SimulationConfiguration config)
        {
            var controller = new SimulationController(this, config, GtkLifecycleController.Instance);
            Logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Application.Invoke(delegate
            {
                Logger.Debug("[StartSimulation] Invoking from main thread ");
                _startingWindow.Hide();
                controller.RunSimulation();
                controller = null; // aid gc ?
            });
        }

        public void FinishSimulation(SimulationController controller)
        {
            controller.Dispose();
            Logger.Info("[FinishSimulation] Finished one simulation");
            _startingWindow.Display();
        }

        public override void OnWindowClose()
        {
            base.OnWindowClose();
            DisposeOfView();
            GtkLifecycleController.Instance.ApplicationQuit();
        }

        private void DisposeOfView()
        {
            Logger.Debug("[DisposeOfView] Disposing of view..");
            _startingWindow.Dispose();
        }

        public void DisplayError(string error)
        {
            Logger.Error(error);
            Application.Invoke(delegate
            {
                MessageDialog errorDialog = new MessageDialog(_startingWindow.Window, DialogFlags.DestroyWithParent,
                    MessageType.Error, ButtonsType.Ok,
                    "Unexpected error. Simulation tried to do a step when an step was in progress.")
                {
                    Title = "Unexpected error"
                };
                errorDialog.Run();
                errorDialog.Destroy();
            });
        }
    }
}
