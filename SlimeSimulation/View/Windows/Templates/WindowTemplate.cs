using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Controller;
using SlimeSimulation.View.Factories;

namespace SlimeSimulation.View
{
    abstract public class WindowTemplate : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected bool disposed = false;
        private WindowController controller;

        private Window window;

        public Window Window {
            get { return window; }
        }

        public WindowTemplate(String windowTitle, WindowController controller)
        {
            this.controller = controller;
            window = new Window(windowTitle);
            window.Maximize();
            //window.Resize(600, 600);
            window.DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object o, DeleteEventArgs args)
        {
            controller.OnQuit();
        }

        protected void ListenToClicksOn(Gtk.Widget widget)
        {
            var factory = new ButtonPressHandlerFactory(widget, controller.OnClickCallback);
            logger.Debug("[ListenToClicksOn] Attaching to widget: {0}, using controllers OnClickCallback: {1}",
                widget, controller);
            ListenToClicksOn(widget, factory);
        }
        protected void ListenToClicksOn(Gtk.Widget widget, ButtonPressHandlerFactory factory)
        {
            logger.Debug("[ListenToClicksOn] Attaching to widget: {0}, using factory: {1}", widget, factory);
            widget.Events |= Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask;
            widget.ButtonPressEvent += new ButtonPressEventHandler(factory.ButtonPressHandler);
        }

        protected abstract void AddToWindow(Window window);

        public void InitialDisplay()
        {
            AddToWindow(window);
            logger.Debug("[Display] Displaying..");
            Display();
            GtkLifecycleController.Instance.ApplicationRun();
        }

        public void Display()
        {
            logger.Debug("[Display] Called from {0}", this);
            window.ShowAll();
        }

        public void Hide()
        {
            logger.Debug("[Hide] Called from {0}", this);
            window.HideAll();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            logger.Debug("[Dispose] Overriden method called from within " + this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            else if (disposing)
            {
                GtkLifecycleController.Instance.ApplicationQuit();
                window.Dispose();
            }
            disposed = true;
            logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
