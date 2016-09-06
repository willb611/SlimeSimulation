using System;
using NLog;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            var applicationStarter = new ApplicationStartWindowController();
            applicationStarter.Render();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Logger.Error("[TaskSchedulerOnUnobservedTaskException] Sender: {0}. Exception: {1}", sender, unhandledExceptionEventArgs.ExceptionObject);
            LogManager.Flush();
        }
    }
}
