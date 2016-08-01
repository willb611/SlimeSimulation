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
            DrawingArea flowResultArea = new GraphDrawingArea(flowResult.Edges, new FlowResultLineWidthController(flowResult),
                new FlowResultNodeHighlightController(flowResult));


            hbox.PackStart(flowResultArea, true, true, 0);
            hbox.PackStart(new NodeHighlightKey().GetVisualKey(), false, true, 0);
            
            VBox vbox = new VBox(false, 10);
            vbox.PackStart(new Label("Network with amount of flow: " + flowResult.FlowAmount), false, true, 10);
            vbox.PackStart(hbox, true, true, 10);
            
            window.Add(vbox);
        }
    }
}
