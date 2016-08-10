using NLog;
using SlimeSimulation.View.Windows;
using SlimeSimulation.FlowCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.View;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.Controller
{
    class FlowResultController : SimulationStepWindowController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MainView view;
        private FlowResult flowResult;

        public FlowResultController(SimulationController main, MainView view, FlowResult flowResult) : base(main)
        {
            this.view = view;
            this.flowResult = flowResult;
        }

        public override void Render()
        {
            logger.Debug("[Render] Entered");
            using (window = new FlowResultWindow(flowResult, this))
            {
                view.Display(window);
                logger.Debug("[Render] Displayed");
            }
        }
        
        public override void OnClickCallback(Gtk.Widget widget, Gtk.ButtonPressEventArgs args)
        {
            logger.Debug("[OnClick] Clicked!");
            flowResult.Validate();
            GraphDrawingArea area = widget as GraphDrawingArea;
            if (area != null)
            {
                area.InvertEdgeDrawing();
            }
        }
    }
}
