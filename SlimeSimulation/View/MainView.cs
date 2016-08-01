using System;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Model;
using System.Collections.Generic;

namespace SlimeSimulation.View {
    public class MainView : IDisposable {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainView() {
            logger.Info("Entered constructor");
            Application.Init();
        }

        public void Dispose() {
            logger.Info("[Dispose] Quitting application");
            Application.Quit();
        }

        public void Display(WindowTemplate window) {
            window.Display();
            Application.Run();
        }
    }
}
