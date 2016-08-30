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
using SlimeSimulation.View.WindowComponent.SimulationControlComponent;
using SlimeSimulation.View.WindowComponent.SimulationStateDisplayComponent;
using SlimeSimulation.View.Windows.Templates;
using Window = Gtk.Window;

namespace SlimeSimulation.View.Windows
{
    public class SlimeNetworkWindow : GraphDrawingWindowTemplate
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SlimeNetworkWindowController _controller;
        private readonly ISimulationControlBoxFactory _simulationControlBoxFactory;
        private readonly SlimeNetwork _slimeNetwork;
        private readonly GraphWithFoodSources _graphWithFoodSources;
        private readonly ISet<Edge> _edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources;
        
        private AbstractSimulationControlBox _simulationControlBox;
        private StepsTakenInAdaptingSlimeDisplayComponent _stepsTakenInSimulationDisplayComponent;
        private StepsTakenForSlimeToExploreDisplayComponent _stepsTakenForSlimeToExploreDisplayComponent;

        public SlimeNetworkWindow(SlimeNetwork slimeNetwork, GraphWithFoodSources graphWithFoodSources,
            SlimeNetworkWindowController controller, ISimulationControlBoxFactory simulationControlBoxFactory)
          : base("SlimeNetworkWindow", controller)
        {
            _slimeNetwork = slimeNetwork;
            _graphWithFoodSources = graphWithFoodSources;
            _controller = controller;
            _simulationControlBoxFactory = simulationControlBoxFactory;
            _edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources =
                slimeNetwork.GetAllEdgesInGraphReplacingThoseWhichAreSlimed(graphWithFoodSources);
        }

        protected override void AddToWindow(Window window)
        {
            var bgColor = new Color(255, 255, 255);
            window.ModifyBg(StateType.Normal, bgColor);
            
            var vbox = new VBox(false, 10);
            vbox.PackStart(SimulationStateInterface(), false, true, 10);
            vbox.PackStart(SlimeSimulationDisplayParameters(), false, true, 10);
            vbox.PackStart(SlimeNetworkDisplay(bgColor), true, true, 10);

            window.Add(vbox);
        }

        private Widget SlimeSimulationDisplayParameters()
        {
            return new SimulationInitialConfigurationDisplayComponent(_controller);
        }

        private Widget SlimeNetworkDisplay(Color bgColor)
        {
            var hbox = new HBox();
            hbox.ModifyBg(StateType.Normal, bgColor);
            GraphDrawingArea = new GraphDrawingArea(_edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources,
                new SlimeLineViewController(_edgesInSlimeNetworkUnionEdgesFromGraphWithFoodSources),
                new SlimeNodeViewController(_slimeNetwork.NodesInGraph));
            ListenToClicksOn(GraphDrawingArea);

            hbox.Add(GraphDrawingArea);
            return hbox;;
        }

        private Widget SimulationStateInterface()
        {
            _simulationControlBox = _simulationControlBoxFactory.MakeControlBox(_controller, Window);
            _stepsTakenInSimulationDisplayComponent = new StepsTakenInAdaptingSlimeDisplayComponent(_controller);
            _stepsTakenForSlimeToExploreDisplayComponent = new StepsTakenForSlimeToExploreDisplayComponent(_controller);
            return new VBox {
                _stepsTakenForSlimeToExploreDisplayComponent,
                _stepsTakenInSimulationDisplayComponent,
                _simulationControlBox};
        }
    }
}
