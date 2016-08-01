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
            AddEvent(flowResultArea);
        }

        private void AddEvent(DrawingArea drawingArea) {
            drawingArea.Events |= Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask;
            drawingArea.ButtonPressEvent += new ButtonPressEventHandler(ButtonPressHandler);
        }

        private void ButtonPressHandler(object obj, ButtonPressEventArgs args) {
            logger.Info("[ButtonPressHandler] Given args: {0}, x: {1}, y: {2}, type: {3}", args, args.Event.X, args.Event.Y, args.Event.Type);
        }
    }
}
