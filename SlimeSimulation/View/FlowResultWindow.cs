using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;


namespace SlimeSimulation.View {
    class FlowResultWindow {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private FlowResult flowResult;
        private readonly Window thisWindow;

        public FlowResultWindow(FlowResult flowResult) {
            this.flowResult = flowResult;
            thisWindow = new Window("Flow result");
            //myWindow.Maximize();
            thisWindow.Resize(600, 600);
            AttachFlowResultToWindow(thisWindow);
        }

        private void AttachFlowResultToWindow(Window window) {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            thisWindow.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            thisWindow.Add(hbox);
            DrawingArea flowResultArea = new GraphDrawingArea(flowResult.Edges, new LineWidthController(flowResult));
            hbox.Add(flowResultArea);
        }

        internal void Display() {
            logger.Debug("[Display] Showing window");
            flowResult.LogFlowOnEdges();
            thisWindow.ShowAll();
        }
    }
}
