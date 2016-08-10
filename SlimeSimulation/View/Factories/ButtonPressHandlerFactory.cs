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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Gtk.Widget widget;
        private OnClickCallback callback;
        public ButtonPressHandlerFactory(Gtk.Widget widget, OnClickCallback callback)
        {
            this.widget = widget;
            this.callback = callback;
        }
        public void ButtonPressHandler(object obj, Gtk.ButtonPressEventArgs args)
        {
            logger.Debug("[ButtonPressHandler] Given args: {0}, x: {1}, y: {2}, type: {3}", args, args.Event.X, args.Event.Y,
              args.Event.Type);
            callback(widget, args);
        }
    }
}
