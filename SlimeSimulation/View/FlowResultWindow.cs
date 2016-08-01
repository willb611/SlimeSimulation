using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;


namespace SlimeSimulation.View {
    class FlowResultWindow : WindowTemplate {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private FlowResult flowResult;

        public FlowResultWindow(FlowResult flowResult) : base ("Flow result") {
            this.flowResult = flowResult;
            AttachFlowResultToWindow(this.Window);
        }

        private void AttachFlowResultToWindow(Window window) {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            window.Add(hbox);
            DrawingArea flowResultArea = new GraphDrawingArea(flowResult.Edges, new FlowResultLineWidthController(flowResult),
                new FlowResultNodeHighlightController(flowResult));
            hbox.Add(flowResultArea);
        }
    }
}
