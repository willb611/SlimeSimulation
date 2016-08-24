using System;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.Controller.SimulationUpdaters;
using SlimeSimulation.Model.Generation;

namespace SlimeSimulation.Controller.WindowController
{
    public class ApplicationStartWindowController : Templates.WindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private ApplicationStartWindow _startingWindow;
        
        public override void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args)
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
                    _startingWindow = Window as ApplicationStartWindow;
                    Logger.Debug("[Render] Display with main view");
                    gtkLifecycleController.Display(Window);
                    Logger.Debug("[Render] Left main GTK loop ? ");
                }
                Logger.Debug("[Render] Finished");
            }
        }

        internal void StartSimulation(SimulationConfiguration config)
        {
            var flowCalculator = new FlowCalculator(new LupDecompositionSolver());
            ISlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator(config.GenerationConfig);
            var simulationUpdater = new SimulationUpdater(flowCalculator, new SlimeNetworkAdaptionCalculator(config.FeedbackParam));
            var initial = slimeNetworkGenerator.Generate();
            var controller = new SimulationController(this, config.FlowAmount, simulationUpdater, initial, initial);
            Logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Gtk.Application.Invoke(delegate
            {
                Logger.Debug("[StartSimulation] Invoking from main thread ");
                _startingWindow.Hide();
                controller.RunSimulation();
            });
        }

        public void FinishSimulation(SimulationController controller)
        {
            Logger.Info("[FinishSimulation] Finished");
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
    }
}
