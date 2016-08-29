using System;
using Gtk;
using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class WindowControllerTemplate : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed = false;

        public WindowTemplate Window;

        public virtual void OnWindowClose()
        {
            Logger.Debug("[OnWindowClose] About to dispose of window: {0}", Window);
            Window.Dispose();
            Logger.Debug("[OnWindowClose] Disposed of window.");
        }
        
        public abstract void OnClickCallback(Widget widget, ButtonPressEventArgs args);
        public abstract void Render();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Logger.Debug("[Dispose] Overriden method called from within " + this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                if (Window != null)
                {
                    Window.Dispose();
                    Window = null;
                }
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
        ~WindowControllerTemplate()
        {
            Dispose(false);
        }
    }
}
