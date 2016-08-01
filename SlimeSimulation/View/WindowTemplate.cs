using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;

namespace SlimeSimulation.View {
    class WindowTemplate {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Window window;
        public Window Window {
            get {
                return window;
            }
        }

        public WindowTemplate(String windowTitle) {
            window = new Window(windowTitle);
            //myWindow.Maximize();
            window.Resize(600, 600);
            window.DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object o, DeleteEventArgs args) {
            Application.Quit();
        }

        public void Display() {
            logger.Debug("[Display] Displaying..");
            window.ShowAll();
        }
    }
}
