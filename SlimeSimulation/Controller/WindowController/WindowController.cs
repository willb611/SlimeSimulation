using NLog;
using SlimeSimulation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller {
    public abstract class WindowController {
        protected WindowTemplate window;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnQuit() {
            logger.Debug("[OnQuit] About to dispose of window.");
            window.Dispose();
            logger.Debug("[OnQuit] Disposed of window.");
        }

        public abstract void OnClick();
        public abstract void Render();
    }
}
