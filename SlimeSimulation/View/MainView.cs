using System;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;

namespace SlimeSimulation.View {
    public class MainView : IDisposable {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected bool disposed = false;

        public MainView() {
            logger.Info("Entered constructor");
            Application.Init();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            } else if (disposing) {
                logger.Info("[Dispose] Quitting application");
                Application.Quit();
            }
            disposed = true;
        }

        public void Display(WindowTemplate window) {
            window.Display();
            Application.Run();
        }
    }
}
