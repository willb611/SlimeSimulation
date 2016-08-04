using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;

namespace SlimeSimulation.View {
    abstract public class WindowTemplate : IDisposable {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected bool disposed = false;

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

        protected abstract void AddToWindow(Window window);

        private void Window_DeleteEvent(object o, DeleteEventArgs args) {
            Application.Quit();
        }

        public void Display() {
            AddToWindow(window);
            logger.Debug("[Display] Displaying..");
            window.ShowAll();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            } else if (disposing) {
                window.Dispose();
            }
            disposed = true;
        }
    }
}
