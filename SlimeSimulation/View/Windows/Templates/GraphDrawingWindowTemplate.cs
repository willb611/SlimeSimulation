using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.View.Windows.Templates
{
    public abstract class GraphDrawingWindowTemplate : WindowTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected GraphDrawingArea GraphDrawingArea;
        public GraphDrawingWindowTemplate(string windowTitle, WindowControllerTemplate windowController) : base(windowTitle, windowController)
        {
        }
        
        protected override void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                base.Dispose(disposing);
                GraphDrawingArea?.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
        ~GraphDrawingWindowTemplate()
        {
            Dispose(false);
        }
    }
}
