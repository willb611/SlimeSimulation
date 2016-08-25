using Gtk;
using NLog;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.View.Factories
{
    public class ButtonPressHandlerFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Widget _widget;
        private readonly OnClickCallback _callback;
        public ButtonPressHandlerFactory(Widget widget, OnClickCallback callback)
        {
            _widget = widget;
            _callback = callback;
        }
        public void ButtonPressHandler(object obj, ButtonPressEventArgs args)
        {
            Logger.Debug("[ButtonPressHandler] Given args: {0}, x: {1}, y: {2}, type: {3}", args, args.Event.X, args.Event.Y,
              args.Event.Type);
            _callback(_widget, args);
        }
    }
}
