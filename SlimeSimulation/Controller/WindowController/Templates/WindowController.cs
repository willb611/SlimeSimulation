using Gtk;
using NLog;
using SlimeSimulation.Controller;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller.WindowController
{
    public abstract class WindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected WindowTemplate Window;

        public virtual void OnQuit()
        {
            Logger.Debug("[OnQuit] About to dispose of window: {0}", Window);
            Window.Dispose();
            Logger.Debug("[OnQuit] Disposed of window.");
        }

        public abstract void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args);
        public abstract void Render();
    }
}
