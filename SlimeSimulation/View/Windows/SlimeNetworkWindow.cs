using System.Collections.Generic;
using Gdk;
using Gtk;
using NLog;
using SlimeSimulation.Controller.WindowComponentController;
using SlimeSimulation.Controller.WindowController;
using SlimeSimulation.Model;
using SlimeSimulation.View.Factories;
using SlimeSimulation.View.WindowComponent;
using SlimeSimulation.View.Windows.Templates;
using Window = Gtk.Window;

namespace SlimeSimulation.View.Windows
{
    class SlimeNetworkWindow : GraphDrawingWindowTemplate
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<SlimeEdge> _edges;
        private readonly SlimeNetworkWindowController _controller;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;

        private CheckButton _shouldSimulationStepsBeDisplayedButton;
        
        public SlimeNetworkWindow(List<SlimeEdge> edges, SlimeNetworkWindowController controller, ISimulationControlBoxFactory simulationControlBoxFactory)
          : base("SlimeNetworkWindow", controller)
        {
            _edges = edges;
            _controller = controller;
            _simulationControlBoxFactory = simulationControlBoxFactory;
        }

        public override void Display()
        {
            _shouldSimulationStepsBeDisplayedButton.Active = _controller.WillFlowResultsBeDisplayed;
            base.Display();
        }

        protected override void AddToWindow(Window window)
        {
            var bgColor = new Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);

            var hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            GraphDrawingArea = new GraphDrawingArea(new List<Edge>(_edges), new ConnectivityLineViewController(_edges),
              new ConnectivityNodeViewController());
            ListenToClicksOn(GraphDrawingArea);

            hbox.Add(GraphDrawingArea);

            var vbox = new VBox(false, 10);
            vbox.PackStart(SimulationStateInterface(), false, true, 10);
            vbox.PackStart(ShouldSimulationStepResultsBeDisplayedInput(), false, true, 10);
            vbox.PackStart(hbox, true, true, 10);

            window.Add(vbox);
        }

        private Widget SimulationStateInterface()
        {
            return new VBox {StepNumberLabel(), _simulationControlBoxFactory.MakeControlBox(_controller, Window)};
        }

        private Widget ShouldSimulationStepResultsBeDisplayedInput()
        {
            _shouldSimulationStepsBeDisplayedButton = new CheckButton("Should simulation step result (flow graph) be displayed?");
            _logger.Debug("[ShouldSimulationStepResultsBeDisplayedInput] Setting initial value to: " + _controller.WillFlowResultsBeDisplayed);

            _shouldSimulationStepsBeDisplayedButton.Active = _controller.WillFlowResultsBeDisplayed;

            _shouldSimulationStepsBeDisplayedButton.Toggled += delegate
            {
                _controller.ToggleAreFlowResultsDisplayed(_shouldSimulationStepsBeDisplayedButton.Active);
            };
            return _shouldSimulationStepsBeDisplayedButton;
        }

        public void UpdateWillFlowResultsBeDisplayed(bool willFlowResultsBeDisplayed)
        {
            _shouldSimulationStepsBeDisplayedButton.Active = willFlowResultsBeDisplayed;
        }

        private Label StepNumberLabel()
        {
            var stepNumber = _controller.StepsSoFarInSimulation();
            return new Label("Simulation steps completed: " + stepNumber);
        }
    }
}
