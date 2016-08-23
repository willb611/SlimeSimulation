using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller.WindowController.Templates
{
    public abstract class WindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected WindowTemplate Window;

        public virtual void OnWindowClose()
        {
            Logger.Debug("[OnWindowClose] About to dispose of window: {0}", Window);
            Window.Dispose();
            Logger.Debug("[OnWindowClose] Disposed of window.");
        }

        public abstract void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args);
        public abstract void Render();
    }
}
