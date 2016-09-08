using Gtk;
using NLog;
using SlimeSimulation.Algorithms.FlowCalculation;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows;

namespace SlimeSimulation.Controller.WindowController
{
    class FlowResultWindowController : SimulationStepAbstractWindowController
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
            using (AbstractWindow = new FlowResultWindow(_flowResult, this))
            {
                SimulationController.Display(AbstractWindow);
                Logger.Debug("[Render]  _gtkLifecycleController.Display(Window); has finished running");
            }
        }
        
        public override void OnClickCallback(Widget widget, ButtonPressEventArgs args)
        {
            Logger.Debug("[OnClick] Clicked!");
            GraphDrawingArea area = widget as GraphDrawingArea;
            area?.InvertEdgeDrawing();
        }
    }
}
