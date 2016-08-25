using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Controller;
using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.View.Factories
{
    public class ButtonPressHandlerFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Gtk.Widget _widget;
        private readonly OnClickCallback _callback;
        public ButtonPressHandlerFactory(Gtk.Widget widget, OnClickCallback callback)
        {
            _widget = widget;
            _callback = callback;
        }
        public void ButtonPressHandler(object obj, Gtk.ButtonPressEventArgs args)
        {
            Logger.Debug("[ButtonPressHandler] Given args: {0}, x: {1}, y: {2}, type: {3}", args, args.Event.X, args.Event.Y,
              args.Event.Type);
            _callback(_widget, args);
        }
    }
}
