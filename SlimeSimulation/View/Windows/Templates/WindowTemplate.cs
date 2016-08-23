using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using NLog;
using SlimeSimulation.Controller;
using SlimeSimulation.View.Factories;
using SlimeSimulation.Controller.WindowController;

namespace SlimeSimulation.View.Windows.Templates
{
    abstract public class WindowTemplate : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed = false;
        private readonly WindowController _controller;

        private readonly Window _window;

        public Window Window {
            get { return _window; }
        }

        public WindowTemplate(String windowTitle, WindowController controller)
        {
            this._controller = controller;
            _window = new Window(windowTitle);
            _window.Maximize();
            //window.Resize(600, 600);
            _window.DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object o, DeleteEventArgs args)
        {
            _controller.OnQuit();
        }

        protected void ListenToClicksOn(Gtk.Widget widget)
        {
            var factory = new ButtonPressHandlerFactory(widget, _controller.OnClickCallback);
            Logger.Debug("[ListenToClicksOn] Attaching to widget: {0}, using controllers OnClickCallback: {1}",
                widget, _controller);
            ListenToClicksOn(widget, factory);
        }
        protected void ListenToClicksOn(Gtk.Widget widget, ButtonPressHandlerFactory factory)
        {
            Logger.Debug("[ListenToClicksOn] Attaching to widget: {0}, using factory: {1}", widget, factory);
            widget.Events |= Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask;
            widget.ButtonPressEvent += new ButtonPressEventHandler(factory.ButtonPressHandler);
        }

        protected abstract void AddToWindow(Window window);

        public void InitialDisplay()
        {
            AddToWindow(_window);
            Logger.Debug("[Display] Displaying..");
            Display();
            GtkLifecycleController.Instance.ApplicationRun();
        }

        public void Display()
        {
            Logger.Debug("[Display] Called from {0}", this);
            _window.ShowAll();
        }

        public void Hide()
        {
            Logger.Debug("[Hide] Called from {0}", this);
            _window.HideAll();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Logger.Debug("[Dispose] Overriden method called from within " + this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            else if (disposing)
            {
                GtkLifecycleController.Instance.ApplicationQuit();
                _window.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
