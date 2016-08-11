using System;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;
using System.Threading;

namespace SlimeSimulation.View
{
    public class GtkLifecycleController : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected bool disposed = false;
        private int runningCount = 0;

        public static GtkLifecycleController instance;

        public static GtkLifecycleController Instance {
            get {
                return instance;
            }
        }

        internal void ApplicationRun()
        {
            Interlocked.Increment(ref runningCount);
            logger.Debug("[ApplicationRun] After incrementing, count: {0}", runningCount);
            Application.Run();
        }

        internal void ApplicationQuit()
        {
            Interlocked.Decrement(ref runningCount);
            logger.Debug("[ApplicationQuit] After decrementing, count: {0}", runningCount);
            Application.Quit();
        }

        public GtkLifecycleController()
        {
            if (instance != null)
            {
                throw new ApplicationException("Attempting to make multiple views");
            } else
            {
                instance = this;
            }
            logger.Info("Entered constructor");
            Application.Init();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            else if (disposing)
            {
                logger.Info("[Dispose] Quitting application");
                ApplicationQuit();
            }
            disposed = true;
        }

        public void StartBeingAbleToDisplay()
        {
            ApplicationRun();
        }

        public void Display(WindowTemplate window)
        {
            try
            {
                logger.Debug("[Display] About to display window {0}", window);
                window.InitialDisplay();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error: ");
            }
            finally
            {
            }
        }
    }
}
