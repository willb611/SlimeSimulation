using System;
using System.Threading;
using Gtk;
using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.View
{
    public class GtkLifecycleController : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed;
        private int _runningCount;

        public static GtkLifecycleController instance;

        public static GtkLifecycleController Instance {
            get {
                return instance;
            }
        }

        internal void ApplicationRun()
        {
            Interlocked.Increment(ref _runningCount);
            Logger.Debug("[ApplicationRun] After incrementing, count: {0}", _runningCount);
            Application.Run();
        }

        internal void ApplicationQuit()
        {
            Interlocked.Decrement(ref _runningCount);
            Logger.Debug("[ApplicationQuit] After decrementing, count: {0}", _runningCount);
            Application.Quit();
        }

        public GtkLifecycleController()
        {
            if (instance != null)
            {
                throw new ApplicationException("Attempting to make multiple views");
            }
            instance = this;
            Logger.Info("Entered constructor");
            Application.Init();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                Logger.Info("[Dispose] Quitting application");
                ApplicationQuit();
            }
            Disposed = true;
        }

        public void StartBeingAbleToDisplay()
        {
            ApplicationRun();
        }

        public void Display(WindowTemplate window)
        {
            try
            {
                Logger.Debug("[Display] About to display window {0}", window);
                window.InitialDisplay();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error: ");
            }
            finally
            {
            }
        }
    }
}
