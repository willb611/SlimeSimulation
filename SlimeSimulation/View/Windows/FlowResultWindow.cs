using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Controller;

namespace SlimeSimulation.View.Windows
{
    class FlowResultWindow : GraphDrawingWindowTemplate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private FlowResult flowResult;
        private FlowResultController controller;

        public FlowResultWindow(FlowResult flowResult, FlowResultController controller) : base("Flow result", controller)
        {
            this.flowResult = flowResult;
            this.controller = controller;
            logger.Debug("[constructor] Finished");
        }

        protected override void AddToWindow(Window window)
        {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            graphDrawingArea = new GraphDrawingArea(flowResult.Edges, new FlowResultLineViewController(flowResult),
              new FlowResultNodeViewController(flowResult));
            base.ListenToClicksOn(graphDrawingArea);

            hbox.PackStart(graphDrawingArea, true, true, 0);
            hbox.PackStart(new NodeHighlightKey().GetVisualKey(), false, true, 0);

            VBox vbox = new VBox(false, 10);
            vbox.PackStart(new Label("Network with amount of flow: " + flowResult.FlowAmount), false, true, 10);
            vbox.PackStart(hbox, true, true, 10);

            window.Add(vbox);
        }
    }
}
