using SlimeSimulation.FlowCalculation;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowsComponentController;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows.Templates;

namespace SlimeSimulation.View.Windows
{
    class FlowResultWindow : GraphDrawingWindowTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly FlowResult _flowResult;
        private FlowResultWindowController _windowController;

        public FlowResultWindow(FlowResult flowResult, FlowResultWindowController windowController) : base("Flow result", windowController)
        {
            _flowResult = flowResult;
            _windowController = windowController;
            Logger.Debug("[constructor] Finished");
        }

        protected override void AddToWindow(Window window)
        {
            Gdk.Color bgColor = new Gdk.Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            HBox hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            GraphDrawingArea = new GraphDrawingArea(_flowResult.Edges, new FlowResultLineViewController(_flowResult),
              new FlowResultNodeViewController(_flowResult));
            ListenToClicksOn(GraphDrawingArea);

            hbox.PackStart(GraphDrawingArea, true, true, 0);
            hbox.PackStart(new NodeHighlightKey().GetVisualKey(), false, true, 0);

            VBox vbox = new VBox(false, 10);
            vbox.PackStart(new Label("Network with amount of flow: " + _flowResult.FlowAmount), false, true, 10);
            vbox.PackStart(new SimulationStepButton(_windowController), false, true, 10);
            vbox.PackStart(hbox, true, true, 10);

            window.Add(vbox);
        }
    }
}
