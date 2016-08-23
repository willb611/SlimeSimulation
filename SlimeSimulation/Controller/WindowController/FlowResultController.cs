using NLog;
using SlimeSimulation.View.Windows;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.Controller.WindowController.Templates;
using SlimeSimulation.View;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller.WindowController
{
    class FlowResultController : SimulationStepWindowController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GtkLifecycleController _gtkLifecycleController;
        private readonly FlowResult _flowResult;

        public FlowResultController(SimulationController main, GtkLifecycleController gtkLifecycleController, FlowResult flowResult) : base(main)
        {
            this._gtkLifecycleController = gtkLifecycleController;
            this._flowResult = flowResult;
        }

        public override void Render()
        {
            Logger.Debug("[Render] Entered");
            using (Window = new FlowResultWindow(_flowResult, this))
            {
                _gtkLifecycleController.Display(Window);
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
