using NLog;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller
{
    public abstract class WindowController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected WindowTemplate window;
        protected MainController mainController;


        public WindowController(MainController mainController)
        {
            this.mainController = mainController;
        }

        public void OnQuit()
        {
            logger.Debug("[OnQuit] About to dispose of window: {0}", window);
            window.Dispose();
            logger.Debug("[OnQuit] Disposed of window.");
            mainController.DoNextSimulationStep();
        }

        public abstract void OnClick();
        public abstract void Render();
    }
}
