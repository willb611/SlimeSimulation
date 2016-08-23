using NLog;
using SlimeSimulation.Controller;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.Windows.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Windows
{
    abstract class GraphDrawingWindowTemplate : WindowTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected GraphDrawingArea GraphDrawingArea;
        public GraphDrawingWindowTemplate(string windowTitle, WindowController controller) : base(windowTitle, controller)
        {
        }
        
        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            else if (disposing)
            {
                base.Dispose(disposing);
                GraphDrawingArea.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
