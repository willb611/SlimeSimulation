using System;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;

namespace SlimeSimulation.Controller
{
  public class MainView : IDisposable
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    protected bool disposed = false;
    protected bool running = false;

    public MainView()
    {
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

    public void Display(WindowTemplate window)
    {
      try
      {
        logger.Debug("[Display] About to display window {0}, running: {1}", window, running);
        window.Display();
        //if (!running) {
        //  logger.Debug("[Display] About to set running to true");
        //   running = true;
        Application.Run();
        //  }
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