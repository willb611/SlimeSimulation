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

        private static GtkLifecycleController _instance = new GtkLifecycleController();

        public static GtkLifecycleController Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Warn("[get] no instance found, returning a new one");
                    return new GtkLifecycleController();
                }
                else
                {
                    return _instance;
                }
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

        private GtkLifecycleController()
        {
            if (_instance != null)
            {
                throw new ApplicationException("Attempting to make multiple views");
            }
            _instance = this;
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
                Logger.Info("[Dispose] Quitting");
                for (var i = Interlocked.Exchange(ref _runningCount, 0); i > 0; i--)
                {
                    ApplicationQuit();
                }
            }
            Disposed = true;
        }
        ~GtkLifecycleController()
        {
            Dispose(false);
        }

        public void StartBeingAbleToDisplay()
        {
            ApplicationRun();
        }

        public void Display(AbstractWindow abstractWindow)
        {
            try
            {
                Logger.Debug("[Display] About to display window {0}", abstractWindow);
                abstractWindow.InitialDisplay();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error: " + e);
            }
        }
    }
}
