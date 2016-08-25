using NLog;
using SlimeSimulation.View.Windows;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;

namespace SlimeSimulation.Controller.WindowController
{
    class FlowResultWindowController : SimulationStepWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly FlowResult _flowResult;

        public FlowResultWindowController(SimulationController mainController, FlowResult flowResult) : base(mainController)
        {
            _flowResult = flowResult;
        }

        public override void Render()
        {
            Logger.Debug("[Render] Entered");
            using (Window = new FlowResultWindow(_flowResult, this))
            {
                SimulationController.Display(Window);
                Logger.Debug("[Render]  _gtkLifecycleController.Display(Window); has finished running");
            }
        }
        
        public override void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClick] Clicked!");
            _flowResult.Validate();
            GraphDrawingArea area = widget as GraphDrawingArea;
            if (area != null)
            {
                area.InvertEdgeDrawing();
            }
        }
    }
}
