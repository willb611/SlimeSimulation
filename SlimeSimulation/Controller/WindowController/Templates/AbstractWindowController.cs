using System;
using Gtk;
using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class AbstractWindowController : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed = false;

        public AbstractWindow AbstractWindow;

        public virtual void OnWindowClose()
        {
            Logger.Debug("[OnWindowClose] About to dispose of window: {0}", AbstractWindow);
            AbstractWindow.Dispose();
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
                if (AbstractWindow != null)
                {
                    AbstractWindow.Hide();
                    AbstractWindow.Dispose();
                    AbstractWindow = null;
                }
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
        ~AbstractWindowController()
        {
            Dispose(false);
        }
    }
}
