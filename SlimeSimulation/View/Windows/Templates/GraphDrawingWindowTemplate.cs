using NLog;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.View.Windows
{
    abstract class GraphDrawingWindowTemplate : WindowTemplate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected GraphDrawingArea graphDrawingArea;
        public GraphDrawingWindowTemplate(string windowTitle, WindowController controller) : base(windowTitle, controller)
        {
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            else if (disposing)
            {
                base.Dispose(disposing);
                graphDrawingArea.Dispose();
            }
            disposed = true;
            logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
