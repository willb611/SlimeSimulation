using System.Collections.Generic;
using System.Linq;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SlimeNetworkWindowController _controller;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;
        private readonly SlimeNetwork _slimeNetwork;
        private readonly GraphWithFoodSources _graphWithFoodSources;
        private readonly ISet<Edge> _edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources;

        private CheckButton _shouldSimulationStepsBeDisplayedButton;
        
        public SlimeNetworkWindow(SlimeNetwork slimeNetwork, GraphWithFoodSources graphWithFoodSources,
            SlimeNetworkWindowController controller, ISimulationControlBoxFactory simulationControlBoxFactory)
          : base("SlimeNetworkWindow", controller)
        {
            _slimeNetwork = slimeNetwork;
            _graphWithFoodSources = graphWithFoodSources;
            _controller = controller;
            _simulationControlBoxFactory = simulationControlBoxFactory;
            _edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources = MakeEdgesFromSlimeOrGraph(slimeNetwork, graphWithFoodSources);
        }

        private ISet<Edge> MakeEdgesFromSlimeOrGraph(SlimeNetwork slimeNetwork, GraphWithFoodSources graphWithFoodSources)
        {
            var edges = new HashSet<Edge>(graphWithFoodSources.Edges);
            foreach (var slimeEdge in slimeNetwork.Edges)
            {
                edges.Remove(slimeEdge.Edge);
                edges.Add(slimeEdge.Edge);
            }
            return edges;
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
            
            var vbox = new VBox(false, 10);
            vbox.PackStart(SimulationStateInterface(), false, true, 10);
            vbox.PackStart(SlimeNetworkDisplay(bgColor), true, true, 10);

            window.Add(vbox);
        }

        private Widget SlimeNetworkDisplay(Color bgColor)
        {
            var hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            GraphDrawingArea = new GraphDrawingArea(_edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources,
                new SlimeLineViewController(_edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources),
                new SlimeNodeViewController(_slimeNetwork.Nodes));
            ListenToClicksOn(GraphDrawingArea);

            hbox.Add(GraphDrawingArea);
            return hbox;;
        }

        private Widget SimulationStateInterface()
        {
            return new VBox {StepNumberLabel(),
                _simulationControlBoxFactory.MakeControlBox(_controller, Window),
                ShouldSimulationStepResultsBeDisplayedInput()};
        }

        private Widget ShouldSimulationStepResultsBeDisplayedInput()
        {
            _shouldSimulationStepsBeDisplayedButton = new CheckButton("Should simulation step result (flow graph) be displayed?");
            Logger.Debug("[ShouldSimulationStepResultsBeDisplayedInput] Setting initial value to: " + _controller.WillFlowResultsBeDisplayed);

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
