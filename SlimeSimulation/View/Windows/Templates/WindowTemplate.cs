using System;
using Gdk;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.Factories;
using GC = System.GC;
using Window = Gtk.Window;

namespace SlimeSimulation.View.Windows.Templates
{
    public abstract class WindowTemplate : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected bool Disposed;
        private readonly WindowControllerTemplate _windowController;

        private readonly Window _window;

        public Window Window {
            get { return _window; }
        }

        public WindowTemplate(String windowTitle, WindowControllerTemplate windowController)
        {
            _windowController = windowController;
            _window = new Window(windowTitle);
            _window.Maximize();
            
            _window.DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object o, DeleteEventArgs args)
        {
            _windowController.OnWindowClose();
        }

        protected void ListenToClicksOn(Widget widget)
        {
            var factory = new ButtonPressHandlerFactory(widget, _windowController.OnClickCallback);
            Logger.Debug("[ListenToClicksOn] Attaching to widget: {0}, using controllers OnClickCallback: {1}",
                widget, _windowController);
            ListenToClicksOn(widget, factory);
        }
        protected void ListenToClicksOn(Widget widget, ButtonPressHandlerFactory factory)
        {
            Logger.Debug("[ListenToClicksOn] Attaching to widget: {0}, using factory: {1}", widget, factory);
            widget.Events |= EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
            widget.ButtonPressEvent += factory.ButtonPressHandler;
        }

        protected abstract void AddToWindow(Window window);

        public void InitialDisplay()
        {
            AddToWindow(_window);
            Logger.Debug("[Display] Displaying..");
            Display();
            GtkLifecycleController.Instance.ApplicationRun();
        }

        public virtual void Display()
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
            if (disposing)
            {
                Hide();
                GtkLifecycleController.Instance.ApplicationQuit();
                _window.Dispose();
            }
            Disposed = true;
            Logger.Debug("[Dispose : bool] finished from within " + this);
        }
    }
}
