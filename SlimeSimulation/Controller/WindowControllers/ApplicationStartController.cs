using System;
using SlimeSimulation.Controller;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows;
using NLog;
using SlimeSimulation.Configuration;
using SlimeSimulation.Model;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.LinearEquations;
using SlimeSimulation.Controller.SimulationUpdaters;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller
{
    public class ApplicationStartController : WindowController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private ApplicationStartWindow startingWindow;
        
        public override void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args)
        {
            logger.Info("[OnClickCallback] Entered. Doing nothing");
        }

        public override void Render()
        {
            using (var mainView = new GtkLifecycleController())
            {
                using (window = new ApplicationStartWindow("Slime simulation parameter selection", this))
                {
                    logger.Debug("[Render] Made window");
                    startingWindow = window as ApplicationStartWindow;
                    logger.Debug("[Render] Display with main view");
                    mainView.Display(window);
                    logger.Debug("[Render] Start running application");
                    mainView.StartBeingAbleToDisplay();
                    logger.Debug("[Render] Left main GTK loop ? ");
                }
                logger.Debug("[Render] Finished");
            }
        }

        internal void StartSimulation(SimulationConfiguration config)
        {
            var flowCalculator = new FlowCalculator(new LupDecompositionSolver());
            SlimeNetworkGenerator slimeNetworkGenerator = new LatticeSlimeNetworkGenerator(config.GenerationConfig);
            SimulationUpdater simulationUpdater = new SimulationUpdater(flowCalculator, new SlimeNetworkAdaptionCalculator(config.FeedbackParam));
            var initial = slimeNetworkGenerator.Generate();
            var controller = new SimulationController(this, config.FlowAmount, simulationUpdater, initial);
            logger.Info("[StartSimulation] Running simulation from user supplied parameters");
            Gtk.Application.Invoke(delegate
            {
                logger.Debug("[StartSimulation] Invoking from main thread ");
                startingWindow.Hide();
                controller.RunSimulation();
            });
        }

        public void FinishSimulation(SimulationController controller)
        {
            logger.Info("[FinishSimulation] Finished");
            startingWindow.Display();
        }

        public override void OnQuit()
        {
            base.OnQuit();
            DisposeOfView();
        }

        private void DisposeOfView()
        {
            logger.Debug("[DisposeOfView] Disposing of view..");
            startingWindow.Dispose();
        }
    }
}
