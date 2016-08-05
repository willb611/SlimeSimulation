using NLog;
using SlimeSimulation.FlowCalculation;
using SlimeSimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeSimulation.Controller {
    class FlowResultController : WindowController {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MainView view;
        private FlowResult flowResult;

        public FlowResultController(MainController main, MainView view, FlowResult flowResult) : base(main) {
            this.view = view;
            this.flowResult = flowResult;
        }

        public override void Render() {
            logger.Debug("[Render] Entered");
            using (window = new FlowResultWindow(flowResult, this)) {
                view.Display(window);
                logger.Debug("[Render] Displayed");
            }
        }

        public override void OnClick() {
            flowResult.Validate();
            //window.InvertEdgeDrawing();
            ((FlowResultWindow) window).GraphDrawingArea.InvertEdgeDrawing();
        }
    }
}