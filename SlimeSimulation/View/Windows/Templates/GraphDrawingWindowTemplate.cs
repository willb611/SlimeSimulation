using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Windows.Templates
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
