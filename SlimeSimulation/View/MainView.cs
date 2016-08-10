using System;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;

namespace SlimeSimulation.View
{
    public class MainView : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected bool disposed = false;

        public static MainView instance;

        public static MainView Instance {
            get {
                return instance;
            }
        }

        public MainView()
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
                Application.Quit();
            }
            disposed = true;
        }

        public void StartBeingAbleToDisplay()
        {
            Application.Run();
        }

        public void Display(WindowTemplate window)
        {
            try
            {
                logger.Debug("[Display] About to display window {0}", window);
                window.Display();
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
